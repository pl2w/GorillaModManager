namespace GorillaModManager.Models.Persistence
{
    /// <summary> Cached mod info from gamebanana for loading gamebanana specific info from the mod install view. </summary>
    public class GameBananaInfo
    {
        public string? iconUrl;
        public string? author;
        public string? description;

        public GameBananaInfo(string? iconUrl, string? author, string? description)
        {
            this.iconUrl = iconUrl;
            this.author = author;
            this.description = description;
        }
    }
}
