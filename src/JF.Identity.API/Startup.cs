using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using Orleans;
using Orleans.Runtime.Configuration;

namespace JF.Identity.API
{
    public class Startup
    {
        private static IClusterClient client;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InitializeOrleans().Wait();
            services.AddSingleton(client);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var al = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            //al.ApplicationStarted.Register(() =>
            //{
            //    client.Connect();
            //});
            al.ApplicationStopping.Register(() =>
            {
                client.Dispose();
            });
            app.UseMvc();
        }

        private async Task InitializeOrleans()
        {
            var config = new ClientConfiguration();
            config.DeploymentId = "JF.Identity";
            config.PropagateActivityId = true;
            var hostEntry = await Dns.GetHostEntryAsync("localhost");
            var ip = hostEntry.AddressList[0];
            config.Gateways.Add(new IPEndPoint(ip, 10400));

            Console.WriteLine("Initializing...");

            client = new ClientBuilder().UseConfiguration(config).Build();
            await client.Connect();
        }
    }
}
