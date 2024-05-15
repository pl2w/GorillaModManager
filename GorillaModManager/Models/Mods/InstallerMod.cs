using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorillaModManager.Models.Mods
{
    public class InstallerMod
    {
        public string DownloadUrl { get; set; } = string.Empty;
        public string ModName { get; set; } = string.Empty;

        public InstallerMod(string downloadUrl, string modName)
        {
            this.DownloadUrl = downloadUrl;
            this.ModName = modName;
        }
    }
}
