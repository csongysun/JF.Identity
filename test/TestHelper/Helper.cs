using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

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
    }
}
