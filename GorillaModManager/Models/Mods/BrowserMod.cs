namespace GorillaModManager.Models.Mods
{
    public class BrowserMod
    {
        public string ModName { get; set; } = string.Empty;
        public string ModShortDescription { get; set; } = string.Empty;
        public string ModAuthor { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
        public string ThumbnailImageUrl { get; set; } = string.Empty;
        public int DownloadsCount { get; set; } = -1;
        public int LikesCount { get; set; } = -1;

        public BrowserMod(string modName, string modShortDescription, string modAuthor, string downloadUrl, string thumbnailImageUrl, int downloadsCount, int likesCount)
        {
            this.ModName = modName;
            this.ModShortDescription = modShortDescription;
            this.ModAuthor = modAuthor;
            this.DownloadUrl = downloadUrl;
            this.ThumbnailImageUrl = thumbnailImageUrl;
            this.DownloadsCount = downloadsCount;
            this.LikesCount = likesCount;
        }
    }
}
