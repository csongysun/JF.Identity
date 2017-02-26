using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Identity.Store.EFCore;
using Microsoft.EntityFrameworkCore;
using Identity.Store;
using CSYS.Identity.Store;
using Identity.Store.Model;
using Identity.Manager;
using CSYS.Identity;

namespace JF.Identity
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Store Configure
            var connection = Configuration.GetConnectionString("Identity");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
            services.AddScoped<IUserStore, UserStore>();
            services.AddScoped<IRoleStore, RoleStore>();
            #endregion

            #region Manager Configure
            services.AddScoped(typeof(UserManager), ser => ser.GetRequiredService<UserManager<User>>());
            services.AddScoped(typeof(RoleManager), ser => ser.GetRequiredService<RoleManager<Role>>());
            services.AddScoped(typeof(SignInManager), ser => ser.GetRequiredService<SignInManager<User>>());
            #endregion

            #region Identity Configure
            services.AddIdentity<User, Role>();
            services.AddScoped<UserManager<User>, UserManager>();
            services.AddScoped<RoleManager<Role>, RoleManager>();
            services.AddScoped<SignInManager<User>, SignInManager>();

            services.Configure<IdentityOptions>(Configuration.GetSection("Identity"));
            services.Configure<IdentityOptions>(identity =>
            {
                identity.SignIn.AccessToken.SecretKey = "test_access_token_secret_key";
                identity.SignIn.RefreshToken.SecretKey = "test_refresh_token_secret_key";
            });
            #endregion

            services.AddAutoMapper(typeof(Identity.Api.Model.MappingProfile));

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });


        }
    }
}
