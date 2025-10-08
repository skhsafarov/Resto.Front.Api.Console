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
    public static class Console
    {
        private const string PipeName = "IikoPluginConsole";
        private static NamedPipeClientStream _pipeClient;
        private static StreamWriter _writer;
        private static readonly object _lock = new object();
        private static bool _isConnected;
        private static TextWriter _originalConsoleOut;
        private static ConsoleRedirectWriter _redirectWriter;
        private static bool _isInitialized;

        static Console()
        {
            // Автоматически определяем имя плагина из вызывающей сборки
            var callingAssembly = Assembly.GetCallingAssembly();
            var consoleExePath = callingAssembly.Location;

            // Запускаем Resto.Front.Console.exe если он существует
            if (File.Exists(consoleExePath))
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = consoleExePath,
                    CreateNoWindow = false,  // Показываем окно консоли
                    UseShellExecute = true,   // Используем shell для запуска - создаст новое окно
                    WorkingDirectory = Path.GetDirectoryName(consoleExePath)
                };

                try
                {
                    Process.Start(processStartInfo);

                    SystemConsole.WriteLine($"[INFO] Started Resto.Front.Console.exe from: {consoleExePath}");

                    // Даём время консоли запуститься перед подключением
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    SystemConsole.WriteLine($"[WARN] Failed to start Resto.Front.Console.exe: {ex.Message}");
                }
            }
            else
            {
                SystemConsole.WriteLine($"[WARN] Resto.Front.Console.exe not found at: {consoleExePath}");
            }

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

                    _pipeClient.Connect(2000);
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
                // Сохраняем оригинальный вывод Console
                _originalConsoleOut = SystemConsole.Out;

                // Создаём перенаправляющий writer
                _redirectWriter = new ConsoleRedirectWriter(_originalConsoleOut);

                // Перенаправляем Console.Out
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
        /// Записывает сообщение в консоль мониторинга.
        /// </summary>
        /// <param name="message">Сообщение для вывода</param>
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
        /// Записывает форматированное сообщение в консоль мониторинга.
        /// </summary>
        /// <param name="format">Строка формата</param>
        /// <param name="args">Аргументы для форматирования</param>
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// Получает значение, указывающее, установлено ли подключение к консоли мониторинга.
        /// </summary>
        public static bool IsConnected
        {
            get { return _isConnected; }
        }

        /// <summary>
        /// Закрывает соединение и восстанавливает оригинальный Console.Out
        /// </summary>
        public static void Shutdown()
        {
            lock (_lock)
            {
                if (!_isInitialized)
                    return;

                try
                {
                    // Восстанавливаем оригинальный вывод Console
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
        /// Writer который перенаправляет Console.WriteLine в ConsoleLogger
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
                // Отправляем в ConsoleLogger
                Console.WriteLine(value);

                // Также выводим в оригинальную консоль (если есть)
                if (_originalOutput != null)
                    _originalOutput.WriteLine(value);
            }

            public override void Write(string value)
            {
                // Для Write просто передаём в оригинальный поток
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
