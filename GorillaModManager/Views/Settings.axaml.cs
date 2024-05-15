using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using GorillaModManager.Models;
using GorillaModManager.Models.Mods;
using GorillaModManager.Models.Persistence;
using GorillaModManager.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace GorillaModManager.Views
{
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();

            GorilaPath.IsReadOnly = true;

            GorilaPath.Text = ManagerSettings.Default.GamePath;

            HandlePathButtons();
        }

        private void HandlePathButtons()
        {
            if(File.Exists(Path.Combine(ManagerSettings.Default.GamePath, "Gorilla Tag.exe")))
            {
                InstallButton.IsEnabled = true;
                LaunchGame.IsEnabled = true;
                GotoGame.IsEnabled = true;
                UseBackup.IsEnabled = true;
            }
            else
            {
                InstallButton.IsEnabled = false;
                LaunchGame.IsEnabled = false;
                GotoGame.IsEnabled = false;
                UseBackup.IsEnabled = false;
            }

            if (File.Exists(Path.Combine(ManagerSettings.Default.GamePath, "winhttp.dll")))
            {
                UninstallButton.IsEnabled = true;
                ToggleButton.IsEnabled = true;
            }
            else
            {
                UninstallButton.IsEnabled = false;
                ToggleButton.IsEnabled = false;
            }

            if(Directory.Exists(Path.Combine(ManagerSettings.Default.GamePath, "BepInEx")))
            {
                BackupMods.IsEnabled = true;
            }
            else
            {
                BackupMods.IsEnabled = false;
            }
        }

        public async void OnPathClick(object sender, RoutedEventArgs args)
        {
            IReadOnlyList<IStorageFile> files = await TopLevel.GetTopLevel(this).StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Select 'Gorilla Tag.exe'." ,
                FileTypeFilter =
                [
                    new FilePickerFileType("Gorilla Filter") 
                    { 
                        Patterns =
                        [
                            "Gorilla Tag.exe"
                        ]
                    } 
                ]
            });

            if (files.Count <= 0)
                return;

            GorilaPath.Text = DataUtils.SetGamePath(Path.GetDirectoryName(files[0].Path.LocalPath));

            HandlePathButtons();
        }

        public async void BepInButtons(object sender, RoutedEventArgs args)
        {
            string objectName = ((Button)sender).Name;

            switch (objectName)
            {
                case "InstallButton":
                    await InstallationHandler.InstallFileFromUrl(
                        new InstallerMod("https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.1/BepInEx_win_x64_5.4.23.1.zip", "BepInEx"), string.Empty, false);

                    HandlePathButtons();
                    break;
                case "ToggleButton":
                    string winHttp = Path.Combine(ManagerSettings.Default.GamePath, "winhttp");
                    if (File.Exists(winHttp + ".dll"))
                    {
                        File.Move(winHttp + ".dll", winHttp + ".disabled");
                        ToggleButton.Content = "Enable BepInEx";
                    }
                    else if (File.Exists(winHttp + ".disabled"))
                    {
                        File.Move(winHttp + ".disabled", winHttp + ".dll");
                        ToggleButton.Content = "Disable BepInEx";
                    }
                    break;
                case "UninstallButton":
                    File.Delete(Path.Combine(ManagerSettings.Default.GamePath, "winhttp.dll"));
                    File.Delete(Path.Combine(ManagerSettings.Default.GamePath, ".doorstop_version"));
                    File.Delete(Path.Combine(ManagerSettings.Default.GamePath, "changelog.txt"));
                    File.Delete(Path.Combine(ManagerSettings.Default.GamePath, "doorstop_config.ini"));

                    if (DataUtils.IsBepInExInstalled())
                        Directory.Delete(Path.Combine(ManagerSettings.Default.GamePath, "BepInEx"), true);

                    HandlePathButtons();
                    break;
                case "BackupMods":
                    await HandleBackupMods();
                    break;
                case "UseBackup":
                    await UseBackupHandler();
                    break;
            }
        }

        async Task HandleBackupMods()
        {
            if (!DataUtils.IsBepInExInstalled())
                return;

            MemoryStream zipStream = new();
            ZipFile.CreateFromDirectory(Path.Combine(ManagerSettings.Default.GamePath, "BepInEx"), zipStream);

            var files = await TopLevel.GetTopLevel(this).StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Backup Location"
            });

            if (files.Count <= 0)
                return;

            await File.WriteAllBytesAsync(Path.Combine(files[0].Path.LocalPath, "BepInEx-Backup.zip"), zipStream.ToArray());
        }

        public void LaunchGorillaTag(object sender, RoutedEventArgs args)
        {
            if(Directory.GetParent(ManagerSettings.Default.GamePath).Name == "common")
            {
                Process.Start(new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = "steam://rungameid/1533390"
                });
            }
            else
            {
                string exeLoc = Path.Combine(ManagerSettings.Default.GamePath, "Gorilla Tag.exe");
                if (File.Exists(exeLoc))
                    Process.Start(exeLoc);
            }
        }

        public async Task UseBackupHandler()
        {
            if (ManagerSettings.Default.GamePath == string.Empty)
                return;

            var files = await TopLevel.GetTopLevel(this).StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                AllowMultiple = false,
                Title = "Backup Location",
                FileTypeFilter =
                [
                    new FilePickerFileType("Backup Filter")
                    {
                        Patterns =
                        [
                            "*.zip"
                        ]
                    }
                ]
            });

            if (files.Count <= 0)
                return;

            try
            {
                Directory.CreateDirectory(Path.Combine(ManagerSettings.Default.GamePath, "BepInEx"));

                MemoryStream stream = new(await File.ReadAllBytesAsync(files[0].Path.LocalPath));
                ZipFile.ExtractToDirectory(stream, Path.Combine(ManagerSettings.Default.GamePath, "BepInEx"), true);
            }
            catch (Exception e)
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Backup Failure.", e.Message,
                        ButtonEnum.Ok);

                await box.ShowAsync();
            }
        }

        public void GotoGorillaTag(object sender, RoutedEventArgs args)
        {
            Process.Start("explorer", ManagerSettings.Default.GamePath);
        }
    }
}