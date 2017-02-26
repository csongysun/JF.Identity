using JF.Identity.Api.Controllers;
using JF.Identity.Common.Options;
using JF.Identity.Manager;
using JF.Identity.MongoStore;
using JF.Identity.Service;
using JF.Identity.Store;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Reflection;
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

            #region Ms
            //services.AddSingleton<IRegister, DefaultServiceRegister>();
            //services.AddSingleton<IRequester, DefaultServiceRequester>();
            //services.Configure<ServiceOption>(Configuration.GetSection("Service"));
            #endregion

            #region Service Configure
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenProvider, JwtTokenProvider>();
            services.AddScoped<IUserClaimsPrincipalFactory, UserClaimsPrincipalFactory>();
            #endregion

            #region Store Configure
            var connection = Configuration.GetConnectionString("Identity");
            services.AddSingleton(s => new IdentityDbContext(connection));
            services.AddScoped<IUserStore, UserStore>();
            services.AddScoped<IRoleStore, RoleStore>();
            #endregion

            #region Manager Configure
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<ISignInManager, SignInManager>();
            #endregion

            #region Option Configure
            services.Configure<IdentityOptions>(Configuration.GetSection("Identity"));
            services.Configure<IdentityOptions>(identity =>
            {
                identity.SignIn.AccessToken.SecretKey = "test_access_token_secret_key";
                identity.SignIn.RefreshToken.SecretKey = "test_refresh_token_secret_key";
            });
            #endregion


            services.AddLogging();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvcCore(options =>
            {
                options.ModelValidatorProviders.Clear();
            })
                .AddApplicationPart(typeof(AuthController).GetTypeInfo().Assembly)
                .AddJsonFormatters()
                .AddFormatterMappings();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime al)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("未知错误", Encoding.UTF8);
                    });
                });
            }
            app.UseMvc();
        }
    }
}
