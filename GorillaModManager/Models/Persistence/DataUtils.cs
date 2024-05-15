using System;
using System.IO;

namespace GorillaModManager.Models.Persistence
{
    public static class DataUtils
    {
        public static bool DoesPluginsExist()
        {
            return Directory.Exists(Plugins());
        }

        public static string Plugins()
        {
            return Path.Combine(ManagerSettings.Default.GamePath, "BepInEx", "plugins");
        }

        public static string SetGamePath(string path)
        {
            ManagerSettings.Default.GamePath = path;
            ManagerSettings.Default.Save();
            ManagerSettings.Default.Reload();

            return path;
        }

        public static bool IsBepInExInstalled()
        {
            return Directory.Exists(Path.Combine(ManagerSettings.Default.GamePath, "BepInEx"));
        }
    }
}
