using GorillaModManager.Models;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Reflection;

namespace GorillaModManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string ManagerVersion { get; } = "v1.0.0";

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
