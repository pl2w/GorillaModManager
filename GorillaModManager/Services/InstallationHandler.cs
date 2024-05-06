using GorillaModManager.Models;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace GorillaModManager.Services
{
    public static class InstallationHandler
    {
        public static List<ModModel> _queue { get; private set; } = new List<ModModel>();
        public static bool IsInstalling { get; private set; } = false;
        
        public static void AddToQueue(ModModel mod)
        {
            _queue.Add(mod);
        }

        public static void InstallQueue()
        {
            if (GlobalSettings.GorillaPath == string.Empty)
                return;

            StartInstallation();
        }

        static async void StartInstallation()
        {
            if (IsInstalling)
                return;

            IsInstalling = true;
            for (int i = 0; i < _queue.Count; i++)
            {
                await InstallFile(_queue[i], GlobalSettings.GetPluginsPath());
            }

            IsInstalling = false;
            _queue.Clear();
        }

        public static async Task InstallFile(ModModel mod, string location)
        {
            try
            {
                WebClient client = new WebClient();
                byte[] file = await client.DownloadDataTaskAsync(mod.DownloadUrl);

                string filename = new ContentDisposition(client.ResponseHeaders["content-disposition"]).FileName;

                var fileExt = filename.Split('.').Last();

                if (fileExt == "zip")
                {
                    MemoryStream stream = new MemoryStream(file);
                    ZipFile.ExtractToDirectory(stream, location, true);
                }
                else
                {
                    await File.WriteAllBytesAsync(Path.Combine(location, filename), file);
                }
            }
            catch (System.Exception)
            {

            }
        }
    }
}
