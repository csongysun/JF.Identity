using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Serialization;

namespace JF.Identity.API
{
    public class Startup
    {
        private  IClusterClient client;

        public void ConfigureServices(IServiceCollection services)
        {
            InitSiloAsync().Wait();
            services.AddSingleton<IGrainFactory>(client);
            services.AddMvcCore()
                .AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            var al = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            InitSiloAsync().Wait();

            app.UseMvc();
        }

        private async Task InitSiloAsync()
        {
            Console.WriteLine("silo initing");

            var config = ClientConfiguration.LocalhostSilo();
            config.FallbackSerializationProvider = typeof(ILBasedSerializer).GetTypeInfo();
            client = new ClientBuilder().UseConfiguration(config).Build();
            await client.Connect();
            Console.WriteLine("connected");
        }

    }
}
