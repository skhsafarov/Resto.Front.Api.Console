using System;

namespace RestoFrontApiConsole
{
    /// <summary>
    /// Simplified wrapper around ConsoleLogger for easier usage in iiko plugins.
    /// Provides the same interface as System.Console but outputs to the Resto Front API Console.
    /// This is the Console.cs file that should be used in plugins.
    /// </summary>
    public static class Console
    {
        /// <summary>
        /// Writes a line to the console logger
        /// </summary>
        /// <param name="value">The value to write</param>
        public static void WriteLine(string value)
        {
            ConsoleLogger.WriteLine(value);
        }

        /// <summary>
        /// Writes a formatted line to the console logger
        /// </summary>
        /// <param name="format">The format string</param>
        /// <param name="args">The arguments for formatting</param>
        public static void WriteLine(string format, params object[] args)
        {
            ConsoleLogger.WriteLine(format, args);
        }

        /// <summary>
        /// Writes an empty line to the console logger
        /// </summary>
        public static void WriteLine()
        {
            ConsoleLogger.WriteLine("");
        }

        /// <summary>
        /// Writes a value to the console logger without a newline
        /// </summary>
        /// <param name="value">The value to write</param>
        public static void Write(string value)
        {
            ConsoleLogger.WriteLine(value); // Note: ConsoleLogger only supports WriteLine, so we use that
        }

        /// <summary>
        /// Writes a formatted string to the console logger without a newline
        /// </summary>
        /// <param name="format">The format string</param>
        /// <param name="args">The arguments for formatting</param>
        public static void Write(string format, params object[] args)
        {
            ConsoleLogger.WriteLine(format, args); // Note: ConsoleLogger only supports WriteLine, so we use that
        }

        /// <summary>
        /// Gets a value indicating whether the console is connected
        /// </summary>
        public static bool IsConnected => ConsoleLogger.IsConnected;

        /// <summary>
        /// Attempts to reconnect to the console
        /// </summary>
        public static void Reconnect()
        {
            ConsoleLogger.Reconnect();
        }

        /// <summary>
        /// Shuts down the console connection
        /// </summary>
        public static void Shutdown()
        {
            ConsoleLogger.Shutdown();
        }

        // Additional convenience methods for different data types

        /// <summary>
        /// Writes an object to the console logger
        /// </summary>
        /// <param name="value">The object to write</param>
        public static void WriteLine(object value)
        {
            ConsoleLogger.WriteLine(value?.ToString() ?? "null");
        }

        /// <summary>
        /// Writes an integer to the console logger
        /// </summary>
        /// <param name="value">The integer to write</param>
        public static void WriteLine(int value)
        {
            ConsoleLogger.WriteLine(value.ToString());
        }

        /// <summary>
        /// Writes a boolean to the console logger
        /// </summary>
        /// <param name="value">The boolean to write</param>
        public static void WriteLine(bool value)
        {
            ConsoleLogger.WriteLine(value.ToString());
        }

        /// <summary>
        /// Writes a decimal to the console logger
        /// </summary>
        /// <param name="value">The decimal to write</param>
        public static void WriteLine(decimal value)
        {
            ConsoleLogger.WriteLine(value.ToString());
        }

        /// <summary>
        /// Writes a double to the console logger
        /// </summary>
        /// <param name="value">The double to write</param>
        public static void WriteLine(double value)
        {
            ConsoleLogger.WriteLine(value.ToString());
        }

        /// <summary>
        /// Writes a DateTime to the console logger
        /// </summary>
        /// <param name="value">The DateTime to write</param>
        public static void WriteLine(DateTime value)
        {
            ConsoleLogger.WriteLine(value.ToString());
        }

        // Write methods for different data types

        /// <summary>
        /// Writes an object to the console logger
        /// </summary>
        /// <param name="value">The object to write</param>
        public static void Write(object value)
        {
            ConsoleLogger.WriteLine(value?.ToString() ?? "null");
        }

        /// <summary>
        /// Writes an integer to the console logger
        /// </summary>
        /// <param name="value">The integer to write</param>
        public static void Write(int value)
        {
            ConsoleLogger.WriteLine(value.ToString());
        }

        /// <summary>
        /// Writes a boolean to the console logger
        /// </summary>
        /// <param name="value">The boolean to write</param>
        public static void Write(bool value)
        {
            ConsoleLogger.WriteLine(value.ToString());
        }

        /// <summary>
        /// Writes a decimal to the console logger
        /// </summary>
        /// <param name="value">The decimal to write</param>
        public static void Write(decimal value)
        {
            ConsoleLogger.WriteLine(value.ToString());
        }

        /// <summary>
        /// Writes a double to the console logger
        /// </summary>
        /// <param name="value">The double to write</param>
        public static void Write(double value)
        {
            ConsoleLogger.WriteLine(value.ToString());
        }

        /// <summary>
        /// Helper method for logging different log levels
        /// </summary>
        /// <param name="level">Log level (INFO, WARN, ERROR)</param>
        /// <param name="message">The message to log</param>
        public static void WriteLog(string level, string message)
        {
            ConsoleLogger.WriteLine($"[{level.ToUpper()}] {message}");
        }

        /// <summary>
        /// Helper method for logging different log levels with formatting
        /// </summary>
        /// <param name="level">Log level (INFO, WARN, ERROR)</param>
        /// <param name="format">Format string</param>
        /// <param name="args">Arguments for formatting</param>
        public static void WriteLog(string level, string format, params object[] args)
        {
            ConsoleLogger.WriteLine($"[{level.ToUpper()}] {string.Format(format, args)}");
        }

        /// <summary>
        /// Writes an INFO level log message
        /// </summary>
        /// <param name="message">The message to log</param>
        public static void WriteInfo(string message)
        {
            WriteLog("INFO", message);
        }

        /// <summary>
        /// Writes an INFO level log message with formatting
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="args">Arguments for formatting</param>
        public static void WriteInfo(string format, params object[] args)
        {
            WriteLog("INFO", format, args);
        }

        /// <summary>
        /// Writes a WARN level log message
        /// </summary>
        /// <param name="message">The message to log</param>
        public static void WriteWarn(string message)
        {
            WriteLog("WARN", message);
        }

        /// <summary>
        /// Writes a WARN level log message with formatting
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="args">Arguments for formatting</param>
        public static void WriteWarn(string format, params object[] args)
        {
            WriteLog("WARN", format, args);
        }

        /// <summary>
        /// Writes an ERROR level log message
        /// </summary>
        /// <param name="message">The message to log</param>
        public static void WriteError(string message)
        {
            WriteLog("ERROR", message);
        }

        /// <summary>
        /// Writes an ERROR level log message with formatting
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="args">Arguments for formatting</param>
        public static void WriteError(string format, params object[] args)
        {
            WriteLog("ERROR", format, args);
        }

        /// <summary>
        /// Writes an exception to the console logger
        /// </summary>
        /// <param name="ex">The exception to log</param>
        public static void WriteException(Exception ex)
        {
            WriteError($"Exception: {ex.GetType().Name}: {ex.Message}");
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                WriteLine($"Stack trace: {ex.StackTrace}");
            }
            
            if (ex.InnerException != null)
            {
                WriteLine($"Inner exception: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
            }
        }
    }
}