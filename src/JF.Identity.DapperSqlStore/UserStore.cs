using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSYS.Common;
using JF.Identity.Store;
using JF.Identity.Store.Model;
using Dapper;
using System.Linq;
using System.Data.SqlClient;
using Npgsql;

namespace JF.Identity.DapperSqlStore
{
    public class UserStore : IUserStore
    {
        private readonly IdentityDbContext _context;

        public UserStore(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<Error> CreateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var er = 0;
            var sql = @"INSERT INTO users (Id, Email, PasswordHash, Nickname, SecurityStamp) 
                VALUES (uuid_generate_v4(), @Email, @PasswordHash, @Nickname, uuid_generate_v4())";

            using (var db = _context.Connection)
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        er = await db.ExecuteAsync(sql,
                            new
                            {
                                Email = user.Email,
                                PasswordHash = user.PasswordHash,
                                Nickname = user.Nickname,
                            },
                            tran);
                        tran.Commit();
                        if (er == 0) return ErrorDescriber.DBInsertFailed("0 row effected");
                        return null;
                    }
                    catch(PostgresException e)
                    {
                        tran.Rollback();
                        return ErrorDescriber.DBInsertFailed(e.SqlState);
                    }
                }
            }
        }

        

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(User user, string roleName,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }



        public async Task<User> FindByEmailAsync(string email,  CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            var sql = @"SELECT * FROM users WHERE Email = @Email TOP 1";

            using (var db = _context.Connection)
            {
                db.Open();
                return await db.QueryFirstAsync<User>(sql, new { Email = email });
            }
        }

        public Task<User> FindByIdAsync(string id,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(User user,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(User user,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUsersForClaimAsync(Claim claim,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(User user, string roleName,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim,  CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<Error> UpdateAsync(User user,  CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var sql = @"UPDATE users SET 
                PasswordHash = @PasswordHash,
                AccessFailedCount = @AccessFailedCount,
                BanEnd = @BanEnd,
                LockoutEnd = @LockoutEnd,
                RefreshToken = @RefreshToken,
                RefreshTokenValid = @RefreshTokenValid,
                SecurityStamp = @SecurityStamp
                WHERE Id = @Id";

            using (var db = _context.Connection)
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        var x = await db.ExecuteAsync(sql, new
                        {
                            PasswordHash = user.PasswordHash,
                            AccessFailedCount = user.AccessFailedCount,
                            BanEnd = user.BanEnd,
                            LockoutEnd = user.LockoutEnd,
                            RefreshToken = user.RefreshToken,
                            RefreshTokenValid = user.RefreshTokenValid,
                            SecurityStamp = user.SecurityStamp,
                        },
                        tran);
                        tran.Commit();
                        if (x != 0) return ErrorDescriber.DBInsertFailed("Not 1 row effected");
                        return null;
                    }
                    catch (PostgresException e)
                    {
                        tran.Rollback();
                        return ErrorDescriber.DBInsertFailed(e.SqlState);
                    }
                }
            }
        }

        #region Custom
        public async Task<(Error, User)> CreateAndRetrieveAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var sql = @"INSERT INTO users (Id, Email, PasswordHash, Nickname, SecurityStamp) 
                VALUES (uuid_generate_v4(), @Email, @PasswordHash, @Nickname, uuid_generate_v4())
                returning *";

            using (var db = _context.Connection)
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        var x = await db.QueryAsync<User>(sql, new
                        {
                            Email = user.Email,
                            PasswordHash = user.PasswordHash,
                            Nickname = user.Nickname,
                        },
                        tran);
                        tran.Commit();
                        user = x.First();
                        return (null, user);
                    }
                    catch (PostgresException e)
                    {
                        tran.Rollback();
                        return (ErrorDescriber.DBInsertFailed(e.SqlState), null);
                    }
                }
            }
        }
        public async Task<Error> SignInAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var sql = @"UPDATE users SET 
                AccessFailedCount = 0,
                LockoutEnd = now(),
                RefreshToken = @RefreshToken,
                RefreshTokenValid = @RefreshTokenValid,
                SecurityStamp = @SecurityStamp
                WHERE Id = @Id";

            using (var db = _context.Connection)
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        var x = await db.ExecuteAsync(sql, new
                        {
                            RefreshToken = user.RefreshToken,
                            RefreshTokenValid = user.RefreshTokenValid,
                            SecurityStamp = user.SecurityStamp,
                            Id = user.Id
                        },
                        tran);
                        tran.Commit();
                        if (x != 1) return ErrorDescriber.DBUpdateFailed("Not 1 row effected");
                        return null;
                    }
                    catch (PostgresException e)
                    {
                        tran.Rollback();
                        return ErrorDescriber.DBUpdateFailed(e.SqlState);
                    }
                }
            }
        }

        #endregion


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~RoleStore() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        protected void ThrowIfDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion
    }
}
