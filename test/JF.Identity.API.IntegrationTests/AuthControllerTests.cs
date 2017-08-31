using System;
using System.Collections.Generic;
using System.Text;
using TestHelper;

namespace JF.Identity.API.IntegrationTests
{
    public class AuthControllerTests : TestEndPoint<TestFixture, TestStartup>
    {
        public AuthControllerTests(TestFixture fixture) : base(fixture)
        {
        }
    }
}
