using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using GorillaModManager.Models;
using GorillaModManager.Views.SubViews;

namespace GorillaModManager.Views
{
    public partial class Settings : UserControl
    {
        BepInExWindow? _bepInWindow;

        public Settings()
        {
            InitializeComponent();
            GorilaPath.IsReadOnly = true;
        }

        public static FilePickerFileType GorillaTagFile { get; } = new("Gorilla Tag")
        {
            Patterns = ["Gorilla Tag.exe", "GorillaTag.exe"]
        };

        public async void OnPathButtonClick(object sender, RoutedEventArgs args)
        {
            var gtagPath = await TopLevel.GetTopLevel(this).StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Select 'Gorilla Tag.exe'.",
                AllowMultiple = false,
                FileTypeFilter = [GorillaTagFile]
            });

            GlobalSettings.Path = gtagPath[0].Path.LocalPath;
            GorilaPath.Text = GlobalSettings.Path;
        }

        public void InstallBepInEx(object sender, RoutedEventArgs args)
        {
            if (GlobalSettings.Path == string.Empty)
                return;

            if (_bepInWindow?.IsVisible == true)
                return;

            _bepInWindow = new BepInExWindow();
            _bepInWindow.Show();
        }
    }
}
