using JF.Identity.Common;
using JF.Identity.Store;
using JF.Identity.Store.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace JF.Identity.MongoStore
{
    public class UserStore : IUserStore
    {
        private readonly ILogger _logger;

        private readonly IMongoDatabase _identity;
        private readonly IMongoCollection<User> _userSet;
        private readonly IMongoCollection<Role> _roleSet;

        public UserStore(IdentityDbContext context, ILogger<UserStore> logger)
        {
            this._identity = context.Identity;
            this._userSet = this._identity.GetCollection<User>("Users");
            this._roleSet = this._identity.GetCollection<Role>("Roles");
            this._logger = logger;
        }

        #region User Base CRUD
        public async Task CreateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _userSet.InsertOneAsync(user, null, cancellationToken);
            _logger.LogInformation(LoggingEvents.USER_DB, "Created user {0}: {1}", user.Email, user.Id);
        }

        public Task<User> FindByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _userSet.AsQueryable().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var filter = new BsonDocument("_id", user.Id);

            var result = await _userSet.ReplaceOneAsync(filter, user, null, cancellationToken);

            if (!result.IsAcknowledged)
            {
                _logger.LogDebug(LoggingEvents.USER_DB_FAILED, "Failed when update user: {0} \n info: {1}", user, result);
                _logger.LogError(LoggingEvents.USER_DB_FAILED, "Failed when update user: {0}", user.Email);
                throw new DbException(LoggingEvents.USER_DB_FAILED, "Update user failed");
            }
            else
            {
                _logger.LogInformation(LoggingEvents.USER_DB, "Updated user {0}: {1}", user.Email, user.Id);
            }
        }

        private async Task UpdateByUserIdAsync(Guid userId, UpdateDefinition<User> ud, CancellationToken cancellationToken = default(CancellationToken))
        {
            var filter = new BsonDocument("_id", userId);
            var result = await _userSet.UpdateOneAsync(u => u.Id == userId, ud, null, cancellationToken);

            if (!result.IsAcknowledged)
            {
                _logger.LogDebug(LoggingEvents.USER_DB_FAILED, "Failed when update user: {0} \n info: {1}", userId, result);
                _logger.LogError(LoggingEvents.USER_DB_FAILED, "Failed when update user: {0}", userId);
                throw new DbException(LoggingEvents.USER_DB_FAILED, "Update user failed");
            }
            else
            {
                _logger.LogInformation(LoggingEvents.USER_DB, "Updated user {0}", userId);
            }
        }

        #endregion

        #region Extended

        public async Task SignInAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.AccessFailedCount = 0;
            user.LockoutEnd = DateTimeOffset.Now;

            await UpdateAsync(user, cancellationToken);
        }

        public async Task<User> CreateAndRetrieveAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await CreateAsync(user, cancellationToken);
            }
            catch (DbException)
            {
                throw;
            }
            return await FindByEmailAsync(user.Email, cancellationToken);
        }

        #endregion

        #region UserEmail

        public Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _userSet.AsQueryable().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        #endregion

        #region UserRole
        public async Task AddToRoleThenUpdateAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var role = await _roleSet.AsQueryable().FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
            if (role == null)
                throw new DbException(LoggingEvents.USER_DB_FAILED, $"Role [{roleName}] not found");
            UpdateDefinition<User> ud = null;
            //if(user.Roles.Any(r=>r.Name == roleName))
            //{
            //    ud = Builders<User>.Update.Set<Role>(u => u.Roles.First(r => r.Name == roleName), role);
            //}
            //else
            //{
            //    ud  = Builders<User>.Update.AddToSet<Role>(u => u.Roles.Where(r => r.Name == roleName), role);
            //}

            ud = Builders<User>.Update.Set<Role>(u => u.Roles.First(r => r.Name == roleName), role);
            await UpdateByUserIdAsync(user.Id, ud, cancellationToken);
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult<IList<string>>(user.Roles.Select(r => r.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UserClaim

        public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult<IList<Claim>>(user.Claims.ToList());
        }

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateDefinition<User> ud = Builders<User>.Update.AddToSetEach<Claim>(u => u.Claims, claims);
            return UpdateByUserIdAsync(user.Id, ud, cancellationToken);
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateDefinition<User> ud = Builders<User>.Update.Set<Claim>(u => u.Claims.First(c => c.Type == claim.Type && c.Value == claim.Value), newClaim);
            return UpdateByUserIdAsync(user.Id, ud, cancellationToken);
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
