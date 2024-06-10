using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using OreasModel;
using OreasServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;

namespace OreasCore.Custom_Classes
{
    public class JobStructure
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public string IP { get; set; }
        public int PortNo { get; set; }
        public int MachineID { get; set; }
        public bool AutoClear { get; set; }

        public JobStructure(int h, int m, string iP, int portNo, int machineID, bool autoClear)
        {
            Hours = h;
            Minutes = m;
            IP = iP;
            PortNo = portNo;
            MachineID = machineID;
            AutoClear = autoClear;
        }

    }
    public static class Jobs
    {
        public static List<JobStructure> Queue = new List<JobStructure>();
    }
    public interface IMyTasks
    {
        Guid ID { get; set; }
        Task MyTaskAsync(int MachineID, string IP, int PortNo, bool AutoClear, CancellationToken token);

    }
    public class MyTasks : IMyTasks
    {
        private readonly ILogger<MyTasks> logger;
        private readonly IMachine machine;

        public Guid ID { get; set; }

        public MyTasks(ILogger<MyTasks> logger, IMachine machine)
        {
            this.logger = logger;
            this.machine = machine;
            ID = Guid.NewGuid();
        }

        public async Task MyTaskAsync(int MachineID, string IP, int PortNo, bool AutoClear, CancellationToken token)
        {


            zkemkeeper.CZKEM ZkMachine = new zkemkeeper.CZKEM();
            try
            {


                if (ZkMachine.Connect_Net(IP, PortNo))
                {
                    logger.LogInformation($"Connected: {IP} @ {DateTime.Now} ");

                    DataTable _table = new DataTable();
                    _table.Columns.Add("ATMachineID", typeof(int));
                    _table.Columns.Add("ATEnrollmentNo", typeof(string));
                    _table.Columns.Add("ATInOutMode", typeof(int));
                    _table.Columns.Add("ATDateTime", typeof(DateTime));
                    int recordNo = 0;
                    int NoOfRecords = 0;
                    if (ZkMachine.ReadAllGLogData(ZkMachine.MachineNumber))
                    {
                        int idwVerifyMode = 0, idwInOutMode = 0, dwWorkCode = 0, idwYear = 0, idwMonth = 0, idwDay = 0, idwHour = 0, idwMinute = 0, idwSecond = 0;
                        string sdwEnrollNumber = "";
                        if (ZkMachine.GetDeviceStatus(ZkMachine.MachineNumber, 6, ref NoOfRecords))
                        {
                            logger.LogInformation($"Fetching Records of {IP} @ {DateTime.Now}");

                            while (ZkMachine.SSR_GetGeneralLogData(ZkMachine.MachineNumber, out sdwEnrollNumber, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref dwWorkCode))
                            {


                                _table.Rows.Add((int)MachineID, sdwEnrollNumber, idwInOutMode, new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond));

                                recordNo = recordNo + 1;
                                //logger.LogInformation($"Fetching Record: {recordNo.ToString()} / {NoOfRecords.ToString()}");

                                await Task.Delay(1, token);
                                if (token.IsCancellationRequested)
                                {
                                    logger.LogInformation($"Cancellation Requested");
                                    break;
                                }
                            }
                        }

                        logger.LogInformation($"Saving Data of: {IP} @ {DateTime.Now} ");

                        string msg = machine.PostMachineAttendance(_table).Result;

                        if (msg == "Successful")
                        {
                            logger.LogInformation($"Sucessfully Saved Record of {IP} @ {DateTime.Now}");

                            if (ZkMachine.GetDeviceStatus(ZkMachine.MachineNumber, 6, ref NoOfRecords) && AutoClear)
                            {
                                logger.LogInformation($"Clearing Data after saving record of: {IP} @ {DateTime.Now} ");

                                if (ZkMachine.ClearGLog(ZkMachine.MachineNumber))
                                {
                                    logger.LogInformation($"Data Cleared after saving record of: {IP} @ {DateTime.Now} ");

                                }
                                else
                                {
                                    logger.LogInformation($"Failed to Clear Data after saving record of: {IP} @ {DateTime.Now} ");
                                }
                            }
                        }
                        else
                            logger.LogInformation($"Exception while saving record of: {IP} @ {DateTime.Now} ");

                    }



                }
                else
                {
                    logger.LogInformation($"Failed To connect: {IP} @ {DateTime.Now} ");
                }




            }
            catch (Exception ex)
            {

                logger.LogInformation($"Exception: {ex.Message} @ {DateTime.Now} ");
            }
            finally
            {
                if (ZkMachine != null)
                {
                    ZkMachine.Disconnect();
                    logger.LogInformation($"Disconnected: {IP} @ {DateTime.Now} ");
                }

            }


        }

    }
    public class MyBackgroundTask : BackgroundService
    {
        private readonly ILogger<MyBackgroundTask> logger;
        private readonly IServiceProvider serviceProvider;
        private PeriodicTimer timer;
        private DateTime CurrentDatetime;
        public DateTime GetCurrentDateTime { get => this.CurrentDatetime; }

    
        public MyBackgroundTask(ILogger<MyBackgroundTask> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            

            CurrentDatetime = DateTime.Now;

            await Task.Delay((60 - CurrentDatetime.Second) * 1000, stoppingToken);

            this.timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            using (var scope = serviceProvider.CreateScope())
            {
                CurrentDatetime = DateTime.Now;

                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    CurrentDatetime = DateTime.Now;

                    foreach (var item in Jobs.Queue.Where(x => x.Hours == CurrentDatetime.Hour && x.Minutes == CurrentDatetime.Minute).ToList())
                    {
                        var scopedService = scope.ServiceProvider.GetRequiredService<IMyTasks>();
                        logger.LogInformation($"Calling Event: {item.IP} @ {DateTime.Now} ");
                        await Task.Factory.StartNew(async () => { await scopedService.MyTaskAsync(item.MachineID, item.IP, item.PortNo, item.AutoClear, stoppingToken).ConfigureAwait(false); }, stoppingToken).ConfigureAwait(false);
                    }

                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("From Background: Stopping..." + DateTime.Now.ToLocalTime());
            return base.StopAsync(cancellationToken);
        }
    }

}
