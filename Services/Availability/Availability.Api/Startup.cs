using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoMapper;
using Availability.Api.Application.Claims;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Availability.Infrastructure;
using Availability.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Availability.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AvailabilityContext>(options =>
            {
                var accountEndpoint = Configuration["Cosmos:Endpoint"];
                var accountKey = Configuration["Cosmos:Key"];
                var databaseName = Configuration["Cosmos:DatabaseName"];
                
                options.UseCosmos(accountEndpoint, accountKey, databaseName);
            });
            
            services.AddSingleton<IClaimConverter, UserClaimConverter>()
                .AddScoped<IAvailabilityRecordRepository, AvailabilityRecordRepository>()
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddAutoMapper(Assembly.GetExecutingAssembly());
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cerber.Availability.Api", 
                    Version = "v1",
                    Description = "Cerber availability api",
                    Contact = new OpenApiContact
                    {
                        Name = "Patryk Pasek",
                        Email = string.Empty,
                        Url = new Uri("http://github.com/sonquer"),
                    }
                });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            
            var key = Encoding.ASCII.GetBytes(Configuration["Token:Key"]);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
