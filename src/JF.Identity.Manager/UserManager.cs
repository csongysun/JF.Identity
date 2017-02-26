using JF.Identity.Common;
using JF.Identity.Common.Exceptions;
using JF.Identity.Common.Options;
using JF.Identity.Service;
using JF.Identity.Store;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace JF.Identity.Manager
{
    public class UserManager: IUserManager
    {
        private readonly HttpContext _context;
        private readonly IUserStore _store;
        protected ILogger _logger { get; set; }
        private readonly IdentityOptions _options;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenProvider _tokenProvider;
        private CancellationToken _cancellationToken => _context?.RequestAborted ?? default(CancellationToken);
        public UserManager(IUserStore store,
            ITokenProvider tokenProvider,
            IPasswordHasher passwordHasher,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<UserManager> logger
            )
        {
            _logger = logger;
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _tokenProvider = tokenProvider;
            _passwordHasher = passwordHasher;
            _options = optionsAccessor?.Value ?? new IdentityOptions();
        }

        #region Base

        public Task<User> FindByIdAsync(Guid id)
        {
            return _store.FindByIdAsync(id, _cancellationToken);
        }

        public async Task<XError> CreateAsync(User user, string password)
        {
            UpdatePasswordHashInternal(user, password);
            user.Email = user.Email.ToLower();
            if (await _store.FindByEmailAsync(user.Email) != null)
            {
                return new XError(HttpStatusCode.Conflict, ErrorCode.EmailAlreadyExist);
            }
            await _store.CreateAsync(user, _cancellationToken);
            return XError.Ok;
        }

        public Task UpdateUserAsync(User user)
        {
            var err = ValidateUser(user);
            if (err != null)
                throw new JFIdentityException(LoggingEvents.USER_CREATE, err);
            return _store.UpdateAsync(user, _cancellationToken);
        }

        public Task<User> GetUserByClaimsAsync(ClaimsPrincipal principal)
        {
            var idstr = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
            var userId = Guid.Parse(idstr);
            //var user = await FindByIdAsync(userId);
            //return user;
            return FindByIdAsync(userId);
        }

        public Task<User> GetCurrenUserAsync() => GetUserByClaimsAsync(_context.User);

        #endregion

        #region Auth

        public async Task SignInAsync(User user)
        {
            await UpdateSecurityStampInternal(user);
            await UpdateRefreshTokenInternal(user);
            await _store.SignInAsync(user);
        }

        public async Task SignOutAsync()
        {
            var user = await GetCurrenUserAsync();
            await UpdateSecurityStampInternal(user);
            await UpdateUserAsync(user);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var result = await VerifyPasswordAsync(user, password);
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                await UpdatePasswordHashThenUpdateAsync(user, password);
            }

            var success = result != PasswordVerificationResult.Failed;
            if (!success)
            {
                _logger.LogWarning(0, $"Invalid password for user {user.Email}.");
            }
            return success;
        }

        public Task UpdateSecurityStampAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsLockedOutAsync(User user)
        {
            var lockoutTime = user.LockoutEnd;
            return Task.FromResult(lockoutTime >= DateTimeOffset.UtcNow);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public async Task AccessFailedAsync(User user)
        {
            var count = user.AccessFailedCount + 1;
            if (count < _options.Lockout.MaxFailedAccessAttempts)
            {
                user.AccessFailedCount = count;
                await UpdateUserAsync(user);
            }
            else
            {
                _logger.LogWarning(12, "User {userId} is locked out.", user.Id);
                user.LockoutEnd = DateTimeOffset.UtcNow.Add(_options.Lockout.DefaultLockoutTimeSpan);
                user.AccessFailedCount = 0;
                await UpdateUserAsync(user);
            }
        }

        #endregion

        #region UserEmail

        public Task<User> FindByEmailAsync(string email)
        {
            return _store.FindByEmailAsync(email.ToLower(), _cancellationToken);
        }

        public Task<bool> IsEmailConfirmedAsync(User user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        #endregion

        #region Roles
        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return await _store.GetRolesAsync(user, _cancellationToken);
        }

        public async Task AddToRoleAsync(User user, string role)
        {
            if (await _store.IsInRoleAsync(user, role, _cancellationToken))
            {
                _logger.LogWarning(5, "User {userId} is already in role {role}.", user.Id, role);
                throw new AppException(LoggingEvents.USER_ADD_TO_ROLE_FAILED, "User is already in role");
            }
            await _store.AddToRoleThenUpdateAsync(user, role, _cancellationToken);
        }

        #endregion

        #region UserClaim

        public Task<IList<Claim>> GetClaimsAsync(User user)
        {
            return _store.GetClaimsAsync(user, _cancellationToken);
        }

        #endregion

        #region Private

        protected Task UpdateSecurityStampInternal(User user)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            return Task.CompletedTask;
        }

        protected async Task UpdateRefreshTokenInternal(User user)
        {
            var (token, tokenEnd) = await GenerateRefreshTokenAsync(user);
            user.RefreshToken = token;
            user.RefreshTokenEnd = tokenEnd;    
            return;
        }

        protected Task<(string token, DateTimeOffset tokenEnd)> GenerateRefreshTokenAsync(User user)
        {
            var claims = new Claim[]
            {
                new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(_options.ClaimsIdentity.SecurityStampClaimType, user.SecurityStamp),
            };
            var result = _tokenProvider.Generate(claims, _options.SignIn.RefreshToken);
            return Task.FromResult(result);
        }

        protected string ValidateUser(User user)
        {
            //todo:添加注册校验
            return null;
        }

        protected async Task UpdatePasswordHashThenUpdateAsync(User user, string newPassword)
        {
            UpdatePasswordHashInternal(user, newPassword);
            await UpdateSecurityStampInternal(user);
            await _store.UpdateAsync(user, _cancellationToken);
        }

        protected void UpdatePasswordHashInternal(User user, string newPassword)
        {
            var hash = newPassword != null ? _passwordHasher.HashPassword(newPassword) : null;
            user.PasswordHash = hash;
        }

        protected Task<PasswordVerificationResult> VerifyPasswordAsync(User user, string password)
        {
            var hash = user.PasswordHash;
            if (hash == null)
            {
                return Task.FromResult(_passwordHasher.VerifyHashedPassword(hash, password));
            }
            return Task.FromResult(_passwordHasher.VerifyHashedPassword(hash, password));
        }

        public Task ClearTokenInternal(User user)
        {
            user.RefreshToken = null;
            user.RefreshTokenEnd = DateTimeOffset.Now;
            user.Token = null;
            user.RefreshTokenEnd = DateTimeOffset.Now;
            return Task.CompletedTask;
        }

        #endregion

    }
}
