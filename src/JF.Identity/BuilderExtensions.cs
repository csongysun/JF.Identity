using CSYS.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseIdentity(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var marker = app.ApplicationServices.GetService<IdentityMarkerService>();
            if (marker == null)
            {
                throw new InvalidOperationException("Identity not Add");
            }

            //var option = app.ApplicationServices.GetService<IOptions<IdentityOptions>>()?.Value;
            //var tokenOption = option.SignIn.AccessToken;
            //var jwtBearerOptions = new JwtBearerOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOption.SecretKey)),

            //        ValidateIssuer = true,
            //        ValidIssuer = tokenOption.Issuer,

            //        ValidateAudience = true,
            //        ValidAudience = tokenOption.Audience,

            //        ValidateLifetime = true,

            //        ClockSkew = TimeSpan.Zero
            //    }
            //};
            //app.UseJwtBearerAuthentication(jwtBearerOptions);
            return app;
        }
    }

}
