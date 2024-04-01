using System;
using System.Threading;
using System.Threading.Tasks;
using Cl.Gob.Energia.Ecocarga.Api;
using Cl.Gob.Energia.Ecocarga.Api.Services.Interfaces;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cl.Gob.Energia.Ecocarga.Api.Services
{
    internal class CopecHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        private Timer _timer;
        private readonly IOptions<AppSettings> _settings;

        public CopecHostedService(ILogger<CopecHostedService> logger, IServiceProvider services, IOptions<AppSettings> settings)
        {
            _logger = logger;
            _services = services;
            _settings = settings;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Servicio actualización datos Copec ha comenzado.");

            _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, 
                TimeSpan.FromMinutes(_settings.Value.MinutosEsperaElectrolinera));

            return Task.CompletedTask;
        }

        public Task doWorkTask;

        private void ExecuteTask(object state)
        {
            doWorkTask = DoWork();
        }

        private async Task DoWork()
        {
            using (var scope = _services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<ICopecService>();
                scope.ServiceProvider
                    .GetRequiredService<EcocargaContext>();

                await scopedProcessingService.UpdateCopec();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Servicio actualización datos Copec se detuvo.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
