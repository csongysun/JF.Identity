
using JF.Identity.Common;
using JF.Identity.Service;
using JF.Identity.Store.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JF.Identity.Manager
{
    public interface IUserManager
    {

        #region Base

        Task<User> FindByIdAsync(Guid id);

        Task<XError> CreateAsync(User user, string password);

        Task UpdateUserAsync(User user);

        Task<User> GetUserByClaimsAsync(ClaimsPrincipal principal);

        Task<User> GetCurrenUserAsync();

        #endregion

        #region Auth

        Task SignInAsync(User user);

        Task SignOutAsync();

        Task<bool> CheckPasswordAsync(User user, string password);

        Task UpdateSecurityStampAsync(User user);

        Task<bool> IsLockedOutAsync(User user);

        Task ResetAccessFailedCountAsync(User user);

        Task AccessFailedAsync(User user);

        #endregion

        #region UserEmail
        Task<User> FindByEmailAsync(string email);
        Task<bool> IsEmailConfirmedAsync(User user);
        #endregion

        #region Roles
        Task<IList<string>> GetRolesAsync(User user);
        Task AddToRoleAsync(User user, string role);
        #endregion

        #region UserClaim
        Task<IList<Claim>> GetClaimsAsync(User user);

        #endregion

    }
}
