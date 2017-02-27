using CSYS.Identity;
using JF.Identity.Store;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JF.Identity.Manager
{
    public class UserManager : UserManager<User>
    {
        public UserManager(IUserStore store,
            ITokenProvider tokenProvider,
            IPasswordHasher passwordHasher,
            IHttpContextAccessor contextAccessor,
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<UserManager> logger, 
            IdentityErrorDescriber errorDescriber) : base(store, tokenProvider, passwordHasher, contextAccessor, optionsAccessor, logger, errorDescriber)
        {
        }
    }
}
