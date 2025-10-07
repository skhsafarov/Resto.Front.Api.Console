# ?? Resto.Front.Api.Console

[![NuGet](https://img.shields.io/nuget/v/Resto.Front.Api.Console.svg)](https://www.nuget.org/packages/Resto.Front.Api.Console/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![GitHub](https://img.shields.io/badge/github-repo-blue.svg)](https://github.com/skhsafarov/Resto.Front.Api.Console)

Console application for real-time monitoring of Resto Front API plugin logs via Named Pipes.

## ?? Installation

```powershell
Install-Package Resto.Front.Api.Console
```

## ?? Usage

Run the console to start monitoring plugin logs:

```powershell
Resto.Front.Console.exe
```

The console listens on Named Pipe `IikoPluginConsole` for plugin connections.

## ? Features

- ? Real-time log monitoring via Named Pipes
- ? Color-coded output (ERROR, WARN, INFO)
- ? Millisecond timestamps
- ? Automatic reconnection on disconnect
- ? Graceful shutdown (Ctrl+C)

## ?? Development

### Build the package

```powershell
.\build-nuget.ps1
```

### Publish to NuGet.org

```powershell
.\publish-quick.ps1 -ApiKey "your-api-key"
```

Get your API key at: https://www.nuget.org/account/apikeys

## ?? Integration Example

Send logs from your plugin:

```csharp
using System.IO.Pipes;
using System.Text;

public class ConsoleLogger
{
    private const string PipeName = "IikoPluginConsole";
    
    public static void Log(string message)
    {
        try
        {
            using (var pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.Out))
            {
                pipeClient.Connect(100);
                using (var writer = new StreamWriter(pipeClient, Encoding.UTF8))
                {
                    writer.WriteLine(message);
                    writer.Flush();
                }
            }
        }
        catch { /* Console not available */ }
    }
}
```

## ?? Requirements

- .NET Framework 4.7.2+
- Windows OS

## ?? Links

- **NuGet Package**: https://www.nuget.org/packages/Resto.Front.Api.Console/
- **GitHub Repository**: https://github.com/skhsafarov/Resto.Front.Api.Console
- **Issues**: https://github.com/skhsafarov/Resto.Front.Api.Console/issues

## ?? License

MIT © 2025 Safarov Sardor

## ?? Author

**Safarov Sardor**

- GitHub: [@skhsafarov](https://github.com/skhsafarov)
- Repository: [Resto.Front.Api.Console](https://github.com/skhsafarov/Resto.Front.Api.Console)
