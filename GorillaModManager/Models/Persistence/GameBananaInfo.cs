namespace GorillaModManager.Models.Persistence
{
    /// <summary> Cached mod info from gamebanana for loading gamebanana specific info from the mod install view. </summary>
    public class GameBananaInfo
    {
        public byte[]? icon;
        public string? author;
        public string? description;

        public GameBananaInfo(byte[]? icon, string? author, string? description)
        {
            this.icon = icon;
            this.author = author;
            this.description = description;
        }
    }
}
