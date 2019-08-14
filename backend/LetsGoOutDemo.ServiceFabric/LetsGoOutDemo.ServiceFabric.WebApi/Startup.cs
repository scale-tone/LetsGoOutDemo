using System;
using LetsGoOutDemo.ServiceFabric.Actors.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsGoOutDemo.ServiceFabric.WebApi
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

            services.AddCors();
            services.AddMvc();

            // Initializing SignalR with Azure SignalR Service connection string
            services
                .AddSignalR()
                .AddAzureSignalR(Environment.GetEnvironmentVariable(Constants.AzureSignalRConnectionStringEnvironmentVariableName));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                builder => builder
                    // .AllowAnyOrigin()  This doesn't work. The value of the 'Access-Control-Allow-Origin' header in the response must not be the wildcard '*' when the request's credentials mode is 'include'
                    .WithOrigins(Environment.GetEnvironmentVariable(Constants.AllowedCorsOriginsVariableName))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );

            app.UseMvc();

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
