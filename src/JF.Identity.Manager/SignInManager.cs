using CSYS.Identity;
using Identity.Store.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JF.Identity.Manager
{
    public class SignInManager : SignInManager<User>
    {
        public SignInManager(UserManager userManager,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            ITokenProvider tokenProvider,
            IHttpContextAccessor contextAccessor,
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager> logger,
            IdentityErrorDescriber errors) : base(userManager, claimsFactory, tokenProvider, contextAccessor, optionsAccessor, logger, errors)
        {
        }
    }
}
