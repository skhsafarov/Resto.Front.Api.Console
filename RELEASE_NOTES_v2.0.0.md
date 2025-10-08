# Release Notes - Resto.Front.Api.Console v2.0.0

?? **Major Release** - Improved API and Enhanced Features

## ?? New Features

### Console.cs Wrapper Class
- **Simplified API**: New `Console` class provides easier interface similar to `System.Console`
- **Type Safety**: Support for multiple data types (`int`, `bool`, `decimal`, `DateTime`, etc.)
- **Logging Levels**: Dedicated methods for different log levels:
  - `Console.WriteInfo()` - Green colored INFO messages
  - `Console.WriteWarn()` - Yellow colored WARNING messages  
  - `Console.WriteError()` - Red colored ERROR messages
  - `Console.WriteException()` - Comprehensive exception logging

### Enhanced Usage Examples
```csharp
using Console = RestoFrontApiConsole.Console;

// Simple usage
Console.WriteLine("Hello from plugin!");
Console.WriteInfo("Plugin started successfully");
Console.WriteWarn("Low inventory warning");
Console.WriteError("Connection failed");

// Exception handling
try {
    // risky code
} catch (Exception ex) {
    Console.WriteException(ex); // Full exception details
}
```

### Automatic Integration
- **Auto-start**: Console application starts automatically when first log message is sent
- **MSBuild Integration**: Console executable is automatically copied to plugin output directory
- **Zero Configuration**: Works out of the box after NuGet package installation

## ?? Improvements

### Better Documentation
- Comprehensive README with usage examples
- Step-by-step integration guide
- Troubleshooting section
- API reference documentation

### Enhanced Package Structure
- Clean MSBuild targets for seamless integration
- Proper NuGet package metadata
- Symbol packages for debugging support

### Backward Compatibility
- Existing `ConsoleLogger` API remains fully functional
- No breaking changes for current users
- Smooth migration path to new `Console` API

## ?? Package Contents

- **Library**: `Resto.Front.Api.Console.dll` (v2.0.0)
- **Console App**: `Resto.Front.Console.exe`
- **MSBuild Targets**: Automatic file copying and integration
- **Documentation**: Comprehensive usage guides

## ?? Getting Started

### Installation
```powershell
Install-Package Resto.Front.Api.Console
```

### Quick Start
```csharp
using RestoFrontApiConsole;
using Console = RestoFrontApiConsole.Console;

public class MyPlugin 
{
    public void Initialize() 
    {
        Console.WriteInfo("Plugin initialized successfully!");
    }
}
```

## ?? Testing

Included test project demonstrates all features:
- Basic logging functionality
- Different log levels
- Exception handling
- Auto-reconnection
- Multiple data types

## ?? Quality Metrics

- ? Zero compilation warnings
- ? .NET Framework 4.7.2 compatibility
- ? Thread-safe operations
- ? Automatic resource cleanup
- ? Comprehensive error handling

## ?? Links

- **NuGet Package**: https://www.nuget.org/packages/Resto.Front.Api.Console
- **GitHub Repository**: https://github.com/skhsafarov/Resto.Front.Api.Console
- **Documentation**: See README.md for detailed usage guide

## ?? Credits

Developed by Sardor Safarov for the iiko ecosystem.

## ?? Known Issues

None at this time. Report issues on GitHub.

---

**Happy Logging!** ??