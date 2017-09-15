using System;
using System.Reflection;
using Orleans.Runtime.Configuration;
using Orleans.Serialization;

namespace JF.Identity.Silo
{
    public class Program
    {
        private static OrleansHostWrapper hostWrapper;

        static void Main(string[] args)
        {
            int exitCode = InitializeOrleansLocalhost();

            System.AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                ShutdownSilo();
            };

            Console.WriteLine("Press Enter to terminate...");
            while(true)
            Console.ReadLine();
        }

        private static int InitializeOrleansLocalhost()
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.Globals.FallbackSerializationProvider = typeof(ILBasedSerializer).GetTypeInfo();
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
