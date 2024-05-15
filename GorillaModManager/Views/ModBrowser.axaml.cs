using Avalonia.Controls;
using Avalonia.Interactivity;
using GorillaModManager.ViewModels;
using System;

namespace GorillaModManager.Views
{
    public partial class ModBrowser : UserControl
    {
        public ModBrowser()
        {
            InitializeComponent();
            this.DataContext = new ModBrowserViewModel();

            SearchIndex.IsReadOnly = true;
        }

        public void OnInstallButtonClick(object sender, RoutedEventArgs e)
        {
            string url = ((Button)sender).Name;
            (DataContext as ModBrowserViewModel).OnInstallClick(url);
        }

        public void OnPageClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
