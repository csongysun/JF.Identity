using System;
using TestHelper;

namespace JF.Identity.API.IntegrationTests
{
    public class TestFixture: Fixture<TestStartup>
    {

        protected override string SolutionName => "JF.Identity.sln";

        protected override Type TargetStartupType => typeof(Startup);
    }
}