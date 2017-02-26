using JF.Identity.Store.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JF.Identity.Manager
{
    public interface IRoleManager
    {

        #region base

        Task CreateAsync(Role role);

        Task DeleteAsync(Role role);
        Task<bool> RoleExistsAsync(string roleName);

        Task<Role> FindByIdAsync(int roleId);

        Task<Role> FindByNameAsync(string roleName);

        Task UpdateRoleAsync(Role role);
      
        Task<IList<Role>> GetAll();
        Task DeleteByIdAsync(int id);

        #endregion

        #region RoleClaim
        Task AddClaimAsync(Role role, Claim claim);
        Task RemoveClaimAsync(Role role, Claim claim);
        Task<IList<Claim>> GetClaimsAsync(Role role);
        #endregion


    }
}
