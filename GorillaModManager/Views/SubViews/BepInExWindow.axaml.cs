using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GorillaModManager.Views.SubViews
{
    public partial class BepInExWindow : Window
    {
        public BepInExWindow()
        {
            InitializeComponent();
        }

        public async void InstallBepInEx(object sender, RoutedEventArgs args)
        {
            Button button = (Button)sender;

            if (button.Name == "x64")
            {

            }
        }
    }
}
