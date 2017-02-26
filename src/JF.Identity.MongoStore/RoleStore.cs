
using JF.Identity.Common;
using JF.Identity.Common.Exceptions;
using JF.Identity.Store;
using JF.Identity.Store.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace JF.Identity.MongoStore
{
    public class RoleStore : IRoleStore
    {
        private readonly ILogger _logger;

        private readonly IMongoDatabase _identity;
        private readonly IMongoCollection<Role> _roleSet;
        public RoleStore(IdentityDbContext context, ILogger<RoleStore> logger)
        {
            this._identity = context.Identity;
            this._roleSet = this._identity.GetCollection<Role>("Roles");
            this._logger = logger;
        }

        #region Role Base CRUD

        public IQueryable Roles => _roleSet.AsQueryable();

        public async Task CreateAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await _roleSet.InsertOneAsync(role, null, cancellationToken);
            }
            catch (MongoWriteException e)
            {
                _logger.LogError(LoggingEvents.ROLE_DB_FAILED, "Failed when create role: {0}", role.Name);
                _logger.LogDebug(LoggingEvents.ROLE_DB_FAILED, e, "Failed when create role: {0}", role);
                throw new DbException(LoggingEvents.ROLE_DB_FAILED, "Create role failed");
            }
            _logger.LogInformation(LoggingEvents.ROLE_DB, "Created role {0}: {1}", role.Name, role.Id);
        }

        public async Task UpdateAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {

            var result = await _roleSet.ReplaceOneAsync(r=>r.Id == role.Id, role, null, cancellationToken);

            if (!result.IsAcknowledged)
            {
                _logger.LogDebug(LoggingEvents.ROLE_DB_FAILED, "Failed when update role: {0} \n info: {1}", role, result);
                _logger.LogError(LoggingEvents.ROLE_DB_FAILED, "Failed when update role: {0}", role.Name);
                throw new DbException(LoggingEvents.ROLE_DB_FAILED, "Update role failed");
            }
            else
            {
                _logger.LogInformation(LoggingEvents.ROLE_DB, "Updated user {0}: {1}", role.Name, role.Id);
            }
        }

        public Task DeleteAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Role> FindByIdAsync(int roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _roleSet.AsQueryable().FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
        }

        public Task<Role> FindByNameAsync(string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _roleSet.AsQueryable().FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
        }

        public async Task<IList<Role>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _roleSet.AsQueryable().ToListAsync(cancellationToken);
        }

        #endregion

        #region RoleClaim

        public Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult<IList<Claim>>(role.Claims.ToList());
        }

        public Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
