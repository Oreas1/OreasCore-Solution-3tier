using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MimeKit;
using OreasModel;
using OreasServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace OreasCore.Custom_Classes
{
    public class MachineOperationtHub : Hub
    {
        private readonly ILogger<MachineOperationtHub> logger;
        private readonly IMachine _IMachine;
        private readonly IEmployee _IEmployee;
        private readonly IWebHostEnvironment _IWebHostEnvironment;

        public MachineOperationtHub(ILogger<MachineOperationtHub> logger,IMachine _IMachine, IEmployee _IEmployee, IWebHostEnvironment _IWebHostEnvironment)
        {
            this.logger = logger;
            this._IMachine = _IMachine;
            this._IEmployee = _IEmployee;
            this._IWebHostEnvironment = _IWebHostEnvironment;
        }

        public async Task StartOperation(int _MachineID=0, string _ForProcess = "", int EmpID = 0)
        {       

            var IsInProcess = MachineProcessStatus.MachineProcessStatusList.Where(a => a.MachineID == _MachineID).FirstOrDefault();

            if (IsInProcess!=null)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", "Machine Is busy in Process: " + IsInProcess.ForProcess + " By: " + IsInProcess.UserName);
                return;
            }           
            
            var MachineDetail = await _IMachine.GetMachineObject(_MachineID);

            if (MachineDetail != null)
            {               

                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                MachineProcessStatus.MachineProcessStatusList.Add(new MachineProcessInfo()
                {
                    MachineID = MachineDetail.ID,
                    ConnectionID = Context.ConnectionId,
                    ForProcess = _ForProcess,
                    UserName = Context.User.Identity.Name,
                    cancellationTokenSource = cts
                });
                zkemkeeper.CZKEM ZkMachine  = new zkemkeeper.CZKEM();

                try
                {
                    
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Connecting to the Machine " + MachineDetail.Name + " ....");
                    if (ZkMachine.Connect_Net(MachineDetail.IP, MachineDetail.PortNo))
                    {
                        ZkMachine.EnableDevice(1, false);
                        await Clients.Caller.SendAsync("ClientAcknowledgment", "Connected to the Machine " + MachineDetail.Name);
                        Task.Delay(10).Wait(token);

                        if (_ForProcess == "Download Attendance")
                        {
                            DownloadAttendanceAsync(MachineDetail.ID, MachineDetail.AutoClearLogAfterDownload, _IMachine, ZkMachine, token).Wait(token);
                            Task.Delay(100).Wait(token);

                            Clients.Caller.SendAsync("ClientAcknowledgment", "Sucessfully Done Download Process Completely for Machine: " + MachineDetail.Name).Wait(token);
                            
                        }                            
                        else if (_ForProcess == "Clear Log")
                            await ClearLogAsync(MachineDetail.ID, _IMachine, ZkMachine, token);
                        else if (_ForProcess == "Sync Time From Server")
                            await SynMachineTimeFromServerAsync(ZkMachine, token);
                        else if (_ForProcess == "Get DateTime From Machine")
                            await GetDateTimeFromMachineAsync(ZkMachine, token);
                        else if (_ForProcess == "Restart Machine")
                            await RestartMachineAsync(ZkMachine, token);
                        else if (_ForProcess == "ShutDown Machine")
                            await ShutDownMachineAsync(ZkMachine, token);
                        else if (_ForProcess == "Remove User From Machine")
                            await RemoveUserAsync(EmpID,ZkMachine, token);
                        else if (_ForProcess == "Synchronize User Card Only")
                            await SynchronizeUserCardOnlyAsync(EmpID, ZkMachine, token);
                        else if (_ForProcess == "Synchronize User Face Template Only")
                            await SynchronizeFaceTemplateOnlyAsync(EmpID, ZkMachine, _MachineID, token);
                        else if (_ForProcess == "Synchronize User Full Template")
                            await SynchronizeFullTemplateAsync(EmpID, ZkMachine, _MachineID, token);
                        else if (_ForProcess == "Synchronize User Full Template All")
                            await SynchronizeFullTemplateAllAsync(EmpID, ZkMachine, _MachineID, token);
                        else
                            await Clients.Caller.SendAsync("ClientAcknowledgment", "Cannot Processed of Unknown Process: " + _ForProcess);
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("ClientAcknowledgment", "Unable to Connect: " + MachineDetail.Name);
                        Task.Delay(1000).Wait(token);
                    }
                }
                catch (Exception ex)
                {
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Some thing went wrong while operating on machine");
                    Task.Delay(1000).Wait(token);
                }
                finally
                {
                    ZkMachine.EnableDevice(1, true);
                    ZkMachine.Disconnect();
                }

                if (cts.IsCancellationRequested)
                {
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Process: " + _ForProcess + " has been Aborted by User");
                }

                await ReleaseResource(Context.ConnectionId);

            }        

        }

        public async Task CancelOperation()
        {
            await ReleaseResource(Context.ConnectionId);
        }

        public async Task<Object> GetOperatorDetail(int _MachineID = 0)
        {
            Object obj = null;
            await Task.Factory.StartNew(()=>
            {
                var machineProcessStatus = MachineProcessStatus.MachineProcessStatusList?.Where(a => a.MachineID == _MachineID).FirstOrDefault();
                if (machineProcessStatus != null)
                {
                    obj = new { ForProcess = machineProcessStatus.ForProcess, UserName = machineProcessStatus.UserName, MachineID = _MachineID };
                    //return ();
                }
                else
                    obj = new { ForProcess = "Not In Process", UserName = "No User", MachineID = _MachineID };
                    //return (new { ForProcess = "Not In Process", UserName = "No User", MachineID = _MachineID });                
            }         
            );

            return obj;
     
        }

        public async Task ReleaseResource(string _ConnectionId)
        {
            await Task.Factory.StartNew(() => {
                var machineProcessStatus = MachineProcessStatus.MachineProcessStatusList.Where(w => w.ConnectionID == _ConnectionId).ToList();
                if (machineProcessStatus != null)
                {
                    foreach (var item in machineProcessStatus)
                    {
                        item.cancellationTokenSource.Cancel();
                        item.cancellationTokenSource.Dispose();
                        MachineProcessStatus.MachineProcessStatusList.Remove(item);
                    }
                }
            });
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ReleaseResource(Context.ConnectionId).Wait();
            return base.OnDisconnectedAsync(exception);
        }

        //------------------------Machine General Process Funtions-----------------------//
        private async Task DownloadAttendanceAsync(int MachineID,bool AutoClear, IMachine _IMachine, zkemkeeper.CZKEM ZkMachine, CancellationToken token)
        {
            try
            {
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
                        while (ZkMachine.SSR_GetGeneralLogData(ZkMachine.MachineNumber, out sdwEnrollNumber, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref dwWorkCode))
                        {
                            _table.Rows.Add((int)MachineID, sdwEnrollNumber, idwInOutMode, new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond));

                            recordNo = recordNo + 1;
                            await Clients.Caller.SendAsync("ClientAcknowledgment", "Fetching Record " + recordNo + " / " + NoOfRecords + " ....");
                            Task.Delay(10).Wait(token);
                            if (Context.ConnectionAborted.IsCancellationRequested)
                            {
                                await Clients.Caller.SendAsync("ClientAcknowledgment", "ConnectionAborted.IsCancellationRequested called");
                                logger.LogInformation("SignalR Exception: ConnectionAborted.IsCancellationRequested called");
                                break;                                
                            }
                        }
                    }
                }

                await Clients.Caller.SendAsync("ClientAcknowledgment", "Fetched: " + recordNo + " / " + NoOfRecords + "; Now saving to data please wait..");
                Task.Delay(10).Wait(token);
                string msg = await _IMachine.PostMachineAttendance(_table);
                Task.Delay(10).Wait(token);

                if (msg == "Successful")
                {
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Sucessfully Done Download Process of Record: " + recordNo + " / " + NoOfRecords);
                    Task.Delay(10).Wait(token);

                    if (ZkMachine.GetDeviceStatus(ZkMachine.MachineNumber, 6, ref NoOfRecords) && AutoClear)
                    {
                        await Clients.Caller.SendAsync("ClientAcknowledgment", "clearing called");

                        if (ZkMachine.ClearGLog(ZkMachine.MachineNumber))
                        {
                            await Clients.Caller.SendAsync("ClientAcknowledgment", "cleared");

                        }
                        else
                        {
                            await Clients.Caller.SendAsync("ClientAcknowledgment", "clearing failed");
                        }
                    }

                }
                else
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Failed to Update the Record: " + recordNo + " / " + NoOfRecords);

                Task.Delay(10).Wait(token);

            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", "Exception: " + ex.Message);
                logger.LogInformation("SignalR Exception: " + ex.Message);
            }
        }
        private async Task ClearLogAsync(int MachineID, IMachine _IMachine, zkemkeeper.CZKEM ZkMachine, CancellationToken token)
        {
            int NoOfRecords = 0;
            if (ZkMachine.GetDeviceStatus(ZkMachine.MachineNumber, 6, ref NoOfRecords))
            {
                if (ZkMachine.ClearGLog(ZkMachine.MachineNumber))
                {
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Log has been succesfully cleared from Machine, now updating database..");
                    Task.Delay(10).Wait(token);

                    await _IMachine.PostMachineAttendanceClear(MachineID, NoOfRecords);
                    Task.Delay(10).Wait(token);

                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Sucessfully Done Clear Log Process");
                    Task.Delay(10).Wait(token);

                }
                else
                {
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Some thing went wrong while clearing log from Machine");
                }
            }
            Task.Delay(1000).Wait(token);
        }
        private async Task SynMachineTimeFromServerAsync(zkemkeeper.CZKEM ZkMachine, CancellationToken token)
        {
            if (ZkMachine.SetDeviceTime(ZkMachine.MachineNumber))
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", "Machine Time has been synced succesfully from Server Time");
            }
            else
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", "Some thing went wrong while syncing Machine Time From Server");
            }
            Task.Delay(1000).Wait(token);
        }
        private async Task GetDateTimeFromMachineAsync(zkemkeeper.CZKEM ZkMachine, CancellationToken token)
        {
            int _year = 1999, _month = 01, _day = 01, _hour = 0, _minute = 0, _second = 0;

            if (ZkMachine.GetDeviceTime(ZkMachine.MachineNumber, ref _year, ref _month, ref _day, ref _hour, ref _minute, ref _second))
            {

                await Clients.Caller.SendAsync("ClientAcknowledgment", "Date and Time of The machine is " + new DateTime(_year, _month, _day, _hour, _minute, _second).ToString("dd-MMM-yyyy hh:mm:ss tt"));
            }
            else
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", "Some thing went wrong while fetching Machine DateTime");
            }
            Task.Delay(1000).Wait(token);
        }
        private async Task RestartMachineAsync(zkemkeeper.CZKEM ZkMachine, CancellationToken token)
        {
            if (ZkMachine.RestartDevice(ZkMachine.MachineNumber))
            {

                await Clients.Caller.SendAsync("ClientAcknowledgment", "Restart command has been sent sucessfully! Machine will take few minutes to restart");
            }
            else
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", "Some thing went wrong while sending Restart command to the Machine");
            }
            Task.Delay(1000).Wait(token);
        }
        private async Task ShutDownMachineAsync(zkemkeeper.CZKEM ZkMachine, CancellationToken token)
        {
            if (ZkMachine.PowerOffDevice(ZkMachine.MachineNumber))
            {

                await Clients.Caller.SendAsync("ClientAcknowledgment", "Shutdown command has been sent sucessfully! Machine will be shutted down in few minutes");
            }
            else
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", "Some thing went wrong while sending Shutdown command to the Machine");
            }
            Task.Delay(1000).Wait(token);
        }

        //------------------------Machine User Process Funtions-----------------------//
        private async Task SynchronizeFullTemplateAsync(int EmpID, zkemkeeper.CZKEM machine, int _MachineID, CancellationToken token)
        {
            try
            {
                tbl_WPT_Employee Emp = _IEmployee.GetEmployeeObject(EmpID).Result;
                tbl_WPT_Employee_PFF templateObj = _IEmployee.GetEmployeeFFCPTemplateObject(EmpID).Result;

                await Clients.Caller.SendAsync("ClientAcknowledgment", "AT :" + Emp.ATEnrollmentNo_Default);

                if (!string.IsNullOrEmpty(Emp.ATEnrollmentNo_Default))
                {

                    #region User Information 

                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Getting User Information From Machine for :" + Emp.ATEnrollmentNo_Default);
                    Task.Delay(1).Wait(token);

                    string username = "", paswsd = ""; int privilege = 0; bool enabled = true;
                    machine.SSR_GetUserInfo(machine.MachineNumber, Emp.ATEnrollmentNo_Default, out username, out paswsd, out privilege, out enabled);

                    string cardNumber = "";
                    machine.GetStrCardNumber(out cardNumber);

                    if (string.IsNullOrEmpty(templateObj.CardNumber) && cardNumber != "0")
                        templateObj.CardNumber = cardNumber;

                    if (string.IsNullOrEmpty(templateObj.Password) && !string.IsNullOrEmpty(paswsd))
                        templateObj.Password = paswsd;


                    await Clients.Caller.SendAsync("ClientAcknowledgment", "updating User Information From Profile to Machine for :" + Emp.ATEnrollmentNo_Default);
                    Task.Delay(1).Wait(token);
                    machine.SSR_SetUserInfo(machine.MachineNumber, Emp.ATEnrollmentNo_Default, Emp.EmployeeName, templateObj.Password, templateObj?.Privilege ?? 0, templateObj.Enabled);

                    #endregion

                    #region User Photo                

                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Synchronizing User Photo for: " + Emp.ATEnrollmentNo_Default);
                    Task.Delay(1).Wait(token);

             

                    string filename = Emp.ATEnrollmentNo_Default + ".jpg";
                    var filepath = _IWebHostEnvironment.WebRootPath + "\\images\\temp\\";         

                    if (!string.IsNullOrEmpty(templateObj.Photo160X210))
                    {
                        System.IO.File.WriteAllBytes(Path.Combine(filepath, filename), System.Convert.FromBase64String(templateObj.Photo160X210));
                        if (machine.UploadUserPhoto(machine.MachineNumber, Path.Combine(filepath, filename)))
                            await Clients.Caller.SendAsync("ClientAcknowledgment", "Photo Uploaded to Machine from Profile");
                        else
                            await Clients.Caller.SendAsync("ClientAcknowledgment", "Failed Photo Uploaded to Machine from Profile");

                    }
                    else
                    {
                        if (machine.DownloadUserPhoto(machine.MachineNumber, filename, filepath))
                        {
                            byte[] photo;
                            photo = System.IO.File.ReadAllBytes(filepath + filename);
                            if (photo != null)
                            {
                                templateObj.Photo160X210 = System.Convert.ToBase64String(photo);
                                await Clients.Caller.SendAsync("ClientAcknowledgment", "Photo Downloaed from Machine for Profile");
                            }
                            else
                                await Clients.Caller.SendAsync("ClientAcknowledgment", "No Photo found from Machine to download for Profile");

                        }
                    }

                    if (System.IO.File.Exists(filepath + filename))
                        System.IO.File.Delete(filepath + filename);


                    #endregion

                    #region Face Template

                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Synchronizing face template for: " + Emp.ATEnrollmentNo_Default);
                    Task.Delay(1).Wait(token);

                    if (templateObj.FaceTemplate != null)
                    {
                        
                        machine.SetUserFaceStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 50, templateObj.FaceTemplate, templateObj.FaceTemplate.Length);
                    }
                    else
                    {
                        string facetemplatestr = ""; int facetemplatelen = 0;

                        if (machine.GetUserFaceStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 50, ref facetemplatestr, ref facetemplatelen))
                            templateObj.FaceTemplate = facetemplatestr;
                    }
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Synchroned face template for: " + Emp.ATEnrollmentNo_Default);
                    Task.Delay(1).Wait(token);

                    #endregion

                    #region Finger Template

                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Synchronizing finger template for: " + Emp.ATEnrollmentNo_Default);
                    Task.Delay(1).Wait(token);

                    machine.ReadAllUserID(machine.MachineNumber);//read all the user information to the memory  except fingerprint Templates
                    machine.ReadAllTemplate(machine.MachineNumber);//read all the users' fingerprint templates to the memory

                    string fingerTemp = ""; int fingertTempLen = 0; int flag_Valid_duress = 1;

                    //----------------------------------------------------------Finger 0-------------------------------------------//
                    if (templateObj.FingerTemplate0 == null)
                    {
                        fingerTemp = ""; fingertTempLen = 0; flag_Valid_duress = 1;
                        if (machine.GetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 0, out flag_Valid_duress, out fingerTemp, out fingertTempLen))
                        {
                            templateObj.FingerTemplate0 = fingerTemp;
                        }
                    }
                    else
                    {
                        if (machine.SetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 0, 1, templateObj.FingerTemplate0))
                        {

                        }
                    }
                    //----------------------------------------------------------Finger 1-------------------------------------------//
                    if (templateObj.FingerTemplate1 == null)
                    {
                        fingerTemp = ""; fingertTempLen = 0; flag_Valid_duress = 1;
                        if (machine.GetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 1, out flag_Valid_duress, out fingerTemp, out fingertTempLen))
                        {
                            if (!string.IsNullOrEmpty(fingerTemp))
                                templateObj.FingerTemplate1 = fingerTemp;
                        }
                    }
                    else
                    {
                        if (machine.SetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 1, 1, templateObj.FingerTemplate1))
                        {

                        }
                    }
                    //----------------------------------------------------------Finger 2-------------------------------------------//
                    if (templateObj.FingerTemplate2 == null)
                    {
                        fingerTemp = ""; fingertTempLen = 0; flag_Valid_duress = 1;
                        if (machine.GetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 2, out flag_Valid_duress, out fingerTemp, out fingertTempLen))
                        {
                            if (!string.IsNullOrEmpty(fingerTemp))
                                templateObj.FingerTemplate2 = fingerTemp;
                        }
                    }
                    else
                    {
                        if (machine.SetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 2, 1, templateObj.FingerTemplate2))
                        {

                        }
                    }
                    //----------------------------------------------------------Finger 3-------------------------------------------//
                    if (templateObj.FingerTemplate3 == null)
                    {
                        fingerTemp = ""; fingertTempLen = 0; flag_Valid_duress = 1;
                        if (machine.GetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 3, out flag_Valid_duress, out fingerTemp, out fingertTempLen))
                        {
                            if (!string.IsNullOrEmpty(fingerTemp))
                                templateObj.FingerTemplate3 = fingerTemp;
                        }
                    }
                    else
                    {
                        if (machine.SetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 3, 1, templateObj.FingerTemplate3))
                        {

                        }
                    }
                    //----------------------------------------------------------Finger 4-------------------------------------------//
                    if (templateObj.FingerTemplate4 == null)
                    {
                        fingerTemp = ""; fingertTempLen = 0; flag_Valid_duress = 1;
                        if (machine.GetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 4, out flag_Valid_duress, out fingerTemp, out fingertTempLen))
                        {
                            if (!string.IsNullOrEmpty(fingerTemp))
                                templateObj.FingerTemplate4 = fingerTemp;
                        }
                    }
                    else
                    {
                        if (machine.SetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 4, 1, templateObj.FingerTemplate4))
                        {

                        }
                    }
                    //----------------------------------------------------------Finger 5-------------------------------------------//
                    if (templateObj.FingerTemplate5 == null)
                    {
                        fingerTemp = ""; fingertTempLen = 0; flag_Valid_duress = 1;
                        if (machine.GetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 5, out flag_Valid_duress, out fingerTemp, out fingertTempLen))
                        {
                            if (!string.IsNullOrEmpty(fingerTemp))
                                templateObj.FingerTemplate5 = fingerTemp;
                        }
                    }
                    else
                    {
                        if (machine.SetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 5, 1, templateObj.FingerTemplate5))
                        {

                        }
                    }
                    //----------------------------------------------------------Finger 6-------------------------------------------//
                    if (templateObj.FingerTemplate6 == null)
                    {
                        fingerTemp = ""; fingertTempLen = 0; flag_Valid_duress = 1;
                        if (machine.GetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 6, out flag_Valid_duress, out fingerTemp, out fingertTempLen))
                        {
                            if (!string.IsNullOrEmpty(fingerTemp))
                                templateObj.FingerTemplate6 = fingerTemp;
                        }
                    }
                    else
                    {
                        if (machine.SetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 6, 1, templateObj.FingerTemplate6))
                        {

                        }
                    }
                    //----------------------------------------------------------Finger 7-------------------------------------------//
                    if (templateObj.FingerTemplate7 == null)
                    {
                        fingerTemp = ""; fingertTempLen = 0; flag_Valid_duress = 1;
                        if (machine.GetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 7, out flag_Valid_duress, out fingerTemp, out fingertTempLen))
                        {
                            if (!string.IsNullOrEmpty(fingerTemp))
                                templateObj.FingerTemplate7 = fingerTemp;
                        }
                    }
                    else
                    {
                        if (machine.SetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 7, 1, templateObj.FingerTemplate7))
                        {

                        }
                    }
                    //----------------------------------------------------------Finger 8-------------------------------------------//
                    if (templateObj.FingerTemplate8 == null)
                    {
                        fingerTemp = ""; fingertTempLen = 0; flag_Valid_duress = 1;
                        if (machine.GetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 8, out flag_Valid_duress, out fingerTemp, out fingertTempLen))
                        {
                            if (!string.IsNullOrEmpty(fingerTemp))
                                templateObj.FingerTemplate8 = fingerTemp;
                        }
                    }
                    else
                    {
                        if (machine.SetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 8, 1, templateObj.FingerTemplate8))
                        {

                        }
                    }
                    //----------------------------------------------------------Finger 9-------------------------------------------//
                    if (templateObj.FingerTemplate9 == null)
                    {
                        fingerTemp = ""; fingertTempLen = 0; flag_Valid_duress = 1;
                        if (machine.GetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 9, out flag_Valid_duress, out fingerTemp, out fingertTempLen))
                        {
                            if (!string.IsNullOrEmpty(fingerTemp))
                                templateObj.FingerTemplate9 = fingerTemp;
                        }
                    }
                    else
                    {
                        if (machine.SetUserTmpExStr(machine.MachineNumber, Emp.ATEnrollmentNo_Default, 9, 1, templateObj.FingerTemplate9))
                        {

                        }
                    }


                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Updating the Profile template for: " + Emp.ATEnrollmentNo_Default);

                    _IEmployee.PostEmployeeFFCPTemplateObject(templateObj, "Save Update", Context.User.Identity.Name).Wait();

                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Sucessfully Synchronized full template for: " + Emp.ATEnrollmentNo_Default);
                    Task.Delay(1).Wait(token);

                    #endregion

                }
                else
                {
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "AT Not found from db :" + Emp.ATEnrollmentNo_Default);
                }


            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", ex.Message);
            }
            finally
            {
                Task.Delay(10).Wait(token);
            }
        }
        private async Task SynchronizeFaceTemplateOnlyAsync(int EmpID, zkemkeeper.CZKEM ZkMachine, int _MachineID, CancellationToken token)
        {
            try
            {
                tbl_WPT_Employee Emp = _IEmployee.GetEmployeeObject(EmpID).Result;
                tbl_WPT_Employee_PFF templateObj = _IEmployee.GetEmployeeFFCPTemplateObject(EmpID).Result;

                await Clients.Caller.SendAsync("ClientAcknowledgment", "AT :" + Emp.ATEnrollmentNo_Default);

                if (!string.IsNullOrEmpty(Emp.ATEnrollmentNo_Default))
                {
                    string filename = Emp.ATEnrollmentNo_Default + ".jpg";
                    var filepath = _IWebHostEnvironment.WebRootPath + "/images/temp/";

                    //---------------------Download photo from database if not exist then download from machine----------
                    if (!string.IsNullOrEmpty(templateObj.Photo160X210))
                    {
                        System.IO.File.WriteAllBytes(filepath + filename, System.Convert.FromBase64String(templateObj.Photo160X210));

                        if (ZkMachine.UploadUserPhoto(ZkMachine.MachineNumber, filepath + filename))
                        {
                            await Clients.Caller.SendAsync("ClientAcknowledgment", "Photo Uploaded to Machine from Profile");
                        }

                    }
                    else
                    {
                        if (ZkMachine.DownloadUserPhoto(ZkMachine.MachineNumber, filename, filepath))
                        {

                            byte[] photo;
                            photo = System.IO.File.ReadAllBytes(filepath + filename);

                            if (photo != null)
                            {
                                templateObj.Photo160X210 = System.Convert.ToBase64String(photo);
                                string msg = _IEmployee.PostEmployeeFFCPTemplate("Save Update", Context.User.Identity.Name, EmpID, templateObj.CardNumber, templateObj.Password, templateObj.Privilege, templateObj.Enabled, false, false, false).Result;
                                if (msg == "OK")
                                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Sucessfully updated Face Template From Machine to profile");
                                else
                                    await Clients.Caller.SendAsync("ClientAcknowledgment", msg);
                                Task.Delay(1000).Wait(token);

                            }

                        }
                        else
                        {
                            await Clients.Caller.SendAsync("ClientAcknowledgment", "user Not Found");
                        }
                    }

                    if (System.IO.File.Exists(filepath + filename))
                    {
                        System.IO.File.Delete(filepath + filename);
                    }

                }
                else
                {
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "AT Not found from db :" + Emp.ATEnrollmentNo_Default);
                }


            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", ex.Message);
            }
            finally
            {
                Task.Delay(1000).Wait(token);
            }
        }
        private async Task SynchronizeUserCardOnlyAsync(int EmpID, zkemkeeper.CZKEM ZkMachine, CancellationToken token)
        {
            try
            {
                tbl_WPT_Employee Emp = _IEmployee.GetEmployeeObject(EmpID).Result;
                tbl_WPT_Employee_PFF templateObj = _IEmployee.GetEmployeeFFCPTemplateObject(EmpID).Result;

                await Clients.Caller.SendAsync("ClientAcknowledgment", "AT :" + Emp.ATEnrollmentNo_Default);

                if (!string.IsNullOrEmpty(Emp.ATEnrollmentNo_Default))
                {

                    if (templateObj.CardNumber == null) //--------------card number--------------//
                    {
                        string username = "", paswsd = "", cardNumber = ""; int privilege = 0; bool enabled = true;

                        if (ZkMachine.SSR_GetUserInfo(ZkMachine.MachineNumber, Emp.ATEnrollmentNo_Default, out username, out paswsd, out privilege, out enabled))
                        {
                            if (ZkMachine.GetStrCardNumber(out cardNumber))
                            {
                                await Clients.Caller.SendAsync("ClientAcknowledgment", "Updating Card Number From Machine to template called....");
                                Task.Delay(10).Wait(token);

                                templateObj.CardNumber = cardNumber != "0" ? cardNumber : null;

                                string msg = _IEmployee.PostEmployeeFFCPTemplate("Save Update", Context.User.Identity.Name, EmpID, templateObj.CardNumber, templateObj.Password, templateObj.Privilege, templateObj.Enabled, false, false, false).Result;

                                if (msg == "OK")
                                    await Clients.Caller.SendAsync("ClientAcknowledgment", "Sucessfully updated Card Number From Machine to template");
                                else
                                    await Clients.Caller.SendAsync("ClientAcknowledgment", msg);
                                Task.Delay(1).Wait(token);
                            }
                            else
                            {
                                await Clients.Caller.SendAsync("ClientAcknowledgment", "No Card Number found on Machine");
                            }
                        }
                        else
                        {
                            await Clients.Caller.SendAsync("ClientAcknowledgment", "User Not Found in Machine");
                        }
                    }
                    else
                    {
                        if (ZkMachine.SetStrCardNumber(templateObj.CardNumber))
                        {
                            await Clients.Caller.SendAsync("ClientAcknowledgment", "setting Card Number from template to machine ....");
                            Task.Delay(10).Wait(token);
                        }
                    }

                }
                else
                {
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "AT Not found from db :" + Emp.ATEnrollmentNo_Default);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", ex.Message);
            }
            finally
            {
                Task.Delay(1000).Wait(token);
            }
        }
        private async Task RemoveUserAsync(int EmpID, zkemkeeper.CZKEM ZkMachine, CancellationToken token)
        {
            try
            {
                tbl_WPT_Employee Emp = _IEmployee.GetEmployeeObject(EmpID).Result;

                await Clients.Caller.SendAsync("ClientAcknowledgment", "AT :" + Emp.ATEnrollmentNo_Default);

                if (!string.IsNullOrEmpty(Emp.ATEnrollmentNo_Default))
                {
                    if (ZkMachine.DelUserFace(ZkMachine.MachineNumber, Emp.ATEnrollmentNo_Default, 50))
                    {
                        await Clients.Caller.SendAsync("ClientAcknowledgment", "User Face Template Has been removed");
                    }
                    // index 11 for delete all fingers and index 12 to delete user
                    if (ZkMachine.SSR_DeleteEnrollData(ZkMachine.MachineNumber, Emp.ATEnrollmentNo_Default, 12))
                    {
                        await Clients.Caller.SendAsync("ClientAcknowledgment", "Sucessfully Remove User From Machine");
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("ClientAcknowledgment", "User not found in machine");
                    }
                }
                else
                {
                    await Clients.Caller.SendAsync("ClientAcknowledgment", "AT Not found from db :" + Emp.ATEnrollmentNo_Default);
                }


            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", ex.Message);
            }
            finally
            {
                Task.Delay(1000).Wait(token);
            }
        }
        private async Task SynchronizeFullTemplateAllAsync(int EmpID, zkemkeeper.CZKEM machine, int _MachineID, CancellationToken token)
        {
            try
            {
                var EmpIDs = _IEmployee.GetEmployeeIDListObject().Result;
                int RemainingIDs = EmpIDs.Count();

                await Clients.Caller.SendAsync("ClientAcknowledgment", EmpIDs.Count().ToString() + " Employees to be synchronized");

                foreach (var id in EmpIDs)
                {
                    await SynchronizeFullTemplateAsync(EmpID, machine, _MachineID, token);
                    RemainingIDs --;
                    Task.Delay(1).Wait(token);
                    await Clients.Caller.SendAsync("ClientAcknowledgment", RemainingIDs.ToString() + " Employees remaing to be synchronized");
                }
                Task.Delay(1).Wait(token);

            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", ex.Message);
            }
            finally
            {
                Task.Delay(1).Wait(token);
            }
        }
    }
    public class PayRunOperationHub : Hub
    {
        private readonly IPayRun _IPayRun;

        private static ConcurrentDictionary<string, int> Total = new ConcurrentDictionary<string, int>();
        private static ConcurrentDictionary<string, int> Processed = new ConcurrentDictionary<string, int>();

        public PayRunOperationHub(IPayRun _IPayRun)
        {
            this._IPayRun = _IPayRun;
            
        }

        public async Task StartOperation(int _MasterID = 0, string _ForProcess = "")
        {
            var IsInProcess = PayRunProcessStatus.PayRunProcessStatusList.Where(a => a.PayRunID == _MasterID).FirstOrDefault();

            if (IsInProcess != null)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", "PayRun Is busy in Process: " + IsInProcess.ForProcess + " By: " + IsInProcess.UserName);
                return;
            }

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            PayRunProcessStatus.PayRunProcessStatusList.Add(new PayRunProcessInfo()
            {
                PayRunID = _MasterID,
                ConnectionID = Context.ConnectionId,
                ForProcess = _ForProcess,
                UserName = Context.User.Identity.Name,
                cancellationTokenSource = cts
            });

            try
            {  
                
                await Clients.Caller.SendAsync("ClientAcknowledgment", "Starting Process " + _ForProcess + "....");
                Task.Delay(10).Wait(token);

                if (_ForProcess == "Employees Accumulating")
                    await EmployeesAccumulating(_IPayRun, _MasterID, _ForProcess,token);
                else if (_ForProcess == "Employees PayRun")
                    await EmployeesPayRun(_IPayRun, _MasterID, _ForProcess, token);
                else if (_ForProcess == "Employees PayRun Reversal")
                    await EmployeesPayRunReversal(_IPayRun, _MasterID, _ForProcess, token);
                else if (_ForProcess == "Emloyees PaySlip Mailing")
                    await EmloyeesPaySlipMailing(_IPayRun, _MasterID, _ForProcess, token);
                else if (_ForProcess == "test")
                    await RunConcurrentTasksWithProgress();
                else
                    Clients.Caller.SendAsync("ClientAcknowledgment", "Cannot Processed of Unknown Process: " + _ForProcess).Wait(token);

                Task.Delay(100).Wait(token);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", "Some thing went wrong while bulk processing on payrun");
                Task.Delay(1000).Wait(token);
            }
            finally
            {
                
            }
            
            if (cts.IsCancellationRequested)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", "Process: " + _ForProcess + " has been Aborted by User");
            }
            
            await ReleaseResource(Context.ConnectionId);
        }

        public async Task CancelOperation()
        {
            await ReleaseResource(Context.ConnectionId);
        }

        public async Task<object> GetOperatorDetail(int _PayRunID = 0)
        {
            object obj = null;
            await Task.Factory.StartNew(() =>
            {
                var payrunProcessStatus = PayRunProcessStatus.PayRunProcessStatusList?.Where(a => a.PayRunID == _PayRunID).FirstOrDefault();
                if (payrunProcessStatus != null)
                {
                    obj = new { ForProcess = payrunProcessStatus.ForProcess, UserName = payrunProcessStatus.UserName, MachineID = _PayRunID };
                    //return ();
                }
                else
                    obj = new { ForProcess = "Not In Process", UserName = "No User", MachineID = payrunProcessStatus };          
            }
            );

            return obj;

        }

        public async Task ReleaseResource(string _ConnectionId)
        {
            await Task.Factory.StartNew(() => {
                var payrunProcessStatus = PayRunProcessStatus.PayRunProcessStatusList.Where(w => w.ConnectionID == _ConnectionId).ToList();
                if (payrunProcessStatus != null)
                {
                    foreach (var item in payrunProcessStatus)
                    {
                        item.cancellationTokenSource.Cancel();
                        item.cancellationTokenSource.Dispose();
                        PayRunProcessStatus.PayRunProcessStatusList.Remove(item);
                    }
                }
            });
        }

        public override Task OnConnectedAsync()
        {
           
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Total.TryRemove(Context.ConnectionId, out _);
            Processed.TryRemove(Context.ConnectionId, out _);

            ReleaseResource(Context.ConnectionId).Wait();
            return base.OnDisconnectedAsync(exception);
        }
        private async Task EmloyeesPaySlipMailing(IPayRun _IPayRun, int _MasterID, string _ForProcess, CancellationToken token)
        {
            try
            {
                Clients.Caller.SendAsync("ClientAcknowledgment", "Please Wait while system gathering Email Information .. ").Wait(token);

                if (string.IsNullOrEmpty(Rpt_Shared.LicenseToEmail) || string.IsNullOrEmpty(Rpt_Shared.LicenseToEmailPswd) || string.IsNullOrEmpty(Rpt_Shared.LicenseToEmailHostName) || Rpt_Shared.LicenseToEmailPortNo <= 0)
                {
                    Clients.Caller.SendAsync("ClientAcknowledgment", "Unable to send mail: Email Not Configured").Wait(token);
                    return;
                }

                var PayRunDetail_List = await _IPayRun.GetPayRunDetail_Emp_EmailDetailList(_MasterID);

                Clients.Caller.SendAsync("ClientAcknowledgment", "Total Email to be sent: " + PayRunDetail_List.Count()).Wait(token);

                int TotalEmail = PayRunDetail_List.Count();
                int EmailProcessed = 0;

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(Rpt_Shared.LicenseToEmailHostName, Rpt_Shared.LicenseToEmailPortNo, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync(Rpt_Shared.LicenseToEmail, Rpt_Shared.LicenseToEmailPswd);

                    foreach (var PayRunDetail in PayRunDetail_List)
                    {
                        var message = new MimeMessage();

                        message.From.Add(new MailboxAddress(Rpt_Shared.LicenseTo, Rpt_Shared.LicenseToEmail));
                   
                        message.To.Add(new MailboxAddress(PayRunDetail.EmployeeName, PayRunDetail.Email));

                        message.Subject = "Pay Slip of " + PayRunDetail.MonthYear;

                        var builder = new BodyBuilder();

                        builder.HtmlBody = "<b>Dear " + PayRunDetail.EmployeeName + "</b><br>" + "Your salary has been generated in the system for the period of "
                            + PayRunDetail.MonthYear + "."
                            + "<br>" + "Please find attachment of Payslip" +
                            "<br>" + "Note: This is system generated email doesnot required signature." +
                            "<hr>" + Rpt_Shared.LicenseToEmailFooter.Replace("@whatsapp", "This is " + PayRunDetail.EmployeeName + " ");


                        builder.Attachments.Add("PaySlip", new MemoryStream(await _IPayRun.GetPDFFilePayRunDetailAsync("PayRun Salary Slip Individual", PayRunDetail.tbl_WPT_PayRunDetail_Emp_ID, 0, 0, DateTime.Now, DateTime.Now, "", "", "", "", 0)).ToArray(), new ContentType("application", "pdf"));


                        message.Body = builder.ToMessageBody();
                        await client.SendAsync(message);

                        EmailProcessed++;
                        Clients.Caller.SendAsync("ClientAcknowledgment", "[" + EmailProcessed.ToString() + "/" + TotalEmail.ToString() + "] Sending Email to.. " + PayRunDetail.Email).Wait(token);
                        Task.Delay(10).Wait(token);
                        if (Context.ConnectionAborted.IsCancellationRequested)
                        {
                            break;
                        }

                    }

                    await client.DisconnectAsync(true);

                    Clients.Caller.SendAsync("ClientAcknowledgment", "Email Sent Successfully... " + EmailProcessed.ToString() + "/" + TotalEmail.ToString()).Wait(token);

                }

            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", ex.Message);
            }
            finally
            {
                Task.Delay(1).Wait(token);
            }
        }
        private async Task EmployeesPayRunReversal(IPayRun _IPayRun, int _MasterID, string _ForProcess, CancellationToken token)
        {
            try
            {
                var PayRunDetailID_List = await _IPayRun.GetPayRunIDForBulk(_MasterID, _ForProcess);
                Clients.Caller.SendAsync("ClientAcknowledgment", PayRunDetailID_List.Count()).Wait(token);

                int TotalPayRun = PayRunDetailID_List.Count();
                int PayRunProcessed = 0;

                foreach (var PayRunDetailID in PayRunDetailID_List)
                {

                    _IPayRun.PostPayRunProcessMasterDetailEmployee(PayRunDetailID, false, Context.User.Identity.Name).Wait(token);

                    PayRunProcessed++;

                    Clients.Caller.SendAsync("ClientAcknowledgment", "Processing... " + PayRunProcessed.ToString() + " / " + TotalPayRun.ToString()).Wait(token);

                    Task.Delay(10).Wait(token);
                    if (Context.ConnectionAborted.IsCancellationRequested)
                    {
                        break;
                    }

                }



            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", ex.Message);
            }
            finally
            {
                Task.Delay(1000).Wait(token);
            }
        }
        private async Task EmployeesPayRun(IPayRun _IPayRun, int _MasterID,string _ForProcess, CancellationToken token)
        {
            try
            {
                var PayRunDetailID_List = _IPayRun.GetPayRunIDForBulk(_MasterID, _ForProcess).Result;
                Clients.Caller.SendAsync("ClientAcknowledgment", PayRunDetailID_List.Count()).Wait(token);

                int TotalPayRun = PayRunDetailID_List.Count();
                int PayRunProcessed = 0;

                foreach (var PayRunDetailID in PayRunDetailID_List)
                {

                    _IPayRun.PostPayRunProcessMasterDetailEmployee(PayRunDetailID, true, Context.User.Identity.Name).Wait(token);

                    PayRunProcessed++;

                    Clients.Caller.SendAsync("ClientAcknowledgment", "Processing... " + PayRunProcessed.ToString() + " / " + TotalPayRun.ToString()).Wait(token);

                    Task.Delay(10).Wait(token);
                    if (Context.ConnectionAborted.IsCancellationRequested)
                    {
                        break;
                    }

                }

                Clients.Caller.SendAsync("ClientAcknowledgment", "Sucessfully Processed: " + PayRunProcessed.ToString() + " / " + TotalPayRun.ToString()).Wait(token);


            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", ex.Message);
            }
            finally
            {
                Task.Delay(1000).Wait(token);
            }
        }
        private async Task EmployeesAccumulating(IPayRun _IPayRun, int _MasterID, string _ForProcess, CancellationToken token)
        {
            try
            {
                var emplist = _IPayRun.GetPayRunIDForBulk(_MasterID, _ForProcess).Result;
                Clients.Caller.SendAsync("ClientAcknowledgment", emplist.Count()).Wait(token);

                int TotalEmp = emplist.Count();
                int EmpProcessed = 0;

                foreach (var emp in emplist)
                {

                    _IPayRun.PostPayRunMasterDetailEmployee(
                                new tbl_WPT_PayRunDetail_Emp()
                                {
                                    ID = 0,
                                    FK_tbl_WPT_PayRunMaster_ID = _MasterID,
                                    FK_tbl_WPT_Employee_ID = emp
                                },
                                "Save New", Context.User.Identity.Name
                                ).Wait(token);

                    EmpProcessed++;

                    Clients.Caller.SendAsync("ClientAcknowledgment", "Processing... " + EmpProcessed.ToString() + " / " + TotalEmp.ToString()).Wait(token);

                    Task.Delay(10).Wait(token);

                    if (Context.ConnectionAborted.IsCancellationRequested)
                    {
                        break;
                    }
                }

                Clients.Caller.SendAsync("ClientAcknowledgment", "Sucessfully Processed: " + EmpProcessed.ToString() + " / " + TotalEmp.ToString()).Wait(token);


            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ClientAcknowledgment", ex.Message);
            }
            finally
            {
                Task.Delay(1000).Wait(token);
            }
        }

        private async Task RunConcurrentTasksWithProgress()
        {
            Total.TryAdd(Context.ConnectionId, 30);
            Processed.TryAdd(Context.ConnectionId, 0);

            DateTime start = DateTime.Now;

       

            await Clients.Caller.SendAsync("ReceiveProgressUpdate", Processed[Context.ConnectionId].ToString() + "/" + Total[Context.ConnectionId]);

            //List<Task> tasks = new List<Task>();

            //for (int j = 1; j <= 3; j++)
            //{
            //    Task t = LongRunningTaskAsync(j);

            //    tasks.Add(t);

            //}
            //await Task.WhenAll(tasks.ToList());

            await LongRunningTaskAsync2();

            //var tasks = new[]
            //{
            // LongRunningTaskAsync(1),
            //LongRunningTaskAsync(2),
            //LongRunningTaskAsync(3)
            //// Add more tasks as needed
            //};

            //await Task.WhenAll(tasks);

            await Clients.Caller.SendAsync("ReceiveProgressUpdate", DateTime.Now.Subtract(start).TotalMilliseconds.ToString());
            //await Clients.Caller.SendAsync("ReceiveProgressUpdate", Processed[Context.ConnectionId].ToString() + "/" + Total[Context.ConnectionId]);
        }
        private async Task LongRunningTaskAsync(int taskid)
        {
            for (int i = 1; i <= 10; i++)
            {
                Processed[Context.ConnectionId] = Processed[Context.ConnectionId] + 1;

                // Simulate a long-running operation.
                await Task.Delay(TimeSpan.FromSeconds(1));

                // Report progress to clients.
                await Clients.Caller.SendAsync("ReceiveProgressUpdate", "["+taskid.ToString() +"] " + Processed[Context.ConnectionId].ToString() + "/" + Total[Context.ConnectionId]);
            }      
        }

        private async Task LongRunningTaskAsync2()
        {
            for (int i = 1; i <= 30; i++)
            {
                Processed[Context.ConnectionId] = Processed[Context.ConnectionId] + 1;

                // Simulate a long-running operation.
                await Task.Delay(TimeSpan.FromSeconds(1));

                // Report progress to clients.
                await Clients.Caller.SendAsync("ReceiveProgressUpdate", Processed[Context.ConnectionId].ToString() + "/" + Total[Context.ConnectionId]);
            }
        }
    }
}
