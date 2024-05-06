using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Collections.Generic;

namespace GorillaModManager.Views
{
    public partial class CustomItems : UserControl
    {
        public List<string> customItemFolders = new List<string>();

        public CustomItems()
        {
            InitializeComponent();
        }

        public async void RegisterButton(object sender, RoutedEventArgs args)
        {
            var folder = await TopLevel.GetTopLevel(this).StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = "Select a 'custom items' folder.",
                AllowMultiple = true
            });

            if (folder.Count <= 0)
                return;

            customItemFolders.Add(folder[0].Path.LocalPath);
        }
    }
}
