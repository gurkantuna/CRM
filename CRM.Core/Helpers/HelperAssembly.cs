using System.Reflection;
using System.Runtime.Versioning;
using CRM.Core.Extensions;

namespace CRM.Core.Helpers {
    public static class HelperAssembly {
        public static Assembly AssemblyFullName(Type type) {
            return Assembly.GetAssembly(type);
        }

        //I : https://stackoverflow.com/questions/19096841/how-to-get-the-version-of-the-net-framework-being-targeted
        public static TargetFrameworkType GetEntryTargetFramework() {
            var targetFramework = Assembly.GetEntryAssembly().GetCustomAttribute<TargetFrameworkAttribute>()?
                                  .FrameworkName
                                  .ToLower();

            return GetFrameworkType(targetFramework);
        }

        public static Framework GetEntryTargetFrameworkAndVersion() {

            var targetFrameworkAndVersion = Assembly.GetEntryAssembly().GetCustomAttribute<TargetFrameworkAttribute>()?
                                                                    .FrameworkName
                                                                    .ToLower();
            return GetFramework(targetFrameworkAndVersion);
        }

        public static TargetFrameworkType GetExecutingTargetFramework() {

            var targetFramework = Assembly.GetExecutingAssembly().GetCustomAttribute<TargetFrameworkAttribute>()?
                                  .FrameworkName
                                  .ToLower();

            return GetFrameworkType(targetFramework);
        }

        public static Framework GetExecutingTargetFrameworkAndVersion() {
            var targetFrameworkAndVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<TargetFrameworkAttribute>()?
                                                                    .FrameworkName
                                                                    .ToLower();
            return GetFramework(targetFrameworkAndVersion);
        }

        private static TargetFrameworkType GetFrameworkType(string targetFramework) {
            TargetFrameworkType targetFrameworkType = TargetFrameworkType.Unknown;

            if(targetFramework.IsNotNullOrWhitespace()) {
                if(targetFramework.Contains("netframework")) {
                    targetFrameworkType = TargetFrameworkType.NetFramework;
                }

                if(targetFramework.Contains("netcoreapp")) {
                    targetFrameworkType = TargetFrameworkType.NetCoreApp;
                }

                if(targetFramework.Contains("netstandard")) {
                    targetFrameworkType = TargetFrameworkType.NetStandard;
                }
            }
            return targetFrameworkType;
        }

        private static Framework GetFramework(string targetFrameworkAndVersion) {
            var infos = targetFrameworkAndVersion.Split(',');

            var name = infos[0];
            var version = infos[1].Replace("version=v", string.Empty);

            return new Framework { Name = name, Version = version };
        }

    }

    public enum TargetFrameworkType {
        Unknown,
        NetFramework,
        NetCoreApp,
        NetStandard
    }

    public class Framework {
        public string Name { get; set; }
        public string Version { get; set; }
    }

}