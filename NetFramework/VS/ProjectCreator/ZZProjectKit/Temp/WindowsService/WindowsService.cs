// <copyright file="WindowsService.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace $safeprojectname$
{
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using BIA.Net.WindowsService;
    using Business.Services;
    using Helpers;
    using System.Threading.Tasks;

    /// <summary>
    /// Service Windows (can work in console)
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    public partial class WindowsService : ServiceAndConsole
    {
        /// <summary>
        /// TODO : Example service to replace by your service
        /// </summary>
        private ServiceExampleForWindowsService serviceExample;

        /// <summary>
        /// Start and run the service
        /// </summary>
        /// <param name="args">arguments</param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            UnityConfig.RegisterTypes();
            this.serviceExample = BIAUnity.Resolve<ServiceExampleForWindowsService>();
            Task.Run(() => this.Run());
        }

        /// <summary>
        /// Run the service
        /// </summary>
        /// <returns>Async Task</returns>
        protected async System.Threading.Tasks.Task Run()
        {
            TraceManager.Info("$saferootprojectname$Service", "Run", "Service is running...");
            while (true)
            {
                int interval = this.serviceExample.Run();
                await Task.Delay(interval);
            }
        }

        /// <summary>
        /// Call when service stop
        /// </summary>
        protected override void OnStop()
        {
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