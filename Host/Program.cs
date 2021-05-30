using ChatApp;
using System;
using System.ServiceModel;

namespace Host
{
    internal static class Program
    {
        private static void WriteLineWithTime(string msg)
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {msg}");
        }

        private static void Main()
        {
            using (var host = new ServiceHost(typeof(Chat)))
            {
                host.Open();
                WriteLineWithTime("Host was started");
                WriteLineWithTime("Type \"shutdown\" to shutdown server");

                while (true)
                {
                    var cmd = Console.ReadLine();

                    if (cmd != null && cmd.ToLower() == "shutdown")
                        break;
                }
            }
        }
    }
}