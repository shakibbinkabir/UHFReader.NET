using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Net;
using UHFReader.Core.Readers;
using UHFReaderApp.Services;

namespace UHFReaderApp.Views
{
    public sealed partial class ConnectionPage : Page
    {
        private readonly ReaderService _readerService;

        public ConnectionPage()
        {
            this.InitializeComponent();
            _readerService = ReaderService.Instance;
        }

        private async void ConnectNetworkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectNetworkButton.IsEnabled = false;
                NetworkStatusText.Text = "Connecting...";
                
                if (!IPAddress.TryParse(IpAddressTextBox.Text, out var ipAddress))
                {
                    NetworkStatusText.Text = "Invalid IP address";
                    return;
                }

                if (!int.TryParse(PortTextBox.Text, out var port) || port < 1 || port > 65535)
                {
                    NetworkStatusText.Text = "Invalid port number";
                    return;
                }

                var endpoint = new IPEndPoint(ipAddress, port);
                var netReader = new NetReader(endpoint);
                
                // Test connection by getting reader information
                byte[] versionInfo = new byte[32];
                byte readerType = 0;
                byte[] trType = new byte[16];
                byte dmaxfre = 0, dminfre = 0, powerdBm = 0, scanTime = 0;
                
                netReader.GetReaderInformation(versionInfo, ref readerType, trType, 
                    ref dmaxfre, ref dminfre, ref powerdBm, ref scanTime);
                
                _readerService.NetReader = netReader;
                
                NetworkStatusText.Text = "Connected successfully!";
                NetworkStatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
                DisconnectNetworkButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                NetworkStatusText.Text = $"Connection failed: {ex.Message}";
                NetworkStatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
            }
            finally
            {
                ConnectNetworkButton.IsEnabled = true;
            }
        }

        private void DisconnectNetworkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _readerService.NetReader = null;
                NetworkStatusText.Text = "Disconnected";
                NetworkStatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray);
                DisconnectNetworkButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                NetworkStatusText.Text = $"Disconnect error: {ex.Message}";
            }
        }

        private async void ConnectSerialButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectSerialButton.IsEnabled = false;
                SerialStatusText.Text = "Connecting...";
                
                if (!int.TryParse(SerialPortTextBox.Text, out var comPort) || comPort < 1)
                {
                    SerialStatusText.Text = "Invalid COM port number";
                    return;
                }

                byte baudRate = BaudRateComboBox.SelectedIndex switch
                {
                    0 => 5, // 9600
                    1 => 6, // 19200
                    2 => 7, // 38400
                    3 => 8, // 57600
                    4 => 9, // 115200
                    _ => 8  // Default to 57600
                };

                var comReader = new ComReader(baudRate, comPort);
                
                // Test connection by getting reader information
                byte[] versionInfo = new byte[32];
                byte readerType = 0;
                byte[] trType = new byte[16];
                byte dmaxfre = 0, dminfre = 0, powerdBm = 0, scanTime = 0;
                
                comReader.GetReaderInformation(versionInfo, ref readerType, trType, 
                    ref dmaxfre, ref dminfre, ref powerdBm, ref scanTime);
                
                _readerService.ComReader = comReader;
                
                SerialStatusText.Text = "Connected successfully!";
                SerialStatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
                DisconnectSerialButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                SerialStatusText.Text = $"Connection failed: {ex.Message}";
                SerialStatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
            }
            finally
            {
                ConnectSerialButton.IsEnabled = true;
            }
        }

        private void DisconnectSerialButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _readerService.ComReader = null;
                SerialStatusText.Text = "Disconnected";
                SerialStatusText.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray);
                DisconnectSerialButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                SerialStatusText.Text = $"Disconnect error: {ex.Message}";
            }
        }
    }
}