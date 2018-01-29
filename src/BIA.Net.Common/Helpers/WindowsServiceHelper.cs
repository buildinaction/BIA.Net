namespace BIA.Net.Common.Helpers
{
    using System;
    using System.ServiceProcess;

    /// <summary>
    /// WindowsService Helper
    /// </summary>
    public static class WindowsServiceHelper
    {
        /// <summary>
        /// Start the service
        /// </summary>
        /// <param name="serviceName">service name</param>
        /// <param name="timeoutMilliseconds">timeout Milliseconds</param>
        public static void Start(string serviceName, int? timeoutMilliseconds = null)
        {
            using (ServiceController service = new ServiceController(serviceName))
            {
                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start();

                    if (timeoutMilliseconds.HasValue)
                    {
                        service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(timeoutMilliseconds.Value));
                    }
                    else
                    {
                        service.WaitForStatus(ServiceControllerStatus.Running);
                    }
                }
            }
        }

        /// <summary>
        /// Stop the service
        /// </summary>
        /// <param name="serviceName">service name</param>
        /// <param name="timeoutMilliseconds">timeout Milliseconds</param>
        public static void Stop(string serviceName, int? timeoutMilliseconds = null)
        {
            using (ServiceController service = new ServiceController(serviceName))
            {
                if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
                {
                    service.Stop();

                    if (timeoutMilliseconds.HasValue)
                    {
                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(timeoutMilliseconds.Value));
                    }
                    else
                    {
                        service.WaitForStatus(ServiceControllerStatus.Stopped);
                    }
                }
            }
        }

        /// <summary>
        /// Restart the service
        /// </summary>
        /// <param name="serviceName">service name</param>
        /// <param name="timeoutMilliseconds">timeout Milliseconds</param>
        public static void Restart(string serviceName, int? timeoutMilliseconds = null)
        {
            DateTime startDate = DateTime.Now;

            Stop(serviceName, timeoutMilliseconds);

            if (timeoutMilliseconds.HasValue)
            {
                timeoutMilliseconds = timeoutMilliseconds.Value - (DateTime.Now - startDate).Milliseconds;
            }

            Start(serviceName, timeoutMilliseconds);
        }

        /// <summary>
        /// returns service status.
        /// </summary>
        /// <param name="serviceName">service name</param>
        /// <returns>service status</returns>
        public static string GetStatus(string serviceName)
        {
            string status = null;

            using (ServiceController service = new ServiceController(serviceName))
            {
                status = service.Status.ToString();
            }

            return status;
        }
    }
}