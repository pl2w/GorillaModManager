using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorillaModManager.Models
{
    public class ModInfo
    {
        public string modGuid = string.Empty;
        public string modName = string.Empty;
        public string modVersion = string.Empty;

        public ModInfo(string modGuid, string modName, string modVersion)
        {
            this.modGuid = modGuid;
            this.modName = modName;
            this.modVersion = modVersion;
        }
    }
}
