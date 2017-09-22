using System;
using System.Threading.Tasks;
using JF.Identity.Domain.AggregatesModel.UserAggregate;
using JF.Identity.Grain;
using JF.Identity.Grain.Commands;
using JF.Identity.Service;
using Moq;
using TestHelper;
using Xunit;

namespace JF.Identity.Grain.Tests
{
    public class AuthWorkerTests
    {
        [Fact]
        public async Task SignUpShouldFailedWhenEmailExist()
        {
            var context = MockDb.Sqlite;
            var userGrainMock = new Mock<IUserGrain>();
            var phMock = new Mock<IPasswordHasher>();
            phMock.Setup(_ => _.HashPassword(It.IsAny<string>())).Returns<string>(pwd => pwd);
            var authMock = new Mock<SignUpWorker>(context, phMock.Object);
            authMock.Setup(a => a.GrainFactory.GetGrain<IUserGrain>(It.IsAny<int>(), null))
                .Returns(userGrainMock.Object);

            var cmd = new SignUpCommand("test@test.cn", "testps", "testnkn");

            await context.AddAsync(new User { Email = cmd.Email });
            await context.SaveChangesAsync();

            var authGrain = authMock.Object;

            var ret = await authGrain.HandleAsync(cmd);

            Assert.False(ret.Succeed);

        }
    }
}
