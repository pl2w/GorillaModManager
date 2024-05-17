using GorillaModManager.Models.Persistence;
using System.Collections.Generic;

namespace GorillaModManager.Models.Mods
{
    public class ModInfo
    {
        public string modGuid = string.Empty;
        public string modName = string.Empty;
        public string modVersion = string.Empty;
        public List<string> modDependencies = new List<string>();

        public GameBananaInfo? gameBananaInfo;

        public ModInfo(string modGuid, string modName, string modVersion, List<string> modDependencies)
        {
            this.modGuid = modGuid;
            this.modName = modName;
            this.modVersion = modVersion;
            this.modDependencies = modDependencies;
        }

        public ModInfo(string modGuid, string modName, string modVersion, List<string> modDependencies, GameBananaInfo gameBananaInfo)
        {
            this.modGuid = modGuid;
            this.modName = modName;
            this.modVersion = modVersion;
            this.modDependencies = modDependencies;
            this.gameBananaInfo = gameBananaInfo;
        }
    }
}
