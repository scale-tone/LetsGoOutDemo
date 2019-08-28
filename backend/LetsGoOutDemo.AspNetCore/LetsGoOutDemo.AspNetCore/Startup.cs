﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Management;
using StackExchange.Redis;

namespace LetsGoOutDemo.AspNetCore
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
            // Replacing SignalR's userId provider with our custom
            services.AddTransient<IUserIdProvider, NickNameUserIdProvider>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            string signalRConnString = this.Configuration.GetValue<string>(Constants.AzureSignalRConnectionStringEnvironmentVariableName);

            // Initializing SignalR with Azure SignalR Service connection string
            services
                .AddSignalR()
                .AddAzureSignalR(signalRConnString);

            // Also initializing and adding SignalR client
            var serviceManager = new ServiceManagerBuilder()
                .WithOptions(option =>
                {
                    option.ConnectionString = signalRConnString;
                    // ServiceTransportType.Persistent would be more efficient, but it is not reliable (connection might be dropped and never reestablished)
                    option.ServiceTransportType = ServiceTransportType.Transient;
                })
                .Build();

            // Adding the initialization Task to the container, not the IServiceHubContext itself.
            // That's OK, because the Task will only ran once (as guaranteed by the framework).
            services.AddSingleton(serviceManager.CreateHubContextAsync(nameof(LetsGoOutHub)));

            // Finally connecting to Redis
            string redisConnString = this.Configuration.GetValue<string>(Constants.RedisConnectionStringEnvironmentVariableName);
            services.AddSingleton(ConnectionMultiplexer.Connect(redisConnString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAzureSignalR(routes =>
            {
                // Hub name is derived from generic type parameter.
                // Path argument value determines the /negotiate endpoint's path, 
                // so in this case it will be /api/negotiate.
                routes.MapHub<LetsGoOutHub>("/api");
            });
        }
    }
}