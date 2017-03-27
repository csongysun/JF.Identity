using CSYS.Identity;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;

namespace JF.Identity.Manager
{
    public class SignInManager : SignInManager<User>
    {
        private readonly HttpContext _context;
        public SignInManager(UserManager userManager,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            ITokenProvider tokenProvider,
            IHttpContextAccessor contextAccessor,
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager> logger,
            IdentityErrorDescriber errors) : base(userManager, claimsFactory, tokenProvider, optionsAccessor, logger, errors)
        {
            _context = contextAccessor.HttpContext;
            CancellationToken = _context?.RequestAborted ?? default(CancellationToken);
        }
    }
}
