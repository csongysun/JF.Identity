using CSYS.Identity;
using JF.Identity.Manager;
using JF.Identity.Store;
using JF.Identity.Store.EFCore;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using JF.Identity.Api.Controllers;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;

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

            #region Model Map
            services.AddAutoMapper(typeof(Api.Model.MappingProfile)); 
            #endregion

            services.AddLogging();
            //services.AddCors();

            services.AddMvcCore()
                .AddApplicationPart(typeof(AuthController).GetTypeInfo().Assembly)
                .AddJsonFormatters()
                .AddAuthorization()
                .AddFormatterMappings()
                .AddDataAnnotations();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            var ex = error.Error;
                            await context.Response.WriteAsync(ex.ToString(), Encoding.UTF8);
                        }
                    });
                });
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.UseIdentity();
            app.UseMvc();
        }
    }
}
