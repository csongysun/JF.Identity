using JF.Identity.Store.Model;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace JF.Identity.Store
{
    public interface IRoleStore
    {
        #region Role Base CRUD
        IQueryable Roles { get; }
        Task CreateAsync(Role role, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateAsync(Role role, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteAsync(Role role, CancellationToken cancellationToken = default(CancellationToken));
        Task<Role> FindByIdAsync(int roleId, CancellationToken cancellationToken = default(CancellationToken));
        Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken = default(CancellationToken));
        Task<IList<Role>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
        #endregion

        #region RoleClaim
        Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Add Claim to Role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="claim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default(CancellationToken));
        Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default(CancellationToken));
        #endregion
    }
}
