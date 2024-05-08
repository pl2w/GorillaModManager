using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using GorillaModManager.Models;
using GorillaModManager.Services;
using System.IO;

namespace GorillaModManager.Views
{
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            GorilaPath.IsReadOnly = true;

            ToggleButtons(false, false, false);
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
            ToggleButtons(true, false, false);

            if (File.Exists(Path.Combine(GlobalSettings.GorillaPath, "winhttp.dll")))
            {
                ToggleBepInEx.Content = "Disable BepInEx";
                ToggleButtons(true, true, true);
            } else if (File.Exists(Path.Combine(GlobalSettings.GorillaPath, "winhttp.disabled")))
            {
                ToggleBepInEx.Content = "Enable BepInEx";
                ToggleButtons(true, true, true);
            }
        } 

        public async void BepInExButtons(object sender, RoutedEventArgs args)
        {
            if (GlobalSettings.GorillaPath == string.Empty)
            {
                ToggleButtons(false, false, false);
                return;
            }

            Button? btn = sender as Button;
            switch (btn?.Name)
            {
                case "UninstallButton":
                    File.Delete(Path.Combine(GlobalSettings.GorillaPath, "winhttp.dll"));
                    File.Delete(Path.Combine(GlobalSettings.GorillaPath, "changelog.txt"));
                    File.Delete(Path.Combine(GlobalSettings.GorillaPath, ".doorstop_version"));
                    File.Delete(Path.Combine(GlobalSettings.GorillaPath, "doorstop_config.ini"));
                    Directory.Delete(Path.Combine(GlobalSettings.GorillaPath, "BepInEx"), true);
                    return;
                case "ToggleBepInEx":
                    string winDll = Path.Combine(GlobalSettings.GorillaPath, "winhttp.dll");
                    string winDisabled = Path.Combine(GlobalSettings.GorillaPath, "winhttp.disabled");

                    if (File.Exists(winDll))
                    {
                        if (File.Exists(winDisabled))
                            File.Delete(winDisabled);

                        File.Move(winDll, Path.Combine(GlobalSettings.GorillaPath, "winhttp.disabled"));
                        ToggleBepInEx.Content = "Enable BepInEx";
                    }
                    else if(File.Exists(winDisabled))
                    {
                        if (File.Exists(winDll))
                            File.Delete(winDll);

                        File.Move(winDisabled, Path.Combine(GlobalSettings.GorillaPath, "winhttp.dll"));
                        ToggleBepInEx.Content = "Disable BepInEx";
                    }
                    else
                    {
                        ToggleButtons(true, false, false);
                    }
                    return;
                case "InstallButton":
                    await InstallationHandler.InstallFile(
                        new ModModel("https://github.com/BepInEx/BepInEx/releases/download/v5.4.23/BepInEx_win_x64_5.4.23.0.zip"), 
                        GlobalSettings.GorillaPath
                    );
                    break;
            }

            ToggleButtons(true, true, true);
        }

        public void ToggleButtons(bool install, bool uninstall, bool toggle)
        {
            InstallButton.IsEnabled = install;
            UninstallButton.IsEnabled = uninstall;
            ToggleBepInEx.IsEnabled = toggle;
        }
    }
}
