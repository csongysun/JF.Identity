using System;
using System.Threading.Tasks;
using JF.Identity.Grain;
using Moq;
using TestHelper;
using Xunit;

namespace JF.Identity.Grain.Tests
{
    public class AuthGrainTests
    {
        [Fact]
        public async Task SignUpShouldFailedWhenEmailExist()
        {
            var context = MockDb.Sqlite;
            var userGrainMock = new Mock<IUserGrain>();
            var authMock = new Mock<AuthWorker>(context);
            authMock.Setup(a => a.GrainFactory.GetGrain<IUserGrain>(It.IsAny<int>(), null))
                .Returns(userGrainMock.Object);

            var cmd = new SignUpCommand("test@test.cn", "testps", "testnkn");

            await context.AddAsync(new User { Email = cmd.Email });
            await context.SaveChangesAsync();

            var authGrain = authMock.Object;

            var ret = await authGrain.SignUpAsync(cmd);

            Assert.False(ret.Succeed);

        }
    }
}
