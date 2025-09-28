using UHFReader.Core;
using UHFReader.Core.Readers;

namespace UHFReaderApp.Services
{
    public class ReaderService
    {
        private static ReaderService? _instance;
        private NetReader? _netReader;
        private ComReader? _comReader;
        
        public static ReaderService Instance => _instance ??= new ReaderService();

        public NetReader? NetReader 
        { 
            get => _netReader;
            set
            {
                _netReader?.Dispose();
                _netReader = value;
                OnConnectionChanged?.Invoke(IsConnected);
            }
        }

        public ComReader? ComReader 
        { 
            get => _comReader;
            set
            {
                _comReader?.Dispose();
                _comReader = value;
                OnConnectionChanged?.Invoke(IsConnected);
            }
        }

        public Reader? ActiveReader => (Reader?)NetReader ?? ComReader;
        
        public bool IsConnected => NetReader != null || ComReader != null;
        
        public string ConnectionType => NetReader != null ? "Network" : 
                                      ComReader != null ? "Serial" : 
                                      "Disconnected";

        public event Action<bool>? OnConnectionChanged;

        public void Disconnect()
        {
            NetReader = null;
            ComReader = null;
        }

        private ReaderService() { }
    }
}