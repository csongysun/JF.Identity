using JF.Domain.Command;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestHelper;

namespace JF.Identity.API.IntegrationTests
{
    public class TestStartup
    {
        public TestStartup(IHostingEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICommandBus, CommandBus>();

            services.AddScoped(_ => MockService.MockSignUpCommandHandler.Object);

            services.AddMvcCore()
                .AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
        }
    }
}