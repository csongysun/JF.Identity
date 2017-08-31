using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TestHelper
{
    public class MockAuthHandler : AuthenticationHandler<MockAuthOptions>
    {
        public MockAuthHandler(
            IOptionsMonitor<MockAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string loginName = Request.Headers["LoginName"].ToString() ?? "";
            if (string.IsNullOrEmpty(loginName))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            string authHeader = Request.Headers["Authorization"].ToString() ?? "";
            string path = Request.Path.ToString() ?? "";

            var identity = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Authentication, authHeader),
                                                     new Claim(ClaimTypes.Uri, path),
                                                     new Claim(ClaimTypes.Name,loginName)
                                                    }, this.Scheme.DisplayName);

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity),
               new Microsoft.AspNetCore.Authentication.AuthenticationProperties(), this.Scheme.DisplayName);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    public class MockAuthOptions : AuthenticationSchemeOptions
    {
        public MockAuthOptions()
        {
        }
    }



    public static class MockAuthMiddlewareAppBuilderExtensions
    {
        public static AuthenticationBuilder AddMock(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<MockAuthOptions, MockAuthHandler>("MockAuth", "MockAuth", null);
        }
    }
}
