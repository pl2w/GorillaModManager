using GorillaModManager.Models.Mods;
using GorillaModManager.Models.Persistence;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GorillaModManager.Services
{
    public static class InstallationHandler
    {
        public const int ZipIdentifier = 0x04034b50;
        public static List<string> currentlyInstallingMods = new List<string>(); 

        public static async Task InstallFileFromUrl(InstallerMod mod, string localPath, bool createFolder)
        {
            try
            {
                if (currentlyInstallingMods.Contains(mod.DownloadUrl))
                    return;

                currentlyInstallingMods.Add(mod.DownloadUrl);

                string fullPath = Path.Combine(ManagerSettings.Default.GamePath, localPath);

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Gorilla-Mod-Manager");

                byte[] downloadedBytes = await client.GetByteArrayAsync(mod.DownloadUrl);

                string pathForExtract = fullPath;
                if (createFolder)
                {
                    pathForExtract = Path.Combine(fullPath, mod.ModName);
                    Directory.CreateDirectory(pathForExtract);
                }

                if (BitConverter.ToInt32(downloadedBytes, 0) == ZipIdentifier)
                {
                    ZipFile.ExtractToDirectory(new MemoryStream(downloadedBytes), pathForExtract, true);

                    currentlyInstallingMods.Remove(mod.DownloadUrl);
                    return;
                }

                File.WriteAllBytes(pathForExtract + ".dll", downloadedBytes);

                currentlyInstallingMods.Remove(mod.DownloadUrl);
            }
            catch (Exception e)
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Installer Failure.", e.Message,
                        ButtonEnum.Ok);

                await box.ShowAsync();
            }
        }
    }
}