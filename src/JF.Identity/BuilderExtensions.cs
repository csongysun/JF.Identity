using CSYS.Identity;
using CSYS.NamingService.ServiceRegister;
using CSYS.Service.Requester;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {
        public static IWebHostBuilder UseIdentity(this IWebHostBuilder builder)
        {

            return builder;
        }
    }

}
