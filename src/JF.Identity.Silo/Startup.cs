using System;
using System.Collections.Generic;
using System.Text;
using JF.Identity.Grain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JF.Identity.Silo
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContextPool<IdentityContext>(options =>
                {
                    options.UseNpgsql("Host=dbserver;Database=identity;Username=postgres;Password=p@ssw0rd");
                });

            var sp = services.BuildServiceProvider();

            var context = sp.GetService<IdentityContext>();
            context.Database.EnsureCreatedAsync().Wait();

            return sp;
        }
    }
}
