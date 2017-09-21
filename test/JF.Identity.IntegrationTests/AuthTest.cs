using System;
using System.Threading.Tasks;
using JF.Identity.Grain;
using JF.Identity.Grain.Commands;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Orleans.TestingHost;

namespace JF.Identity.IntegrationTests
{
    public class AuthTest: IClassFixture<ClusterFixture>
    {
        protected TestCluster Cluster;

        public AuthTest(ClusterFixture fixture) 
        {
            Cluster = fixture.Cluster;
        }

        //[Fact]
        //public async Task SignUp()
        //{
        //    var worker = Cluster.GrainFactory.GetGrain<ISignUpWorker>(0);
        //    var cmd = new SignUpCommand("test@test.cn", "testpwd", "testnickName");

        //    await worker.HandleAsync(cmd);

        //    var timeout = Task.Delay(500);
        //    var check = Task.Run(() =>
        //    {
        //        using (var context = Cluster.ServiceProvider.GetRequiredService<IdentityContext>())
        //        {
        //            var user = context.Users.First();
        //            Assert.NotEqual(cmd.Password, user.PasswordHash);
        //        }
        //    });

        //    await timeout;
        //    await check;
        //}
    }
}
