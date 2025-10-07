using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SystemConsole = System.Console;

namespace RestoFrontApiConsole
{
    public class Program
    {
        private const string PipeName = "IikoPluginConsole";
        
        static async Task Main(string[] args)
        {

            var cts = new CancellationTokenSource();
            SystemConsole.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            await RunServerAsync(cts.Token);
        }

        static async Task RunServerAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var pipeServer = new NamedPipeServerStream(
                        PipeName,
                        PipeDirection.In,
                        NamedPipeServerStream.MaxAllowedServerInstances,
                        PipeTransmissionMode.Message,
                        PipeOptions.Asynchronous))
                    {
                        SystemConsole.WriteLine($"[{DateTime.Now:HH:mm:ss}] Waiting for plugin connection...");

                        await pipeServer.WaitForConnectionAsync(cancellationToken);

                        SystemConsole.WriteLine($"[{DateTime.Now:HH:mm:ss}] Plugin connected!\n");

                        await ReadFromPipeAsync(pipeServer, cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    SystemConsole.WriteLine("\nShutting down...");
                    break;
                }
                catch (Exception ex)
                {
                    SystemConsole.WriteLine($"[ERROR] {ex.Message}");
                    await Task.Delay(1000, cancellationToken);
                }
            }
        }

        static async Task ReadFromPipeAsync(NamedPipeServerStream pipeServer, CancellationToken cancellationToken)
        {
            using (var reader = new StreamReader(pipeServer, Encoding.UTF8))
            {
                try
                {
                    while (!cancellationToken.IsCancellationRequested && pipeServer.IsConnected)
                    {
                        string line = await reader.ReadLineAsync();
                        if (line == null)
                            break;

                        // Форматированный вывод с временной меткой и цветом
                        WriteColoredLine(line);
                    }
                }
                catch (IOException)
                {
                    SystemConsole.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] Plugin disconnected.");
                }
                catch (Exception ex)
                {
                    SystemConsole.WriteLine($"[ERROR] {ex.Message}");
                }
            }
        }

        static void WriteColoredLine(string line)
        {
            // Цветной вывод в зависимости от типа сообщения
            if (line.Contains("[ERROR]"))
            {
                SystemConsole.ForegroundColor = ConsoleColor.Red;
            }
            else if (line.Contains("[WARN]"))
            {
                SystemConsole.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (line.Contains("[INFO]") || line.Contains("success"))
            {
                SystemConsole.ForegroundColor = ConsoleColor.Green;
            }
            else if (line.Contains("===") || line.Contains("***"))
            {
                SystemConsole.ForegroundColor = ConsoleColor.Cyan;
            }

            SystemConsole.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {line}");
            SystemConsole.ResetColor();
        }
    }
}
