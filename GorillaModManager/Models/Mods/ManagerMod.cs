using System;
using System.IO;

namespace GorillaModManager.Models.Mods
{
    public class ManagerMod
    {
        public string ModName { get; set; } = string.Empty;
        public string ModGuid { get; set; } = string.Empty;
        public string ModVersion { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public string ModPath { get; set; } = string.Empty;

        public ManagerMod(string modName, string modGuid, string modVersion, bool enabled, string path)
        {
            this.ModName = modName;
            this.ModGuid = modGuid;
            this.ModVersion = modVersion;
            this.Enabled = enabled;
            this.ModPath = path;
        }

        public void Toggle()
        {
            try
            {
                if (File.Exists(ModPath + ".dll"))
                {
                    File.Move(ModPath + ".dll", ModPath + ".disabled");
                }
                else if (File.Exists(ModPath + ".disabled"))
                {
                    File.Move(ModPath + ".disabled", ModPath + ".dll");
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
