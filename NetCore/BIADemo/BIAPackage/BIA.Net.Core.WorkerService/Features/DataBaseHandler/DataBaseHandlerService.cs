using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    public class DataBaseHandlerService : IHostedService, IDisposable
    {
        private readonly DatabaseHandlerOptions _options;
        public DataBaseHandlerService(DatabaseHandlerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {

            foreach (var handlerRepositorie in _options.DatabaseHandlerRepositories)
            {
                handlerRepositorie.Start();
            }
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var handlerRepositorie in _options.DatabaseHandlerRepositories)
            {
                handlerRepositorie.Stop();
            }
        }

        public virtual void Dispose()
        {

        }
    }
}
