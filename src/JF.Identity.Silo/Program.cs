using System;
using System.Linq;
using System.Net;
using System.Reflection;
using JF.Identity.Grain;
using JF.Identity.Grain.Abstractions;
using Orleans.Runtime.Configuration;

namespace JF.Identity.Silo
{
    public class Program
    {
        private static OrleansHostWrapper hostWrapper;

        static int Main(string[] args)
        {
            int exitCode = InitializeOrleansLocalhost();

            Console.WriteLine("Press Enter to terminate...");
            Console.ReadLine();

            exitCode += ShutdownSilo();

            return exitCode;
        }

        private static int InitializeOrleansLocalhost()
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.AddMemoryStorageProvider();
            config.UseStartupType<Startup>();
            hostWrapper = new OrleansHostWrapper(config);
            return hostWrapper.Run();
        }

        private static int ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                return hostWrapper.Stop();
            }
            return 0;
        }
    }
}
