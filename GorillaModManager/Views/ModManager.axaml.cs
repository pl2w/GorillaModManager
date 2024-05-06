using Avalonia.Controls;
using Avalonia.Interactivity;
using GorillaModManager.Models;
using System.IO;

namespace GorillaModManager.Views
{
    public partial class ModManager : UserControl
    {
        public ModManager()
        {
            InitializeComponent();
        }

        public void RefreshModLists(object sender, RoutedEventArgs args)
        {
            if (GlobalSettings.GorillaPath == string.Empty)
                return;

            string[] mods = Directory.GetFiles(GlobalSettings.GetPluginsPath(), "*.dll", SearchOption.AllDirectories);
            //RefreshButton.Content = mods.Length;
        }
    }
}
