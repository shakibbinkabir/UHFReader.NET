using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIEx;
using UHFReaderApp.Views;

namespace UHFReaderApp.Views
{
    public sealed partial class MainWindow : WindowEx
    {
        public MainWindow()
        {
            this.InitializeComponent();
            
            // Set window properties for Windows 11
            this.Title = "UHF RFID Reader - Windows 11";
            this.SetIcon("Assets/uhf-icon.ico");
            this.IsMinimizable = true;
            this.IsMaximizable = true;
            this.IsResizable = true;
            
            // Set up navigation
            MainNavigation.SelectionChanged += MainNavigation_SelectionChanged;
            
            // Navigate to default page
            ContentFrame.Navigate(typeof(ConnectionPage));
            MainNavigation.SelectedItem = MainNavigation.MenuItems[0];
        }

        private void MainNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                string tag = selectedItem.Tag?.ToString() ?? "";
                
                Type pageType = tag switch
                {
                    "Connection" => typeof(ConnectionPage),
                    "Inventory" => typeof(InventoryPage),
                    "ReadWrite" => typeof(ReadWritePage),
                    "Settings" => typeof(SettingsPage),
                    "Logs" => typeof(LogsPage),
                    _ => typeof(ConnectionPage)
                };
                
                ContentFrame.Navigate(pageType);
            }
        }

        private void ContentFrame_NavigationFailed(object sender, Microsoft.UI.Xaml.Navigation.NavigationFailedEventArgs e)
        {
            // Handle navigation failure
        }
    }
}