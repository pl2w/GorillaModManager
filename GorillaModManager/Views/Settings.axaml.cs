using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using GorillaModManager.Models;
using GorillaModManager.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace GorillaModManager.Views
{
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            GorilaPath.IsReadOnly = true;
            InstallButton.IsEnabled = false;
            UninstallButton.IsEnabled = false;
        }

        public static FilePickerFileType GorillaTagFile { get; } = new("Gorilla Tag")
        {
            Patterns = ["Gorilla Tag.exe", "GorillaTag.exe"]
        };

        public async void OnPathButtonClick(object sender, RoutedEventArgs args)
        {
            var gtagPath = await TopLevel.GetTopLevel(this).StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Select 'Gorilla Tag.exe'.",
                AllowMultiple = false,
                FileTypeFilter = [GorillaTagFile]
            });

            if(gtagPath.Count <= 0)
                return;

            GlobalSettings.GorillaPath = Path.GetDirectoryName(gtagPath[0].Path.LocalPath);
            GorilaPath.Text = GlobalSettings.GorillaPath;
            InstallButton.IsEnabled = true;

            if (File.Exists(Path.Combine(GlobalSettings.GorillaPath, "winhttp.dll")))
                UninstallButton.IsEnabled = true;
        }

        public async void BepInExButtons(object sender, RoutedEventArgs args)
        {
            if (GlobalSettings.GorillaPath == string.Empty)
            {
                InstallButton.IsEnabled = false;
                UninstallButton.IsEnabled = false;
                return;
            }

            Button? btn = sender as Button;

            if(btn?.Name == "UninstallButton")
            {
                File.Delete(Path.Combine(GlobalSettings.GorillaPath, "winhttp.dll"));
                File.Delete(Path.Combine(GlobalSettings.GorillaPath, "changelog.txt"));
                File.Delete(Path.Combine(GlobalSettings.GorillaPath, ".doorstop_version"));
                File.Delete(Path.Combine(GlobalSettings.GorillaPath, "doorstop_config.ini"));
                Directory.Delete(Path.Combine(GlobalSettings.GorillaPath, "BepInEx"), true);
                return;
            }

            await InstallationHandler.InstallFile(new ModModel("https://github.com/BepInEx/BepInEx/releases/download/v5.4.23/BepInEx_win_x64_5.4.23.0.zip"), GlobalSettings.GorillaPath);
        }
    }
}
