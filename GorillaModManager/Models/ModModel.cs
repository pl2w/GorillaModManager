namespace GorillaModManager.Models
{
    public class ModModel
    {
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;  
        public string Description { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
        public int DownloadCount { get; set; } = -1;

        public ModModel(string name, string author, string description, string downloadUrl, int downloadCount)
        {
            Name = name;
            Author = author;
            Description = description;
            DownloadUrl = downloadUrl;
            DownloadCount = downloadCount;
        }

        public ModModel(string downloadUrl)
        {
            DownloadUrl = downloadUrl;
        }
    }
}
