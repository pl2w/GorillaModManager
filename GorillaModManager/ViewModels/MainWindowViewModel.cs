using GorillaModManager.Views;
using System.Diagnostics;

namespace GorillaModManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ModBrowser ModBrowser { get; } = new ModBrowser();
        public ModManager ModManager { get; } = new ModManager();
        public ModConfig ModConfig { get; } = new ModConfig();
        public CustomItems CustomItems { get; } = new CustomItems();
        public Settings Settings { get; } = new Settings();

        public void OnDiscordClick()
        {
            Process.Start("explorer", "https://discord.gg/monkemod");
        }
    }
}
