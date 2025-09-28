@echo off
REM Build script for UHF RFID Reader Windows 11 Application
REM Run this from the repository root directory

echo Building UHF RFID Reader Windows 11 Application...
echo.

REM Check if dotnet is available
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo Error: .NET SDK not found. Please install .NET 8 SDK.
    pause
    exit /b 1
)

echo Step 1: Cleaning previous builds...
dotnet clean UHFReader.NET.Modern.sln -c Release

echo.
echo Step 2: Restoring NuGet packages...
dotnet restore UHFReader.NET.Modern.sln

echo.
echo Step 3: Building Core Library...
dotnet build UHFReader.NET.Core\UHFReader.NET.Core.csproj -c Release

if errorlevel 1 (
    echo Error: Core library build failed.
    pause
    exit /b 1
)

echo.
echo Step 4: Building Windows 11 Application...
REM Note: This requires Windows 11 and Visual Studio 2022
dotnet build UHFReaderApp\UHFReaderApp.csproj -c Release

if errorlevel 1 (
    echo Warning: WinUI 3 application build failed.
    echo This is expected on non-Windows platforms.
    echo Please use Visual Studio 2022 on Windows 11 to build the complete application.
) else (
    echo.
    echo Build completed successfully!
    echo Application can be found in: UHFReaderApp\bin\Release\
)

echo.
echo Build process finished.
pause