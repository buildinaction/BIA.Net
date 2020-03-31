// <copyright file="WindowsService.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.WindowsService
{
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using BIA.Net.WindowsService;
    using Hangfire;
    using Hangfire.SqlServer;
    using Helpers;
    using System;
    using System.Threading.Tasks;
    using ZZCompanyNameZZ.ZZProjectNameZZ.WindowsService.Job;

    /// <summary>
    /// Service Windows (can work in console)
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public partial class WindowsService : ServiceAndConsole
    {

        static string projectName = "ZZProjectNameZZ";
        BackgroundJobServer myServer = null;

        private static BackgroundJobServer GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
                    "Hangfire",
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        UsePageLocksOnDequeue = true,
                        DisableGlobalLocks = true
                    }
                 );
            var options = new BackgroundJobServerOptions
            {
                Queues = new[] { "zzprojectnamezz" },
                ServerName = "ZZProjectNameZZ",
            };

            JobActivator.Current = new UnityJobActivator(BIAUnity.RootContainer);
            return new BackgroundJobServer(options);
        }

        /// <summary>
        /// Start and run the service
        /// </summary>
        /// <param name="args">arguments</param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            UnityConfig.RegisterTypes();

            Task.Run(() => this.Run());
        }

        /// <summary>
        /// Run the service
        /// </summary>
        /// <returns>Async Task</returns>
        protected async Task Run()
        {
            TraceManager.Info("ZZProjectNameZZService", "Run", "Service is running...");
            TraceManager.Configure();
            UnityConfig.RegisterTypes();
            myServer = GetHangfireServers();
            Console.WriteLine("ZZProjectNameZZ Server started. Press any key to exit...");

            //BackgroundJob.Enqueue<UpdateUsersFromAd>(t => t.Run());

            RecurringJob.RemoveIfExists($"{projectName}.UpdateUsersFromAdFunction");
            RecurringJob.RemoveIfExists($"{projectName}.WakeUp");
            //RecurringJob.AddOrUpdate<UpdateUsersFromAd>($"{projectName}.UpdateUsersFromAdFunction", t => t.Run(), Cron.Minutely(), null, "ZZProjectNameZZ");
            //RecurringJob.AddOrUpdate<WakeUpTask>($"{projectName}.WakeUp", t => t.Run(), Cron.Minutely(), null, "ZZProjectNameZZ");

            RecurringJob.AddOrUpdate<UpdateUsersFromAd>($"{projectName}.UpdateUsersFromAdFunction", t => t.Run(), Cron.Minutely(), null, "zzprojectnamezz");
            RecurringJob.AddOrUpdate<WakeUpTask>($"{projectName}.WakeUp", t => t.Run(), Cron.Minutely(), null, "zzprojectnamezz");

            while (true)
            {
                Task t = null;

                await Task.Delay(1000);
            }
        }

        /// <summary>
        /// Call when service stop
        /// </summary>
        protected override void OnStop()
        {
            myServer.Dispose();
        }

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">arguments</param>
        private static void Main(string[] args)
        {
            Main(args, new WindowsService());
        }
    }
}