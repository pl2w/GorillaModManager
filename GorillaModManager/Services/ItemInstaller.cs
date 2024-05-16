using GorillaModManager.Models.Mods;
using GorillaModManager.Models.Persistence;
using GorillaModManager.Utils;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GorillaModManager.Services
{
    public static class ItemInstaller
    {
        public static async Task InstallFromGameBanana(BrowserMod modToInstall)
        {
            HttpClient client = HttpUtils.MakeGMClient();

            string fullPath = Path.Combine(ManagerSettings.Default.GamePath, "BepInEx", "plugins", modToInstall.ModName);
            byte[] data = await client.GetByteArrayAsync(modToInstall.DownloadUrl);
            string hash = GetMD5(data);

            // TODO: add warning
            if (modToInstall.ValidHash != hash)
                return;

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            ZipFile.ExtractToDirectory(new MemoryStream(data), fullPath, true);

            client.Dispose();
        }

        public static async Task InstallFromUrl(string url, string localPath)
        {
            HttpClient client = HttpUtils.MakeGMClient();

            string fullPath = Path.Combine(ManagerSettings.Default.GamePath, localPath);
            byte[] data = await client.GetByteArrayAsync(url);
            ZipFile.ExtractToDirectory(new MemoryStream(data), fullPath, true);

            client.Dispose();
        }

        // https://stackoverflow.com/questions/42543679/get-md5-checksum-of-byte-arrays-conent-in-c-sharp
        public static string GetMD5(byte[] inputData)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(inputData, 0, inputData.Length);
            stream.Seek(0, SeekOrigin.Begin);

            using (var md5Instance = MD5.Create())
            {
                var hashResult = md5Instance.ComputeHash(stream);
                return BitConverter.ToString(hashResult).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
