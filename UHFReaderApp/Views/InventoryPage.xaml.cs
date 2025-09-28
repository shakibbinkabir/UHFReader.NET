using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using UHFReader.Core;
using UHFReaderApp.Services;
using Windows.System;

namespace UHFReaderApp.Views
{
    public sealed partial class InventoryPage : Page
    {
        public ObservableCollection<TagItem> Tags { get; } = new();
        private DispatcherTimer? scanTimer;
        private bool isScanning = false;
        private readonly ReaderService _readerService;

        public InventoryPage()
        {
            this.InitializeComponent();
            TagsListView.ItemsSource = Tags;
            _readerService = ReaderService.Instance;
        }

        private async void StartScanButton_Click(object sender, RoutedEventArgs e)
        {
            if (isScanning)
            {
                StopScanning();
                return;
            }

            var reader = _readerService.ActiveReader;
            
            if (reader == null)
            {
                StatusText.Text = "No reader connected. Please connect a reader first.";
                StatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
                return;
            }

            StartScanning(reader);
        }

        private void StartScanning(Reader reader)
        {
            isScanning = true;
            StartScanButton.Content = "Stop Scanning";
            StartScanButton.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
            StatusText.Text = $"Scanning for tags using {_readerService.ConnectionType} connection...";
            StatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Blue);
            
            scanTimer = new DispatcherTimer();
            scanTimer.Interval = TimeSpan.FromMilliseconds(500); // Scan every 500ms
            scanTimer.Tick += (s, e) => ScanForTags(reader);
            scanTimer.Start();
        }

        private void StopScanning()
        {
            isScanning = false;
            scanTimer?.Stop();
            scanTimer = null;
            
            StartScanButton.Content = "Start Scanning";
            StartScanButton.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
            StatusText.Text = $"Scanning stopped. Found {Tags.Count} unique tags.";
            StatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray);
        }

        private void ScanForTags(Reader reader)
        {
            try
            {
                var foundTags = reader.Inventory_G2(0, 0, 0);
                var newTagsCount = 0;
                
                foreach (var tagBytes in foundTags)
                {
                    var epc = ByteArrayToString(tagBytes);
                    var existingTag = Tags.FirstOrDefault(t => t.EPC == epc);
                    
                    if (existingTag != null)
                    {
                        existingTag.LastSeen = DateTime.Now;
                        existingTag.ReadCount++;
                    }
                    else
                    {
                        Tags.Add(new TagItem
                        {
                            EPC = epc,
                            FirstSeen = DateTime.Now,
                            LastSeen = DateTime.Now,
                            ReadCount = 1,
                            Length = tagBytes.Length
                        });
                        newTagsCount++;
                    }
                }
                
                // Update status
                if (foundTags.Count > 0)
                {
                    StatusText.Text = $"Found {foundTags.Count} tags this scan ({newTagsCount} new). Total unique: {Tags.Count}";
                }
                else
                {
                    StatusText.Text = $"Scanning... Total unique tags: {Tags.Count}";
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Scan error: {ex.Message}";
                StatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
                StopScanning();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Tags.Clear();
            StatusText.Text = "Tag list cleared.";
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);
                
                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("CSV Files", new List<string>() { ".csv" });
                savePicker.FileTypeChoices.Add("Text Files", new List<string>() { ".txt" });
                savePicker.SuggestedFileName = $"RFID_Tags_{DateTime.Now:yyyyMMdd_HHmmss}";

                var file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    var csv = "EPC,First Seen,Last Seen,Read Count,Length\n";
                    foreach (var tag in Tags)
                    {
                        csv += $"{tag.EPC},{tag.FirstSeen},{tag.LastSeen},{tag.ReadCount},{tag.Length}\n";
                    }
                    
                    await Windows.Storage.FileIO.WriteTextAsync(file, csv);
                    StatusText.Text = $"Exported {Tags.Count} tags to {file.Name}";
                    StatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Export error: {ex.Message}";
                StatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
            }
        }

        private static string ByteArrayToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }

    public class TagItem
    {
        public string EPC { get; set; } = "";
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public int ReadCount { get; set; }
        public int Length { get; set; }
        
        public string FirstSeenFormatted => FirstSeen.ToString("HH:mm:ss");
        public string LastSeenFormatted => LastSeen.ToString("HH:mm:ss");
    }
}