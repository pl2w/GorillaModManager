using GorillaModManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorillaModManager.Services
{
    public class BrowserService
    {
        public IEnumerable<ModModel> GetMods() =>
        [
            new ModModel("awesome mod", "pl2squared", "this is a cool mod!", "https://google.com", 74)
        ];
    }
}
