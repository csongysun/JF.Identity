using Orleans.Runtime.Configuration;
using System;
using System.Linq;
using System.Net;

namespace JF.Identity.Silo
{
    public class Program
    {
        private static OrleansHostWrapper hostWrapper;

        static int Main(string[] args)
        {
            int exitCode = InitializeOrleans();

            Console.WriteLine("Press Enter to terminate...");
            Console.ReadLine();

            exitCode += ShutdownSilo();

            return exitCode;
        }

        private static int InitializeOrleans()
        {
            //var config = new ClusterConfiguration();
            //config.Globals.DeploymentId = "JF.Identity";
            //config.Defaults.PropagateActivityId = true;
            //config.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.Disabled;
            //config.Globals.LivenessType = GlobalConfiguration.LivenessProviderType.MembershipTableGrain;
            //    config.Defaults.ProxyGatewayEndpoint = new IPEndPoint(IPAddress.Any, 10400);
            //config.Defaults.Port = 10300;
            //var ips = Dns.GetHostAddressesAsync(Dns.GetHostName()).Result;
            //config.Defaults.HostNameOrIPAddress = ips.FirstOrDefault()?.ToString();
            //hostWrapper = new OrleansHostWrapper(config);
            //return hostWrapper.Run();

            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.AddMemoryStorageProvider();
            // config.Defaults.DefaultTraceLevel = Orleans.Runtime.Severity.Verbose3;

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
