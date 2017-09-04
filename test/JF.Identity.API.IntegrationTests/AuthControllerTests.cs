using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JF.Common;
using JF.Domain.Command;
using JF.Identity.Core.Application.Command;
using Moq;
using Newtonsoft.Json;
using TestHelper;
using Xunit;

namespace JF.Identity.API.IntegrationTests
{
    public class AuthControllerTests : TestEndPoint<TestFixture, TestStartup>
    {
        public AuthControllerTests(TestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task SignUpShouldAccept()
        {
            var cmd = new SignUpCommand("", "", "");
            var handlr = MockService.MockSignUpCommandHandler;
            handlr.SetReturnsDefault(Task.FromResult(new CommandResult()));

            var res = await _client.PostAsync("api/auth/signup" , Helper.JsonContent(cmd));

            Assert.Equal(HttpStatusCode.Accepted, res.StatusCode);
        }

        [Fact]
        public async Task SignUpShouldError()
        {
            var cmd = new SignUpCommand("", "", "");
            var handlr = MockService.MockSignUpCommandHandler;
            var err = new CommandResult("err");
            handlr.SetReturnsDefault(Task.FromResult(err));

            var res = await _client.PostAsync("api/auth/signup", Helper.JsonContent(cmd));

            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
            var e = await res.Content.ReadAsStringAsync();
            Assert.Equal(err.ErrorCode, e);
        }
    }
}
