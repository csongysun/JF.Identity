using JF.Identity.Store.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace JF.Identity.Store
{
    public interface IUserStore
    {
        #region extend
        /// <summary>
        /// Create an User and return the result record.
        /// </summary>
        /// <param name="user">The user to be created.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>User</returns>
        Task<User> CreateAndRetrieveAsync(User user, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update an user for signing in.
        /// </summary>
        /// <param name="user">The signed in user.</param>
        /// <param name="cancellationToken">the cancellation token.</param>
        Task SignInAsync(User user, CancellationToken cancellationToken = default(CancellationToken));
        #endregion

        #region User CRUD
        /// <summary>
        /// Create an User record.
        /// </summary>
        /// <param name="user">The user to be created.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task CreateAsync(User user, CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Update an User.
        /// </summary>
        /// <param name="user">The **completed** user record.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Error message or null when succeed.</returns>
        Task UpdateAsync(User user, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Find an User by id.
        /// </summary>
        /// <param name="id">The given id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The User record or null when failed.</returns>
        Task<User> FindByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        #endregion

        #region UserEmail

        /// <summary>
        /// Find an user by email address.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The user was founded or null.</returns>
        Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken));

        #endregion

        #region UserRole
        Task AddToRoleThenUpdateAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken));
        Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken));
        Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken));
        Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default(CancellationToken));
        #endregion

        #region UserClaim
        Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default(CancellationToken));
        Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken));
        Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default(CancellationToken));
        Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken));
        Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default(CancellationToken));
        #endregion
    }
}
