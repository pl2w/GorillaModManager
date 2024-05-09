using Avalonia.Controls;
using GorillaModManager.Models;
using Mono.Cecil;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Dictionary<string, ModInfo> _cachedModInfos = new();

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
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<ModModel> modModels = new List<ModModel>();
            for (int i = 0; i < modFiles.Count; i++)
            {
                string modSimpleName = Path.GetFileNameWithoutExtension(modFiles[i]);
                string modPath = Path.GetDirectoryName(modFiles[i]);
                bool enabled = Path.GetExtension(modFiles[i]) == ".dll";

                if (searchTerm != null && !modSimpleName.ToLower().Contains(searchTerm.ToLower()))
                    continue;

                string modVersion = "v???";
                string modGuid = "Unknown";
                string modName = modSimpleName;

                if (!_cachedModInfos.TryGetValue($"{modPath}/{modSimpleName}", out ModInfo cachedInfo))
                {
                    List<TypeDefinition> types = AssemblyDefinition
                        .ReadAssembly(modFiles[i], new ReaderParameters { ReadWrite = true })
                        .MainModule
                        .Types
                        .Where(_ => _.IsPublic)
                        .ToList();

                    types.RemoveAll(x => x == null);

                    bool IsBepInPlugin = false;
                    TypeDefinition pluginType = null;
                    for (int j = 0; j < types.Count; j++)
                    {
                        if (types[j].BaseType?.FullName == "BepInEx.BaseUnityPlugin")
                        {
                            pluginType = types[j];
                            IsBepInPlugin = true;
                        }
                    }

                    if (IsBepInPlugin)
                    {
                        for (int z = 0; z < pluginType.CustomAttributes.Count; z++)
                        {
                            if (pluginType.CustomAttributes[z].Constructor.FullName.Contains("BepInEx.BepInPlugin"))
                            {
                                var values = pluginType.CustomAttributes[z].ConstructorArguments;
                                modGuid = (string)values[0].Value;
                                modName = (string)values[1].Value;
                                modVersion = $"v{(string)values[2].Value}";
                            }
                        }
                    }
                }

                if(cachedInfo != null)
                {
                    modVersion = cachedInfo.modVersion;
                    modName = cachedInfo.modName;
                    modGuid = cachedInfo.modGuid;
                }

                ModModel model = new ModModel
                (
                    modName,
                    modGuid,
                    modVersion,
                    enabled,
                    $"{modPath}/{modSimpleName}"
                );

                modModels.Add(model);

                if (!_cachedModInfos.ContainsKey($"{modPath}/{modSimpleName}"))
                {
                    ModInfo info = new(modGuid, modName, modVersion);
                    _cachedModInfos.Add($"{modPath}/{modSimpleName}", info);
                }
            }

            watch.Stop();
            Debug.WriteLine($"Took: {watch.Elapsed}");

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