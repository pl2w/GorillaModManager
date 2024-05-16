using GorillaModManager.Models.Mods;
using GorillaModManager.Models.Persistence;
using GorillaModManager.Utils;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace GorillaModManager.Services
{
    public static class ItemInstaller
    {
        public static async Task InstallFromGameBanana(BrowserMod modToInstall)
        {
            string fullPath = Path.Combine(ManagerSettings.Default.GamePath, "BepInEx", "plugins", modToInstall.ModName);
            byte[] data = await HttpUtils.MakeGMClient().GetByteArrayAsync(modToInstall.DownloadUrl);

            if(!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            ZipFile.ExtractToDirectory(new MemoryStream(data), fullPath, true);
        }

        public static async Task InstallFromUrl(string url, string localPath)
        {
            string fullPath = Path.Combine(ManagerSettings.Default.GamePath, localPath);
            byte[] data = await HttpUtils.MakeGMClient().GetByteArrayAsync(url);
            ZipFile.ExtractToDirectory(new MemoryStream(data), fullPath, true);
        }
    }
}
