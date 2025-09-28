# UHF RFID Reader - Windows 11 Application

A comprehensive, modern Windows 11 application for UHF RFID tag reading and management, built with WinUI 3 and .NET 8.

## Features

### 🏢 Modern Windows 11 Design
- Native WinUI 3 interface with Windows 11 styling
- Responsive design that adapts to window sizes
- Modern navigation with NavigationView
- Fluent Design System integration

### 📡 Connection Management
- **Network Connection (TCP/IP)**: Connect to networked UHF readers
- **Serial Connection (COM Port)**: Connect to USB/RS232 UHF readers
- Support for multiple baud rates (9600 to 115200)
- Real-time connection status monitoring
- Automatic connection validation

### 🏷️ Tag Inventory & Scanning
- Real-time continuous tag scanning (500ms intervals)
- Automatic tag deduplication
- Read count tracking for each unique tag
- First seen / Last seen timestamps
- Tag length information
- Export functionality (CSV format)

### 📊 Data Management
- Live tag list with sorting capabilities
- CSV export with timestamps
- Clear list functionality
- Persistent tag tracking during scan sessions

### ⚙️ Additional Features
- Settings management (expandable)
- Application logging (expandable)
- Read/Write operations (expandable framework)
- Service-based architecture for scalability

## Project Structure

```
UHFReader.NET/
├── UHFReader.NET.Core/          # Modernized .NET 8 library
│   ├── Base/                    # Core reader functionality
│   ├── Constants/               # Error codes and results
│   ├── Exceptions/              # Exception handling
│   └── Readers/                 # Reader implementations
├── UHFReaderApp/                # WinUI 3 Windows 11 App
│   ├── Views/                   # UI Pages
│   ├── Services/                # Application services
│   ├── Assets/                  # App icons and images
│   └── App.xaml                 # Application definition
├── UHFReader.NET/               # Original .NET Framework library
└── Example/                     # Original console example
```

## Requirements

### Development Environment
- Windows 11 (required for WinUI 3)
- Visual Studio 2022 or later with:
  - .NET 8 SDK
  - Windows App SDK
  - Universal Windows Platform development workload

### Hardware
- UHF RFID Reader compatible with UHFReader18.dll
- Network-enabled readers (TCP/IP) or
- Serial/USB connected readers (COM port)

## Building and Running

### Prerequisites
1. Install Visual Studio 2022 with Windows 11 SDK
2. Install .NET 8 SDK
3. Install Windows App SDK

### Build Steps
1. Open `UHFReader.NET.Modern.sln` in Visual Studio
2. Restore NuGet packages
3. Build the solution (Ctrl+Shift+B)
4. Set `UHFReaderApp` as startup project
5. Run the application (F5)

### Deployment
The application can be deployed as:
- MSIX package for Microsoft Store
- Sideloaded MSIX for enterprise deployment
- Framework-dependent executable

## Usage Guide

### 1. Connection Setup
1. Launch the application
2. Navigate to the "Connection" tab
3. Choose connection type:
   - **Network**: Enter IP address and port (default: 192.168.50.52:6000)
   - **Serial**: Select COM port and baud rate (default: COM1, 57600)
4. Click "Connect" and verify successful connection

### 2. Tag Scanning
1. Navigate to the "Inventory" tab
2. Click "Start Scanning" to begin continuous tag detection
3. Tags will appear in the list as they're detected
4. View tag information: EPC, timestamps, read count, length
5. Click "Stop Scanning" to end the scan session

### 3. Data Export
1. After scanning, click "Export CSV"
2. Choose save location and filename
3. CSV file contains: EPC, First Seen, Last Seen, Read Count, Length

## Technical Architecture

### Core Library (UHFReader.NET.Core)
- **Target Framework**: .NET 8 Windows
- **Architecture**: Wrapper around UHFReader18.dll
- **Key Components**:
  - `ReaderBase`: Abstract base class for all readers
  - `NetReader`: Network TCP/IP reader implementation
  - `ComReader`: Serial COM port reader implementation
  - `UnmanagedLibrary`: DLL wrapper with proper resource management

### WinUI 3 Application (UHFReaderApp)
- **Target Framework**: .NET 8 Windows (Windows 11)
- **UI Framework**: WinUI 3 with Windows App SDK
- **Architecture Pattern**: MVVM-ready with service layer
- **Key Services**:
  - `ReaderService`: Centralized reader connection management
- **Key Views**:
  - `MainWindow`: Navigation host
  - `ConnectionPage`: Reader connection management
  - `InventoryPage`: Tag scanning and inventory
  - Additional pages: ReadWrite, Settings, Logs

### Dependencies
- **Microsoft.WindowsAppSDK**: Core WinUI 3 framework
- **WinUIEx**: Enhanced window management
- **CommunityToolkit.WinUI.UI.Controls**: Additional UI controls
- **System.IO.Ports**: Serial port communication

## Extensibility

The application is designed for easy extension:

### Adding New Reader Types
1. Inherit from `ReaderBase` in the Core library
2. Implement connection-specific logic
3. Add to `ReaderService` for management

### Adding New Features
1. Create new pages in `Views/` directory
2. Add navigation items to `MainWindow.xaml`
3. Implement business logic in service classes

### Custom Tag Operations
1. Extend `Reader` class with new methods
2. Add UI controls to relevant pages
3. Implement error handling and user feedback

## Troubleshooting

### Common Issues

**Connection Failures**
- Verify reader IP address and port
- Check Windows Firewall settings
- Ensure COM port is not in use by other applications
- Verify baud rate matches reader configuration

**Build Errors**
- Ensure Windows 11 SDK is installed
- Verify .NET 8 SDK installation
- Check NuGet package restoration
- Confirm Windows App SDK version compatibility

**Runtime Errors**
- Verify UHFReader18.dll and LoadMemLibrary.dll are present
- Check Windows version compatibility (Windows 11 required)
- Ensure proper permissions for hardware access

## License

This project builds upon the original UHFReader.NET library. Please refer to the original repository for licensing information.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly on Windows 11
5. Submit a pull request

## Support

For issues related to:
- **Hardware compatibility**: Refer to your UHF reader documentation
- **Application bugs**: Create an issue in the GitHub repository
- **Feature requests**: Discuss in GitHub issues

---

**Note**: This application requires Windows 11 and cannot be built or run on Linux/macOS due to WinUI 3 platform requirements.