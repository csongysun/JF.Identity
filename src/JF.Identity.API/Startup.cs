using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitSiloAsync().Wait();
            var al = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            al.ApplicationStopping.Register(async () =>
            {
                await client.Close();
            });
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
