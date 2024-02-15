using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Management;
using StackExchange.Redis;
using System.Net;
using Microsoft.Azure.Services.AppAuthentication;
using System.Threading.Tasks;
using System.Threading;

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

            services.AddMvc(options => { options.EnableEndpointRouting = false; });

            string signalRConnString = this.Configuration.GetValue<string>(Constants.AzureSignalRConnectionStringEnvironmentVariableName);
            signalRConnString = GetFromKeyVaultIfNeeded(signalRConnString);

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
                .BuildServiceManager();


            // Creating ServiceHubContext and pushing it to DI
            services.AddSingleton(serviceManager.CreateHubContextAsync(nameof(LetsGoOutHub), default).Result);

            // Finally connecting to Redis
            string redisConnString = this.Configuration.GetValue<string>(Constants.RedisConnectionStringEnvironmentVariableName);
            redisConnString = GetFromKeyVaultIfNeeded(redisConnString);
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

            app.UseRouting();

            app.UseEndpoints(routes => {

                // Hub name is derived from generic type parameter.
                // Path argument value determines the /negotiate endpoint's path, 
                // so in this case it will be /api/negotiate.
                routes.MapHub<LetsGoOutHub>("/api");

            });
        }

        /// <summary>
        /// A singleton Task, that produces access token for KeyVault.
        /// We only use this token at startup, so it's OK to run this code once and then reuse the
        /// resulting token. Taks object by itself ensures that its body will be run once.
        /// </summary>
        private static Task<string> KeyVaultAccessTokenTask = Task.Run(async delegate {
            // Wrapping this awaiting code with Task.Run(), to ensure that no deadlock happens due to SynchronizationContext.
            var tokenProvider = new AzureServiceTokenProvider();
            return await tokenProvider.GetAccessTokenAsync("https://vault.azure.net");
        });

        private const string KeyVaultUrlPart = ".vault.azure.net/secrets/";

        /// <summary>
        /// Container Instances and Linux App Services do not support @Microsoft.KeyVault() notation yet.
        /// This method resolves a secret in KeyVault (while relying on a Managed Identity being configured),
        /// if the config value looks like a Secret Identifier.
        /// </summary>
        private static string GetFromKeyVaultIfNeeded(string configValue)
        {
            if (!configValue.Contains(KeyVaultUrlPart))
            {
                return configValue;
            }

            // Taking the secret out of KeyVault
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {KeyVaultAccessTokenTask.Result}");
                string response = client.DownloadString($"{configValue}?api-version=2016-10-01");
                return response.FromJson().value;
            }
        }
    }
}
