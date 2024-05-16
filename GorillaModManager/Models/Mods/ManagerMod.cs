using Avalonia.Media.Imaging;
using GorillaModManager.ViewModels;
using System;
using System.Diagnostics;
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

        // game banana info stuff
        public bool GameBananaInfoExists => ModIcon is object;
        public Bitmap ModIcon { get; set; } = null;
        public string ModDescription { get; set; } = string.Empty;
        public string ModAuthor { get; set; } = string.Empty;

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

        public void Uninstall()
        {
            try
            {
                if (Path.GetDirectoryName(ModPath + ".dll").EndsWith("plugins"))
                {
                    File.Delete(ModPath + ".dll");
                    File.Delete(ModPath + ".disabled");
                    goto Refresh;
                }
                var targetDirectory = Directory.GetParent(ModPath + ".dll") ?? throw new Exception();
                if (Directory.Exists(targetDirectory.FullName) && targetDirectory.Parent?.Name == "plugins") Directory.Delete(targetDirectory.FullName, true);
                else throw new Exception("Mod path is not in plugins folder, throwing error to prevent possible deletion of important files.");

                Refresh:
                MainWindowViewModel.ModManager.RefreshModList(MainWindowViewModel.ModManager.SearchText);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
