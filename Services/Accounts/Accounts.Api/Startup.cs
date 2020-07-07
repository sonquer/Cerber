using System;
using System.Reflection;
using Accounts.Api.Application.Services;
using Accounts.Domain.AggregateModels.AccountAggregate;
using Accounts.Infrastructure;
using Accounts.Infrastructure.Repository;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
                var cerber = Configuration.GetConnectionString("Cerber");
                options.UseNpgsql(cerber, options =>
                {
                    options.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    options.MigrationsHistoryTable("migrations_history", AccountsContext.ACCOUNTS_SCHEMA);
                });
            });
            
            services.AddMediatR(Assembly.GetExecutingAssembly())
                .AddScoped<IAccountRepository, AccountRepository>()
                .AddScoped<IAuthorizationService, JwtAuthorizationService>()
                .AddAutoMapper(Assembly.GetExecutingAssembly());
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cerber.Accounts.Api", 
                    Version = "v1",
                    Description = "Cerber accounts api",
                    Contact = new OpenApiContact
                    {
                        Name = "Patryk Pasek",
                        Email = string.Empty,
                        Url = new Uri("http://github.com/sonquer"),
                    }
                });
            });

            services.AddHealthChecks();
            services.AddCors();
            services.AddControllers();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var basePath = Configuration["Docker:BasePath"];
            if (string.IsNullOrEmpty(basePath) == false)
            {
                app.Use((context, next) => {
                    context.Request.PathBase = new PathString(basePath);
                    return next();
                });
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger(c => {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c => {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("./v1/swagger.json", "Cerber Availability Api");
            });

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
