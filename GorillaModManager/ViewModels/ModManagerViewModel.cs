using Avalonia.Controls;
using GorillaModManager.Models;
using Mono.Cecil;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;

namespace GorillaModManager.ViewModels
{
    public class ModManagerViewModel : ViewModelBase
    {
        public List<ModModel> InstalledMods
        {
            get
            {
                return _installedMods;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _installedMods, value);
            }
        }

        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        string _searchText;
        public bool isAdvanced = false;
        List<ModModel> _installedMods;

        public ModManagerViewModel()
        {
            this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(400))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RefreshModList!);
        }

        public async void RefreshModList(string? search)
        {
            if (!GlobalSettings.DoesPluginsExist())
                return;

            List<string> mods = Directory.GetFiles(GlobalSettings.GetPluginsPath(), "*.dll", SearchOption.AllDirectories).ToList();
            mods.AddRange(Directory.GetFiles(GlobalSettings.GetPluginsPath(), "*.disabled", SearchOption.AllDirectories));
            
            InstalledMods = GetValidMods(mods, search);
        }

        private List<ModModel> GetValidMods(List<string> modFiles, string searchTerm)
        {
            List<ModModel> modModels = new List<ModModel>();
            for (int i = 0; i < modFiles.Count; i++)
            {
                string modName = Path.GetFileNameWithoutExtension(modFiles[i]);
                string modPath = Path.GetDirectoryName(modFiles[i]);
                bool enabled = Path.GetExtension(modFiles[i]) == ".dll";

                if (searchTerm != null && !modName.ToLower().Contains(searchTerm.ToLower()))
                    continue;

                modModels.Add(new ModModel
                (
                    modName,
                    "Unknown",
                    "v???",
                    enabled,
                    $"{modPath}/{modName}"
                ));
            }

            return modModels;
        }

        public void ToggleMods()
        {
            if (InstalledMods == null || InstalledMods.Count <= 0)
                return;

            for (int i = 0; i < InstalledMods.Count; i++)
            {
                InstalledMods[i].Toggle();
            }

            RefreshModList(SearchText);
        }

        public void ToggleAdvanced()
        {
            isAdvanced = !isAdvanced;
            RefreshModList(SearchText);
        }
    }
}