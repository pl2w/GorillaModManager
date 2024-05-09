using System.IO;

namespace GorillaModManager.Models
{
    public static class GlobalSettings
    {
        public static string GorillaPath { get; set; } = string.Empty;

        private static string _version = "v0.0.1";

#if DEBUG
        public static string Version { get; } = $"{_version} - DEBUG";
#else
        public static string Version { get; } = $"{_version} - RELEASE";
#endif

        public static string GetPluginsPath()
        {
            return Path.Combine(GorillaPath, "BepInEx", "Plugins");
        }

        public static bool DoesPluginsExist()
        {
            return Path.Exists(GetPluginsPath());
        }
    }
}