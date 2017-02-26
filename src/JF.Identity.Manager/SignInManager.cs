using JF.Identity.Common;
using JF.Identity.Common.Exceptions;
using JF.Identity.Common.Options;
using JF.Identity.Service;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace JF.Identity.Manager
{
    public class SignInManager : ISignInManager
    {
        private readonly HttpContext _context;
        private ILogger _logger;
        private readonly IUserManager _userManager;
        private readonly IUserClaimsPrincipalFactory _claimsFactory;
        private readonly ITokenProvider _tokenProvider;

        private readonly IdentityOptions _options;


        private CancellationToken _cancellationToken => _context?.RequestAborted ?? default(CancellationToken);
        public SignInManager(IUserManager userManager,
            IUserClaimsPrincipalFactory claimsFactory,
            ITokenProvider tokenProvider,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager> logger,
            IHttpContextAccessor contextAccessor)
        {
            _context = contextAccessor.HttpContext;

            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _tokenProvider = tokenProvider;
            _options = optionsAccessor?.Value ?? new IdentityOptions();
            _logger = logger;
        }

        private async Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user) => await _claimsFactory.CreateAsync(user);

        /// <summary>
        /// Signin by password
        /// </summary>
        /// <param name="key">the user's key to be signed in.</param>
        /// <param name="password">the password to be validated.</param>
        /// <returns>Login result and the logined user</returns>
        public async Task<(XError err, User user)> PasswordSignInAsync(string key, string password)
        {
            var user = await _userManager.FindByEmailAsync(key);
            if (user == null)
                return (ErrorTable.UserNotFound, null);
            return await PasswordSignInAsync(user, password);
        }

        /// <summary>
        /// Signin by password
        /// </summary>
        /// <param name="user">the user to be signed in.</param>
        /// <param name="password">the password to be validated.</param>
        /// <returns>Login result and the logined user</returns>
        private async Task<(XError err, User user)> PasswordSignInAsync(User user, string password)
        {
            var err = await CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            return err.IsOk ? await SignInAsync(user) : (err, null);
        }

        /// <summary>
        /// Login by refresh token.
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <returns>Login result and the logined user</returns>
        public async Task<(XError err, User user)> RefreshLoginAsync(string refreshToken)
        {
            var principal = _tokenProvider.Validate(refreshToken, _options.SignIn.RefreshToken);
            if (principal == null)
            {
                return (ErrorTable.RefreshTokenInvalid, null);
            }
            var user = await ValidateSecurityStampAsync(principal);
            if (user == null)
            {
                return (ErrorTable.SecurityStampInvalid, null);
            }
            await SetAccessTokenAsync(user);
            return (XError.Ok, user);
        }

        private async Task SetAccessTokenAsync(User user)
        {
            (string token, DateTimeOffset expireTime) = await GenerateAccessTokenAsync(user);
            user.Token = token;
            user.TokenEnd = expireTime;
            return;
        }

        /// <summary>
        /// Generate an access token for given user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>token string and it's expire time.</returns>
        private async Task<(string token, DateTimeOffset expireTime)> GenerateAccessTokenAsync(User user)
        {
            var principal = await CreateUserPrincipalAsync(user);
            return _tokenProvider.Generate(principal.Claims, _options.SignIn.AccessToken);
        }

        private async Task<(XError err, User user)> SignInAsync(User user)
        {
            await _userManager.SignInAsync(user);
            await SetAccessTokenAsync(user);
            return (XError.Ok, user);
        }

        public Task SignOutAsync()
        {
            return _userManager.SignOutAsync();
        }

        /// <summary>
        /// ValidateSecurityStampAsync
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        private async Task<User> ValidateSecurityStampAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }
            var user = await _userManager.GetUserByClaimsAsync(principal);
            if (user != null)
            {
                var securityStamp =
                    principal.FindFirst(_options.ClaimsIdentity.SecurityStampClaimType)?.Value;
                if (securityStamp == user.SecurityStamp)
                {
                    return user;
                }
            }
            return null;
        }

        private async Task<XError> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure)
        {
            await PreSignInCheckAsync(user);

            if (await _userManager.CheckPasswordAsync(user, password))
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                return XError.Ok;
            }

            _logger.LogWarning(2, "User {0} failed to provide the correct password.", user.Email);

            if (lockoutOnFailure)
            {
                await _userManager.AccessFailedAsync(user);
                if (await _userManager.IsLockedOutAsync(user))
                {
                    return LockOut(user);
                }
            }

            return ErrorTable.PasswordIncorrect;
        }

        private Task<bool> IsLockedOutAsync(User user)
        {
            return _userManager.IsLockedOutAsync(user);
        }

        private XError LockOut(User user)
        {
            _logger.LogWarning(3, "User {0} is currently locked out.", user.Email);
            return ErrorTable.UserLockedOut(user.LockoutEnd.Value);
        }

        private async Task PreSignInCheckAsync(User user)
        {
            // email confirm check

            // ban check

            if (await IsLockedOutAsync(user))
            {
                LockOut(user);
            }
        }

    }
}
