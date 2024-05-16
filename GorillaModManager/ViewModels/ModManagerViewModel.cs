using GorillaModManager.Models;
using GorillaModManager.Models.Mods;
using GorillaModManager.Models.Persistence;
using Mono.Cecil;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace GorillaModManager.ViewModels
{
    public class ModManagerViewModel : ViewModelBase
    {
        public List<ManagerMod> InstalledMods
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

        public Dictionary<string, ModInfo> _cachedModInfos = [];

        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        string _searchText = string.Empty;
        List<ManagerMod> _installedMods = [];

        public ModManagerViewModel()
        {
            this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RefreshModList!);
        }

        public void RefreshModList(string? search)
        {
            if (!DataUtils.DoesPluginsExist())
                return;

            List<string> mods = [.. Directory.GetFiles(DataUtils.Plugins(), "*.dll", SearchOption.AllDirectories)];
            mods.AddRange(Directory.GetFiles(DataUtils.Plugins(), "*.disabled", SearchOption.AllDirectories));
            
            InstalledMods = GetValidMods(mods, search);
        }

        private List<ManagerMod> GetValidMods(List<string> modFiles, string searchTerm)
        {
            Stopwatch watch = new();
            watch.Start();

            List<ManagerMod> ManagerMods = [];
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
                List<string> modDependencies = new List<string>();

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
                        for (int z = 0; z < pluginType?.CustomAttributes.Count; z++)
                        {
                            if (pluginType.CustomAttributes[z].Constructor.FullName.Contains("BepInEx.BepInPlugin"))
                            {
                                var values = pluginType.CustomAttributes[z].ConstructorArguments;
                                modGuid = (string)values[0].Value;
                                modName = (string)values[1].Value;
                                modVersion = $"v{(string)values[2].Value}";
                            }

                            //if (pluginType.CustomAttributes[z].Constructor.FullName.Contains("BepInEx.BepInDependency"))
                            //{
                            //    var values = pluginType.CustomAttributes[z].ConstructorArguments;
                            //    modDependencies.Add((string)values[0].Value);
                            //}
                        }
                    }
                }

                if(cachedInfo != null)
                {
                    modVersion = cachedInfo.modVersion;
                    modName = cachedInfo.modName;
                    modGuid = cachedInfo.modGuid;
                    modDependencies = cachedInfo.modDependencies;
                }

                ManagerMod model = new
                (
                    modName,
                    modGuid,
                    modVersion,
                    enabled,
                    $"{modPath}/{modSimpleName}"
                );

                ManagerMods.Add(model);

                if (!_cachedModInfos.ContainsKey($"{modPath}/{modSimpleName}"))
                {
                    ModInfo info = new(modGuid, modName, modVersion, modDependencies);
                    _cachedModInfos.Add($"{modPath}/{modSimpleName}", info);
                }
            }

            watch.Stop();
            Debug.WriteLine($"Took: {watch.Elapsed}");

            return ManagerMods;
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
    }
}