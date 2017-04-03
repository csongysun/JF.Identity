using System;
using System.Threading.Tasks;
using CSYS.Identity;
using JF.Identity.Store;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using CSYS.Common;

namespace JF.Identity.Manager
{
    public class UserManager : UserManager<User>
    {
        private readonly HttpContext _context;
        private readonly IUserStore _store;
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

        public override async Task<Error> CreateAsync(User user, string password)
        {
            UpdatePasswordHashInternal(user, password);
            return await Store.CreateAsync(user, CancellationToken);
        }

        public override async Task<Error> SignInAsync(User user)
        {
            await UpdateSecurityStampInternal(user);
            await UpdateRefreshTokenInternal(user);
            return await _store.SignInAsync(user);
        }

        //protected override async Task<Error> UpdatePasswordHashAsync(User user, string newPassword)
        //{
        //    var hash = newPassword != null ? PasswordHasher.HashPassword(newPassword) : null;
        //    user.PasswordHash = hash;
        //    await UpdateSecurityStampInternal(user);
        //    return null;
        //}
    }
}
