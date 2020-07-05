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
using Microsoft.Extensions.Configuration.UserSecrets;
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
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        var env = hostContext.HostingEnvironment;
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        builder.AddUserSecrets(appAssembly, true);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<AvailabilityContext>(options =>
                    {
                        var accountEndpoint = hostContext.Configuration["Cosmos:Endpoint"];
                        var accountKey = hostContext.Configuration["Cosmos:Key"];
                        var databaseName = hostContext.Configuration["Cosmos:DatabaseName"];
                
                        options.UseCosmos(accountEndpoint, accountKey, databaseName);
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