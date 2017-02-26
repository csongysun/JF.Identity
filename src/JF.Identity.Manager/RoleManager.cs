
using JF.Identity.Common;
using JF.Identity.Common.Exceptions;
using JF.Identity.Store;
using JF.Identity.Store.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace JF.Identity.Manager
{
    public class RoleManager : IRoleManager
    {
        private readonly HttpContext _context;
        private readonly IRoleStore _store;
        protected ILogger _logger { get; set; }

        private CancellationToken _cancellationToken => _context?.RequestAborted ?? default(CancellationToken);

        public RoleManager(IRoleStore store,
            ILogger<RoleManager> logger,
            IHttpContextAccessor contextAccessor)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _logger = logger;
            _context = contextAccessor.HttpContext;
        }


        #region base

        public Task CreateAsync(Role role)
        {
            try
            {
                return _store.CreateAsync(role, _cancellationToken);
            }
            catch (DbException e)
            {
                throw new AppException(LoggingEvents.ROLE_CREATE_FAILED, "Create role failed", e);
            }
        }

        public Task DeleteAsync(Role role)
        {
            return _store.DeleteAsync(role, _cancellationToken);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return (await FindByNameAsync(roleName)) != null;
        }

        public Task<Role> FindByIdAsync(int roleId)
        {
            return _store.FindByIdAsync(roleId, _cancellationToken);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return _store.FindByNameAsync(roleName, _cancellationToken);
        }

        public Task UpdateRoleAsync(Role role)
        {
            try
            {
                return _store.UpdateAsync(role, _cancellationToken);
            }
            catch (DbException e)
            {
                throw new AppException(LoggingEvents.ROLE_UPDATE_FAILED, "Update role failed", e);
            }
        }

        public Task<IList<Role>> GetAll()
        {
            return _store.GetAllAsync(_cancellationToken);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var role = await FindByIdAsync(id);
            if (role == null)
                throw new AppException(LoggingEvents.ROLE_NOT_FOUND, $"Role {id} not found");
            await DeleteAsync(role);
        }

        #endregion

        #region RoleClaim

        public Task AddClaimAsync(Role role, Claim claim)
        {
            return _store.AddClaimAsync(role, claim, _cancellationToken);
        }

        public Task RemoveClaimAsync(Role role, Claim claim)
        {
            return _store.RemoveClaimAsync(role, claim, _cancellationToken);
        }

        public Task<IList<Claim>> GetClaimsAsync(Role role)
        {
            return _store.GetClaimsAsync(role, _cancellationToken);
        }


        #endregion

    }
}
