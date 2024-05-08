using System;
using System.IO;

namespace GorillaModManager.Models
{
    public class ModModel
    {
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;  
        public string Description { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
        public int DownloadCount { get; set; } = -1;
        public string Version { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;
        public string Path { get; set; } = string.Empty;

        public ModModel(string name, string author, string description, string downloadUrl, int downloadCount)
        {
            Name = name;
            Author = author;
            Description = description;
            DownloadUrl = downloadUrl;
            DownloadCount = downloadCount;
        }

        public ModModel(string modName, string modAuthor, string modVersion, bool isEnabled, string path)
        {
            Name = modName;
            Author = modAuthor;
            Version = modVersion;
            Enabled = isEnabled;
            Path = path;
        }

        public ModModel(string downloadUrl)
        {
            DownloadUrl = downloadUrl;
        }

        public void Toggle()
        {
            if(File.Exists(Path + ".dll"))
            {
                File.Move(Path + ".dll", Path + ".disabled");
            }
            else if (File.Exists(Path + ".disabled"))
            {
                File.Move(Path + ".disabled", Path + ".dll");
            }
        }
    }
}
