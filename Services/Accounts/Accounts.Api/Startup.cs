using System;
using System.Reflection;
using Accounts.Infrastructure;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Accounts.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AccountsContext>(options =>
            {
                var accountEndpoint = Environment.GetEnvironmentVariable("ACCOUNT_ENDPOINT") ?? throw new InvalidOperationException("Missing ACCOUNT_ENDPOINT env. variable");
                var accountKey = Environment.GetEnvironmentVariable("ACCOUNT_KEY") ?? throw new InvalidOperationException("Missing ACCOUNT_KEY env. variable");
                var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? throw new InvalidOperationException("Missing DATABASE_NAME env. variable");
                
                options.UseCosmos(accountEndpoint, accountKey, databaseName);
            });
            
            services.AddMediatR(Assembly.GetExecutingAssembly())
                .AddAutoMapper(Assembly.GetExecutingAssembly());
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
