using System.Threading.Tasks;
using JF.Domain.Command;
using JF.Identity.Core.Application.Command;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace JF.Identity.API
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var al = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

            app.UseMvc();
        }

    }
}
