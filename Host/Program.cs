using ChatApp;
using Host.ChatService;
using System;
using System.ServiceModel;
using System.Text;

namespace Host
{
    internal class Callback : IChatCallback
    {
        public void GetMessage(byte[] message, string senderName)
        {
            Console.WriteLine($"{senderName}: {Encoding.Default.GetString(message)}");
        }
    }

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
                var server = new Callback();
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