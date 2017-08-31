using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace TestHelper
{
    public abstract class Fixture<TStartup> : IDisposable
    {

        protected Action<IWebHostBuilder> ConfigBuilder;

        protected abstract string SolutionName { get; }
        protected abstract Type TargetStartupType { get; }

        protected virtual string SolutionRelativeTargetProjectParentDir { get; } = "src";


        public Fixture()
        {
            if (string.IsNullOrEmpty(SolutionName))
                throw new ArgumentNullException("SolutionName couldn't be null");
            var solutionRelativeTargetProjectParentDir = Path.Combine(SolutionRelativeTargetProjectParentDir);
            var startupAssembly = TargetStartupType.GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(solutionRelativeTargetProjectParentDir, startupAssembly);

            var builder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .ConfigureServices(InitializeServices)
                .UseEnvironment(Helper.EnvironmentName)
                .UseStartup(typeof(TStartup));

            ConfigBuilder?.Invoke(builder);

            Server = new TestServer(builder);
            Client = Server.CreateClient();
        }

        public TestServer Server { get; }
        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }

        protected virtual void InitializeServices(IServiceCollection services)
        {
        }

        private string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
        {
            var projectName = startupAssembly.GetName().Name;
            var applicationBasePath = AppContext.BaseDirectory;
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, SolutionName));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }
    }
}
