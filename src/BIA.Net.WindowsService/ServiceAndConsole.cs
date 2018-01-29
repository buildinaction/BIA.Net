

namespace BIA.Net.WindowsService
{
    using BIA.Net.Common;
    using System;
    using System.Diagnostics;
    using System.ServiceProcess;

    [System.ComponentModel.DesignerCategory("")]
    public class ServiceAndConsole : ServiceBase
    {

        static protected void Main(string[] args, ServiceAndConsole service)
        {

            if (Environment.UserInteractive)
            {
                service.OnStart(args);
                Console.WriteLine("Press enter to stop program");
                Console.Read();
                service.OnStop();
            }
            else
            {
                ServiceBase.Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            TraceManager.Configure();
            TraceManager.Info("OnStart", "Run", "Service is strating...");
        }
    }
}
