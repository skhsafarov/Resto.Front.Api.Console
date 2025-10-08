using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using System.Threading;

using SystemConsole = System.Console;

namespace RestoFrontApiConsole
{
    /// <summary>
    /// ???????????? ????? ??? ???????? ????????? ? ??????? ??????????? Resto Front API
    /// </summary>
    public static class ConsoleLogger
    {
        private const string PipeName = "IikoPluginConsole";
        private static NamedPipeClientStream _pipeClient;
        private static StreamWriter _writer;
        private static readonly object _lock = new object();
        private static bool _isConnected;
        private static TextWriter _originalConsoleOut;
        private static ConsoleRedirectWriter _redirectWriter;
        private static bool _isInitialized;

        static ConsoleLogger()
        {
            Initialize();
        }

        private static void Initialize()
        {
            // ????????????? ?????????? ???? ? ??????????? ??????????
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            var consoleExePath = Path.Combine(assemblyDirectory, "Resto.Front.Console.exe");

            // ????????? Resto.Front.Console.exe ???? ?? ??????????
            if (File.Exists(consoleExePath))
            {
                StartConsoleApplication(consoleExePath);
            }
            else
            {
                // ????????? ????? ? ?????? ??????????? ??????
                var alternativePaths = new[]
                {
                    Path.Combine(assemblyDirectory, "tools", "Resto.Front.Console.exe"),
                    Path.Combine(Environment.CurrentDirectory, "Resto.Front.Console.exe")
                };

                foreach (var path in alternativePaths)
                {
                    if (File.Exists(path))
                    {
                        StartConsoleApplication(path);
                        consoleExePath = path;
                        break;
                    }
                }
            }

            ConnectToPipe();
        }

        private static void StartConsoleApplication(string consoleExePath)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = consoleExePath,
                CreateNoWindow = false,
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(consoleExePath)
            };

            try
            {
                Process.Start(processStartInfo);
                SystemConsole.WriteLine($"[INFO] Started Resto.Front.Console.exe from: {consoleExePath}");
                
                // ???? ????? ??????? ??????????? ????? ????????????
                Thread.Sleep(1500);
            }
            catch (Exception ex)
            {
                SystemConsole.WriteLine($"[WARN] Failed to start Resto.Front.Console.exe: {ex.Message}");
            }
        }

        private static void ConnectToPipe()
        {
            lock (_lock)
            {
                if (_isInitialized)
                    return;

                try
                {
                    _pipeClient = new NamedPipeClientStream(
                        ".",
                        PipeName,
                        PipeDirection.Out,
                        PipeOptions.Asynchronous);

                    _pipeClient.Connect(3000);
                    _writer = new StreamWriter(_pipeClient, Encoding.UTF8) { AutoFlush = true };
                    _isConnected = true;

                    RedirectConsoleOutput();
                    _isInitialized = true;
                }
                catch (TimeoutException)
                {
                    _isConnected = false;
                    _isInitialized = true;
                }
                catch (Exception ex)
                {
                    _isConnected = false;
                    _isInitialized = true;
                    SystemConsole.WriteLine($"[WARN] ConsoleLogger failed to connect: {ex.Message}");
                }
            }
        }

        private static void RedirectConsoleOutput()
        {
            try
            {
                _originalConsoleOut = SystemConsole.Out;
                _redirectWriter = new ConsoleRedirectWriter(_originalConsoleOut);
                SystemConsole.SetOut(_redirectWriter);
            }
            catch (Exception ex)
            {
                SystemConsole.WriteLine($"[WARN] Failed to redirect console output: {ex.Message}");
            }
        }

        private static void RestoreConsoleOutput()
        {
            try
            {
                if (_originalConsoleOut != null)
                {
                    SystemConsole.SetOut(_originalConsoleOut);
                }
            }
            catch (Exception ex)
            {
                SystemConsole.WriteLine($"[WARN] Failed to restore console output: {ex.Message}");
            }
        }

        /// <summary>
        /// ?????????? ????????? ? ??????? ???????????.
        /// </summary>
        /// <param name="message">????????? ??? ??????</param>
        public static void WriteLine(string message)
        {
            if (!_isConnected)
                return;

            lock (_lock)
            {
                try
                {
                    _writer?.WriteLine(message);
                }
                catch (IOException)
                {
                    _isConnected = false;
                }
                catch (Exception ex)
                {
                    SystemConsole.WriteLine($"[WARN] ConsoleLogger write error: {ex.Message}");
                    _isConnected = false;
                }
            }
        }

        /// <summary>
        /// ?????????? ??????????????? ????????? ? ??????? ???????????.
        /// </summary>
        /// <param name="format">?????? ???????</param>
        /// <param name="args">????????? ??? ??????????????</param>
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// ???????? ????????, ???????????, ??????????? ?? ??????????? ? ??????? ???????????.
        /// </summary>
        public static bool IsConnected
        {
            get { return _isConnected; }
        }

        /// <summary>
        /// ????????????? ???????????????? ? ???????
        /// </summary>
        public static void Reconnect()
        {
            lock (_lock)
            {
                if (_isConnected)
                {
                    Shutdown();
                }
                
                _isInitialized = false;
                ConnectToPipe();
            }
        }

        /// <summary>
        /// ????????? ?????????? ? ??????????????? ???????????? Console.Out
        /// </summary>
        public static void Shutdown()
        {
            lock (_lock)
            {
                if (!_isInitialized)
                    return;

                try
                {
                    RestoreConsoleOutput();

                    if (_redirectWriter != null)
                        _redirectWriter.Dispose();
                    if (_writer != null)
                        _writer.Dispose();
                    if (_pipeClient != null)
                        _pipeClient.Dispose();
                }
                catch { }
                finally
                {
                    _isConnected = false;
                    _isInitialized = false;
                }
            }
        }

        /// <summary>
        /// Writer ??????? ?????????????? Console.WriteLine ? ConsoleLogger
        /// </summary>
        private class ConsoleRedirectWriter : TextWriter
        {
            private readonly TextWriter _originalOutput;

            public ConsoleRedirectWriter(TextWriter originalOutput)
            {
                _originalOutput = originalOutput;
            }

            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }

            public override void WriteLine(string value)
            {
                // ?????????? ? ConsoleLogger
                ConsoleLogger.WriteLine(value);

                // ????? ??????? ? ???????????? ??????? (???? ????)
                if (_originalOutput != null)
                    _originalOutput.WriteLine(value);
            }

            public override void Write(string value)
            {
                // ??? Write ?????? ???????? ? ???????????? ?????
                if (_originalOutput != null)
                    _originalOutput.Write(value);
            }

            public override void Write(char value)
            {
                if (_originalOutput != null)
                    _originalOutput.Write(value);
            }
        }
    }
}