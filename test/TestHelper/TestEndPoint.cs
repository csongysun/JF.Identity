using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace TestHelper
{
    public class TestEndPoint<TFixture, TStartup> : IClassFixture<TFixture> where TFixture: Fixture<TStartup>
    {
        public HttpClient _client;
        public TestServer _server;

        public TestEndPoint(TFixture fixture)
        {
            _client = fixture.Client;
            _server = fixture.Server;
        }
    }
}
