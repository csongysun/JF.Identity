using AutoMapper;
using CSYS.Identity;
using CSYS.Mvc.Formatters.Protobuf;
using CSYS.NamingService.ServiceRegister;
using CSYS.Service.Common;
using CSYS.Service.Requester;
using JF.Identity.Api.Controllers;
using JF.Identity.Manager;
using JF.Identity.Store;
using JF.Identity.Store.EFCore;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;

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

            #region Service Configure
            services.AddSingleton<IRegister, DefaultServiceRegister>();
            services.AddSingleton<IRequester, DefaultServiceRequester>();
            services.Configure<ServiceOption>(Configuration.GetSection("Service"));
            #endregion

            #region Store Configure
            var connection = Configuration.GetConnectionString("Identity");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
            services.AddScoped<IUserStore, UserStore>();
            services.AddScoped<IRoleStore, RoleStore>();
            #endregion

            #region Manager Configure
            //services.AddScoped(typeof(UserManager), ser => ser.GetRequiredService<UserManager<User>>());
            //services.AddScoped(typeof(RoleManager), ser => ser.GetRequiredService<RoleManager<Role>>());
            //services.AddScoped(typeof(SignInManager), ser => ser.GetRequiredService<SignInManager<User>>());
            services.AddScoped<UserManager>();
            services.AddScoped<RoleManager>();
            services.AddScoped<SignInManager>();
            #endregion

            #region Identity Configure
            services.AddIdentity<User, Role>();
            services.AddScoped<UserManager<User>>(ser => ser.GetRequiredService<UserManager>());
            services.AddScoped<RoleManager<Role>>(ser => ser.GetRequiredService<RoleManager>());
            services.AddScoped<SignInManager<User>>(ser => ser.GetRequiredService<SignInManager>());

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

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvcCore()
                .AddApplicationPart(typeof(AuthController).GetTypeInfo().Assembly)
                .AddProtobufFormatters()
//                .AddAuthorization()
                .AddFormatterMappings()
                .AddDataAnnotations();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime al)
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


            al.ApplicationStarted.Register(async () => await app.ApplicationServices.GetRequiredService<IRegister>().RegisterAsync());
            //re.RegisterAsync().Wait();

            app.UseMvc();
        }
    }
}
