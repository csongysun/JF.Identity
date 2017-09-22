//using System;
//using System.Collections.Generic;
//using System.Text;
//using Orleans.TestingHost;
//using Orleans.Runtime.Configuration;

//namespace JF.Identity.IntegrationTests
//{
//    public class ClusterFixture: IDisposable
//    {
//        public TestCluster Cluster { get; private set; }

//        public ClusterFixture()
//        {
//            var config = new ClusterConfiguration();
//            config.UseStartupType<TestStartup>();
//            this.Cluster = new TestCluster();
//            this.Cluster.Deploy();
//        }

//        public void Dispose()
//        {
//            this.Cluster.StopAllSilos();
//        }

//    }
//}
