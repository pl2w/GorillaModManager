using GorillaModManager.Services;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace GorillaModManager.Models.Mods
{
    public class BrowserMod(string modName, string modShortDescription, string modAuthor, string downloadUrl, string thumbnailImageUrl, int downloadsCount, int likesCount, JArray deps, string hash)
    {
        public string ModName { get; set; } = modName;
        public string ModShortDescription { get; set; } = modShortDescription;
        public string ModAuthor { get; set; } = modAuthor;
        public string DownloadUrl { get; set; } = downloadUrl;
        public string ThumbnailImageUrl { get; set; } = thumbnailImageUrl;
        public int DownloadsCount { get; set; } = downloadsCount;
        public int LikesCount { get; set; } = likesCount;
        public Dictionary<string, string> Dependencies { get; set; } = ParseJsonArray(deps);
        public string ValidHash { get; set; } = hash;

        public static Dictionary<string, string> ParseJsonArray(JArray deps)
        {
            if (deps == null)
                return null;

            Dictionary<string, string> depsToReturn = new Dictionary<string, string>();

            for (int i = 0; i < deps.Count; i++)
            {
                depsToReturn.Add
                (
                    deps[i].Values().ElementAt(0).ToString(),
                    deps[i].Values().ElementAt(1).ToString()
                );
            }

            return depsToReturn;
        }

        public async void InstallMod()
        {
            Debug.WriteLine($"Installing {ModName}");

            if(Dependencies?.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("This mod has dependencies listed on its GameBanana page.");
                sb.AppendLine("MAKE SURE TO INSTALL THEM YOURSELF OR THE MOD WILL LIKELY NOT WORK!");

                foreach(var dep in Dependencies)
                {
                    sb.AppendLine(dep.Key);
                }

                await MessageBoxManager
                    .GetMessageBoxStandard("Installer", sb.ToString(),
                        ButtonEnum.Ok).ShowAsync();
            }

            await ItemInstaller.InstallFromGameBanana(this);
        }
    }
}
