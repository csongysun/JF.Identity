using JF.Identity.Store.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JF.Identity.DapperSqlStore.Test
{
    public class UserStoreTest
    {

        [Fact]
        public async Task CreateNoUserThrowTest()
        {
            await TestMysqlDbContext.CleanTable("Users");
            var store = new UserStore(TestMysqlDbContext.Context);
            await Assert.ThrowsAsync<ArgumentNullException>(()=> store.CreateAsync(null));
        }

        [Fact]
        public async Task CreateUserTest()
        {
            await TestMysqlDbContext.CleanTable("Users");
            var store = new UserStore(TestMysqlDbContext.Context);
            var user = new User
            {
                PasswordHash = "MockPasswordHash",
                Nickname = "MockNickname",
                Email = "MockEmail"
            };
            var err = await store.CreateAsync(user);
            Assert.Null(err);
        }

        [Fact]
        public async Task CreateUserKeyConflictTest()
        {
            await TestMysqlDbContext.CleanTable("Users");
            var store = new UserStore(TestMysqlDbContext.Context);
            var user = new User
            {
                PasswordHash = "MockPasswordHash",
                Nickname = "MockNickname",
                Email = "MockEmail"
            };
            var err = await store.CreateAsync(user);
            Assert.Null(err);

            err = await store.CreateAsync(user);
            Assert.NotNull(err);
            Assert.Equal(err.Code, ErrorDescriber.DBInsertFailed("").Code);
        }

        [Fact]
        public async Task CreateUserAndRetrieveTest()
        {
            await TestMysqlDbContext.CleanTable("Users");
            var store = new UserStore(TestMysqlDbContext.Context);
            var user = new User
            {
                PasswordHash = "MockPasswordHash",
                Nickname = "MockNickname",
                Email = "MockEmail"
            };

            Assert.Equal(user.Id, default(Guid));
            Assert.Equal(user.SecurityStamp, default(Guid));
            var (err, newUser) = await store.CreateAndRetrieveAsync(user);

            Assert.Null(err);

            Assert.NotEqual(newUser.Id, default(Guid));
            Assert.NotEqual(newUser.SecurityStamp, default(Guid));
        }

        [Fact]
        public async Task SignInTest()
        {
            await TestMysqlDbContext.CleanTable("Users");
            var store = new UserStore(TestMysqlDbContext.Context);
            var user = new User
            {
                PasswordHash = "MockPasswordHash",
                Nickname = "MockNickname",
                Email = "MockEmail"
            };
            var (err, newUser) = await store.CreateAndRetrieveAsync(user);
            newUser.RefreshToken = Guid.NewGuid().ToString();
            newUser.RefreshTokenValid = true;
            newUser.SecurityStamp = Guid.NewGuid();
            err = await store.SignInAsync(newUser);
            Assert.Null(err);
            Assert.NotEqual(user.RefreshToken, newUser.RefreshToken);
            Assert.NotEqual(user.SecurityStamp, newUser.SecurityStamp);
        }

    }
}
