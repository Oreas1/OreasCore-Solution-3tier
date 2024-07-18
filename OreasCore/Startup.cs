using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace OreasCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration/*, IWebHostEnvironment env*/)
        {
            Configuration = configuration;
            //var contentRoot = env.ContentRootPath;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMyTasks,MyTasks>();
            services.Configure<HostOptions>(hostOptions =>
            {
                hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
            });
            services.AddHostedService<MyBackgroundTask>();



            services.AddTransient<CustomErrorHandlingMiddleware>();

            services.AddDbContextPool<OreasDbContext>(
                options =>
                options
                .UseLazyLoadingProxies()
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(new OreasDbCommandInterceptor()) // interceptor class is defined downward in the same file
                );

            services.AddIdentity<ApplicationUser,ApplicationRole>()
                .AddEntityFrameworkStores<OreasDbContext>();

            services.Configure<IdentityOptions>(
                opt =>
                {
                    opt.Password.RequiredLength = 6;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireLowercase = false;
                    opt.User.RequireUniqueEmail = false;
                    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
                }
            );

            //------setting dateformat--------
            // Default culture
            CultureInfo cultureInfo = new CultureInfo("en-US");
            // Set the date time format
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(cultureInfo);

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Index";
                options.Cookie.Name = "OreasCore.Identity.Token";
                options.ExpireTimeSpan = TimeSpan.FromHours(2);  // session will expire if user is inactive since 60minutes
                options.SlidingExpiration = true;       // apply session expiry
                //On Custom Authorization I have set this path
                //options.AccessDeniedPath = "/Home/AccessDenied";
            });

            services.AddCustomServices();

            services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

            ////adding NewtonsoftJson serivce and setting SerializerSettings to DefaultContractResolver for CaseInsensitive
            services.AddControllersWithViews().AddNewtonsoftJson(
                options => {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                    }         
                );


            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new CustomAuthorizationRequirement())
                .Build();
                
            });

            //services.AddMemoryCache();

            //services.AddAuthorization(c =>

            //c.AddPolicy("General", policy => policy.AddRequirements(new CustomAuthorizationRequirement()))

            //);

            services.AddTransient<IAuthorizationHandler, CustomAuthorizationHandler>();

            services.AddSignalR(o=> {
                o.MaximumParallelInvocationsPerClient = 5;
                o.EnableDetailedErrors = true;
                //o.ClientTimeoutInterval = TimeSpan.FromSeconds(1);
                o.KeepAliveInterval = TimeSpan.FromSeconds(5);               
                
                }).AddJsonProtocol(x=> x.PayloadSerializerOptions.PropertyNamingPolicy = null);

       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //------setting culture
            var cultureInfo = new CultureInfo("en-US"); // Change this according to your desired culture
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            //---------------------setting up report header footer detail i.e saved in database as startup----------------------//
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var myDbContext = serviceScope.ServiceProvider.GetService<OreasDbContext>();
                if (myDbContext.Database.CanConnect())
                {
                    var aspNetCompanyProfile = myDbContext.AspNetOreasCompanyProfile.OrderBy(i=> i.ID).FirstOrDefault();
                    if (aspNetCompanyProfile != null)
                        Rpt_Shared.Set(aspNetCompanyProfile);
                    var userSettings = myDbContext.AspNetOreasGeneralSettings.OrderBy(i=> i.ID).FirstOrDefault();
                    if (userSettings != null)
                    {
                        Rpt_Shared.LetterHead_PaperSize = userSettings.LetterHead_PaperSize;
                        Rpt_Shared.LetterHead_HeaderHeight = userSettings.LetterHead_HeaderHeight;
                        Rpt_Shared.LetterHead_FooterHeight = userSettings.LetterHead_FooterHeight;
                    }
                    foreach (var item in myDbContext.tbl_WPT_Machines.Where(w => w.ScheduledDownloadDailyAT.HasValue || w.ScheduledDownloadDailyAT2.HasValue).ToList())
                    {
                        if (item.ScheduledDownloadDailyAT.HasValue)
                            Jobs.Queue.Add(
                            new JobStructure(
                                item.ScheduledDownloadDailyAT.Value.Hours,
                                item.ScheduledDownloadDailyAT.Value.Minutes,
                                item.IP,
                                item.PortNo,
                                item.ID,
                                item.AutoClearLogAfterDownload
                                )
                            );
                        if (item.ScheduledDownloadDailyAT2.HasValue)
                            Jobs.Queue.Add(
                            new JobStructure(
                                item.ScheduledDownloadDailyAT2.Value.Hours,
                                item.ScheduledDownloadDailyAT2.Value.Minutes,
                                item.IP,
                                item.PortNo,
                                item.ID,
                                item.AutoClearLogAfterDownload
                                )
                            );
                    }

                    //-------------setting fiscal year---------//
                    var fy = myDbContext.tbl_Ac_FiscalYears.Where(f => f.IsClosed == false).OrderBy(d => d.PeriodStart);
                    if (fy != null)
                    {
                        FiscalYear.Set(fy.FirstOrDefault().PeriodStart, fy.LastOrDefault().PeriodEnd);
                    }
                    else
                    {
                        FiscalYear.Set(null, null);
                    }
                }

            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}
            app.UseStatusCodePagesWithReExecute("/Home/Error");


            
            app.UseOwin();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseMiddleware<CustomErrorHandlingMiddleware>();
            

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<MachineOperationtHub>("/machineOperationHub");
                endpoints.MapHub<PayRunOperationHub>("/payrunOperationHub");
            });



        }


        //-----------------this class is used for learning purpose i.e this call is used to intercept when asp core going to run queries of select/update/delete/ 
        public class OreasDbCommandInterceptor : DbCommandInterceptor
        {
            public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
            {
                return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            }

        }

    }
}
