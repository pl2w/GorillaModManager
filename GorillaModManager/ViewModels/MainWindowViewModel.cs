using Avalonia.Controls;
using Avalonia.Interactivity;
using GorillaModManager.Models;
using GorillaModManager.Views;
using System.Diagnostics;

namespace GorillaModManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string ManagerVersion { get; } = GlobalSettings.Version;

        public static ModManagerViewModel ModManager { get; set; }

        public MainWindowViewModel()
        {
            ModManager = new ModManagerViewModel();
        }

        public void OnDiscordClick()
        {
            Process.Start("explorer", "https://discord.gg/monkemod");
        }
    }
}
