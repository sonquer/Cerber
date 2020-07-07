using System.Reflection;
using System.Threading.Tasks;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Availability.Infrastructure;
using Availability.Infrastructure.Repositories;
using Availability.Worker.Application.Processors;
using Availability.Worker.Application.Services.Availability;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Availability.Worker
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostBuilderContext, configuration) =>
                {
                    configuration.AddJsonFile("appsettings.json", false)
                        .AddJsonFile($"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json", true)
                        .AddEnvironmentVariables("AVAILABILITY_WORKER_");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<AvailabilityContext>(options =>
                    {
                        var cerber = hostContext.Configuration.GetConnectionString("Cerber");
                        options.UseNpgsql(cerber);
                    });
                    
                    services.AddScoped<IAvailabilityProcessor, AvailabilityProcessor>()
                        .AddScoped<IAvailabilityService, AvailabilityService>()
                        .AddScoped<IAvailabilityRecordRepository, AvailabilityRecordRepository>()
                        .AddHttpClient()
                        .AddMediatR(Assembly.GetExecutingAssembly())
                        .AddHostedService<Worker>();
                });
    }
}