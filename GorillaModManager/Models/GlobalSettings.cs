using System.IO;

namespace GorillaModManager.Models
{
    public static class GlobalSettings
    {
        public static string GorillaPath { get; set; } = string.Empty;

        public static string GetPluginsPath()
        {
            return Path.Combine(GorillaPath, "BepInEx", "Plugins");
        }
    }
}