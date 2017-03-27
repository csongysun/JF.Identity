using System;
using System.Threading.Tasks;
using CSYS.Identity;
using JF.Identity.Store;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;

namespace JF.Identity.Manager
{
    public class UserManager : UserManager<User>
    {
        private readonly HttpContext _context;
        public UserManager(IUserStore store,
            ITokenProvider tokenProvider,
            IPasswordHasher passwordHasher,
            IHttpContextAccessor contextAccessor,
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<UserManager> logger, 
            IdentityErrorDescriber errorDescriber) : base(store, tokenProvider, passwordHasher, optionsAccessor, logger, errorDescriber)
        {
            _context = contextAccessor.HttpContext;
            CancellationToken = _context?.RequestAborted ?? default(CancellationToken);
        }

        public override Task<User> GetCurrentUserAsync()
        {
            return GetUserByClaims(_context.User);
        }
    }
}
