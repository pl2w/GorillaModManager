using GorillaModManager.Models.Persistence;
using GorillaModManager.Services;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using ReactiveUI;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GorillaModManager.Models.Mods;
using System;

namespace GorillaModManager.ViewModels
{
    public class ModBrowserViewModel : ViewModelBase
    {
        public List<BrowserMod> ModsForPage
        {
            get
            {
                return _modsForPage;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _modsForPage, value);
            }
        }

        List<BrowserMod> _modsForPage;

        public BrowserService _service;

        public bool ModsFetched
        {
            get
            {
                return _modsFetched;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _modsFetched, value);
            }
        }

        bool _modsFetched;

        int _currentPage = 0;

        public ModBrowserViewModel()
        {
            ModsFetched = false;
            _service = new BrowserService();

            SetModsForPage(_currentPage);
        }

        private async void SetModsForPage(int page)
        {
            ModsFetched = false;
            var mods = await _service.GetMods(page);
            SetVisibleMods(mods);
        }

        void SetVisibleMods(IEnumerable<BrowserMod> mods)
        {
            ModsForPage = mods.ToList();
            ModsFetched = true;
        }

        public async void OnInstallClick(string modUrl)
        {
            Debug.WriteLine($"{modUrl}");

            BrowserMod browserMod = FindModForUrl(modUrl);
            InstallerMod mod = new InstallerMod(browserMod.DownloadUrl, browserMod.ModName);

            if (mod == null)
            {
                SetModsForPage(_currentPage);
                return;
            }

            if (!Directory.Exists(Path.Combine(ManagerSettings.Default.GamePath, "BepInEx", "plugins")))
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("Browser Failure.", "You have not setup bepinex properly or your game path is set incorrectly.",
                        ButtonEnum.Ok);

                await box.ShowAsync();
                return;
            }

            await InstallationHandler.InstallFileFromUrl(mod, "BepInEx/plugins", true);
        }

        private BrowserMod FindModForUrl(string modUrl)
        {
            for (int i = 0; i < ModsForPage.Count; i++)
            {
                if (ModsForPage[i].DownloadUrl == modUrl)
                {
                    return ModsForPage[i];
                }
            }

            return null;
        }
    }
}
