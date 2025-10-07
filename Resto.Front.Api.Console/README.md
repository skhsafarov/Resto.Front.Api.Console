# Resto.Front.Api.Console

Console application for capturing and displaying plugin logs from Resto Front API via Named Pipes.

## Description

This tool provides a real-time console interface for monitoring Iiko/Resto Front API plugin activities. It uses Named Pipes for inter-process communication, allowing plugins to send log messages that are displayed with color-coded output and timestamps.

## Features

- ? Real-time log monitoring via Named Pipes
- ? Color-coded output (ERROR, WARN, INFO)
- ? Timestamp for each message (HH:mm:ss.fff)
- ? Automatic reconnection on plugin disconnect
- ? Graceful shutdown with Ctrl+C

## Installation

```bash
dotnet tool install -g Resto.Front.Api.Console
```

Or via NuGet Package Manager:

```bash
Install-Package Resto.Front.Api.Console
```

## Usage

Simply run the executable:

```bash
Resto.Front.Console.exe
```

The console will wait for plugin connections on the Named Pipe `IikoPluginConsole`.

## Message Color Coding

- ?? **Red**: Messages containing `[ERROR]`
- ?? **Yellow**: Messages containing `[WARN]`
- ?? **Green**: Messages containing `[INFO]` or `success`
- ?? **Cyan**: Messages containing `===` or `***`

## Requirements

- .NET Framework 4.7.2 or higher
- Windows OS (Named Pipes)

## Integration Example

To send logs from your plugin to this console:

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
                pipeClient.Connect(100); // 100ms timeout
                using (var writer = new StreamWriter(pipeClient, Encoding.UTF8))
                {
                    writer.WriteLine(message);
                    writer.Flush();
                }
            }
        }
        catch
        {
            // Console not available, ignore
        }
    }
}
```

## License

MIT

## Author

Safarov Sardor
