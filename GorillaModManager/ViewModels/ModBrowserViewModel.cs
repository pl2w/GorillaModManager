using GorillaModManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GorillaModManager.ViewModels
{
    public class ModBrowserViewModel : ViewModelBase
    {
        public ObservableCollection<ModModel> ModItems { get; }

        public ModBrowserViewModel(IEnumerable<ModModel> mods) 
        {
            ModItems = new ObservableCollection<ModModel>(mods);
        }
    }
}
