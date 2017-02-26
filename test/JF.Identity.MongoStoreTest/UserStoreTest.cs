using JF.Identity.MongoStore;
using JF.Identity.Store;
using JF.Identity.Store.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace JF.Identity.MongoStoreTest
{
    public class UserStoreTest
    {
        [Fact]
        public async Task Connect_Test()
        {
            var store = new UserStore(await Drop(), new Mock<ILogger<UserStore>>().Object);
        }

        [Fact]
        public async Task Create_ShouldSuccess_Test()
        {
            var store = new UserStore(await Drop(), new Mock<ILogger<UserStore>>().Object);
            var user = new User { };

            await store.CreateAsync(user);
        }

        [Fact]
        public async Task CreateFailed_ShouldThrow_Test()
        {
            var store = new UserStore(await Drop(), new Mock<ILogger<UserStore>>().Object);
            var user = new User { };

            await store.CreateAsync(user);

            await Assert.ThrowsAsync<MongoWriteException>(() => store.CreateAsync(user));
        }

        [Fact]
        public async Task Create_ShouldRetrive_Test()
        {
            var store = new UserStore(await Drop(), new Mock<ILogger<UserStore>>().Object);
            var user = new User { };

            await store.CreateAsync(user);

            Assert.NotEqual(Guid.Empty, user.Id);
        }

        [Fact]
        public async Task FindById_NotFound_Test()
        {
            var store = new UserStore(await Drop(), new Mock<ILogger<UserStore>>().Object);

            var user = await store.FindByIdAsync(Guid.NewGuid());

            Assert.Null(user);
        }

        [Fact]
        public async Task FindById_Test()
        {
            var store = new UserStore(await Drop(), new Mock<ILogger<UserStore>>().Object);
            var user = new User { };

            await store.CreateAsync(user);
            var newUser = await store.FindByIdAsync(user.Id);

            Assert.Equal(user.Id, newUser.Id);
        }

        private async Task<IdentityDbContext> Drop()
        {
            var dbContext = TestDbContext.DbContext;
            await dbContext.Identity.DropCollectionAsync("Roles");
            await dbContext.Identity.DropCollectionAsync("Users");
            return dbContext;
        }
    }
}
