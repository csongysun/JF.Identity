using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace TestHelper
{
    public static class Helper
    {
        public static string EnvironmentName
        {
            get
            {
                var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (string.IsNullOrEmpty(env)) env = "Production";
                return env;
            }
        }

        public static string GetContentRootPath<TStartup>()
        {
                var projDirName = typeof(TStartup).GetTypeInfo().Assembly.GetName().Name;
                var path = Directory.GetParent(Directory.GetCurrentDirectory())
                    .Parent.Parent.Parent.Parent.FullName;

                path = Path.Combine(path, "src", projDirName);
                return path;
        }

        public static StringContent JsonContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}
