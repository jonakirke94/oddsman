﻿using System;
using System.Data;
using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Scraper.API.Controllers;
using Scraper.API.Filters;
using Scraper.API.Services;
using Scraper.Core.Data;

namespace Scraper.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<DanskeSpilContext>(o => o.UseMySql(Configuration.GetConnectionString("Default")));
            services.AddCors();
            services.Configure<ForwardedHeadersOptions>(o =>
            {
                o.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            var options = new MySqlStorageOptions
            {
                TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 50000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
            };

            var storage = new MySqlStorage(Configuration.GetConnectionString("Hangfire"), options);

            services.AddHangfire(o => o.UseStorage(storage));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var scope = app.ApplicationServices.GetService<IServiceScopeFactory>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders();

            using (var serviceScope = scope.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DanskeSpilContext>();
                context.Database.EnsureCreated();
                var databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
                try
                {
                    databaseCreator.CreateTables();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"{e.Message} - continuing");
                }
            }

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );

            app.UseStaticFiles();

            


            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthFilter() }
            });
            app.UseHangfireServer();

            // Add tasks on startup
            var automate = new AutomationService(scope);

            //BackgroundJob.Schedule(() => automate.ScrapeUpcomingMatches(), DateTimeOffset.Now.AddMinutes(1));


            app.UseMvc();
        }
    }
}
