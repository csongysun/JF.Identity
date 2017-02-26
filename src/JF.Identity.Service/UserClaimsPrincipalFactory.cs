using JF.Identity.Common.Options;
using JF.Identity.Manager;
using JF.Identity.Store.Model;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JF.Identity.Service
{
    public class UserClaimsPrincipalFactory :IUserClaimsPrincipalFactory
    {
        private IdentityOptions _options;
        private IUserManager _userManager;
        private IRoleManager _roleManager;
        public UserClaimsPrincipalFactory(
            IUserManager userManager,
            IRoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor)
        {
            if (optionsAccessor == null || optionsAccessor.Value == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _options = optionsAccessor.Value;
        }

        public async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var userId = user.Id;
            var email = user.Email;
            var id = new ClaimsIdentity(_options.ClaimsIdentity.AuthSchema,
                _options.ClaimsIdentity.EmailClaimType,
                _options.ClaimsIdentity.RoleClaimType);
            id.AddClaim(new Claim(_options.ClaimsIdentity.UserIdClaimType, userId.ToString()));
            id.AddClaim(new Claim(_options.ClaimsIdentity.EmailClaimType, email));
            id.AddClaim(new Claim(_options.ClaimsIdentity.SecurityStampClaimType, user.SecurityStamp));

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                id.AddClaim(new Claim(_options.ClaimsIdentity.RoleClaimType, role));

                // role claims 以后再说
                //id.AddClaims(await RoleManager.GetClaimsAsync(role));
            }

            id.AddClaims(await _userManager.GetClaimsAsync(user));
            return new ClaimsPrincipal(id);
        }
    }
}
