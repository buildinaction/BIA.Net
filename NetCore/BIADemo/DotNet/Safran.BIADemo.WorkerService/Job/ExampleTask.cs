// BIADemo only
// <copyright file="ExampleTask.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>
namespace Safran.BIADemo.WorkerService.Job
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Job;
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    [AutomaticRetry(Attempts = 0, LogEvents = true)]
    internal class ExampleTask : BaseJob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeUserTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="userService">The user app service.</param>
        /// <param name="logger">logger.</param>
        public ExampleTask(IConfiguration configuration, ILogger<SynchronizeUserTask> logger)
            : base(configuration, logger)
        {
        }

        /// <summary>
        /// Call the synchronization service.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
        protected override async Task RunMonitoredTask()
        {
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": Hello from the job ExampleTask.");
        }
    }
}
