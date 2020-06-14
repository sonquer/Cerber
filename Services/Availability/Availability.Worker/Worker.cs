using System;
using System.Threading;
using System.Threading.Tasks;
using Availability.Worker.Application.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Availability.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetService<IMediator>();

                await mediator.Publish(new RequestServicesCommand(), stoppingToken)
                    .ConfigureAwait(false);
                
                await Task.Delay(60_000, stoppingToken);
            }
        }
    }
}