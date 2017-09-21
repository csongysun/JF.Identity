using System;
using System.Collections.Generic;
using System.Text;
using JF.Identity.Grain;
using JF.Identity.Service;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JF.Identity.IntegrationTests
{
    public class TestStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services = AddInMemorySqlite(services);

            var sp = services.BuildServiceProvider();

            var context = sp.GetService<IdentityContext>();
            context.Database.EnsureCreatedAsync().Wait();
            return sp;
        }

        private IServiceCollection AddInMemorySqlite(IServiceCollection services)
        {
            var lf = new LoggerFactory();
            //lf.AddProvider(new SqlLoggerProvider());
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            services.AddEntityFrameworkSqlite()
                .AddDbContext<IdentityContext>(_ =>
                {
                    _.UseSqlite(connection)
                        .UseLoggerFactory(lf);
                });

            return services;
        }
    }
}
