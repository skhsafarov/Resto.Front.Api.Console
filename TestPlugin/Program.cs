using System;
using System.Threading;
using RestoFrontApiConsole;

// Using alias to avoid conflict with System.Console
using Console = RestoFrontApiConsole.Console;

namespace TestPlugin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Test Plugin Started ===");
            
            // ????????? ConsoleLogger (backward compatibility)
            ConsoleLogger.WriteLine("Testing ConsoleLogger functionality (backward compatibility)");
            
            // ????????? ????? Console ?????
            Console.WriteInfo("Plugin initialized successfully");
            Console.WriteWarn("This is a warning message");
            Console.WriteError("This is an error message");
            
            // ????????? ??????????????? ?????
            var version = "2.0.0";
            var buildDate = DateTime.Now;
            Console.WriteLine("Plugin version: {0}, built on {1:yyyy-MM-dd HH:mm:ss}", version, buildDate);
            
            // ????????? ???????????
            if (Console.IsConnected)
            {
                Console.WriteInfo("Console connection established successfully!");
            }
            else
            {
                Console.WriteWarn("Console not connected, trying to reconnect...");
                Console.Reconnect();
            }
            
            // ????????? ????????? ???????
            for (int i = 1; i <= 5; i++)
            {
                var orderId = $"ORDER-{i:D4}";
                var total = (decimal)(100 + i * 25);
                
                Console.WriteLine("Processing order: {0}", orderId);
                Thread.Sleep(500);
                Console.WriteInfo("Order {0} completed with total: {1:C}", orderId, total);
                Thread.Sleep(500);
            }
            
            Console.WriteLine("*** Test completed successfully ***");
            
            // ????????? ????????? ???? ??????
            Console.WriteLine("Testing different data types:");
            Console.WriteLine(42);
            Console.WriteLine(true);
            Console.WriteLine(3.14159);
            Console.WriteLine(DateTime.Now);
            
            // ????????? ??????????
            try
            {
                throw new InvalidOperationException("Test exception for logging");
            }
            catch (Exception ex)
            {
                Console.WriteException(ex);
            }
            
            Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
            
            Console.WriteInfo("Plugin is shutting down...");
            Console.Shutdown();
        }
    }
}