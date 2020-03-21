using System;
using System.Collections.Generic;
using System.ServiceModel;
using ChatApp;
using Host.ChatService;

namespace Host
{
    internal class Callback : IChatCallback
    {
        public void GetMessage(string message)
        {
            Console.Write(message);
        }

        public void GetHistory(Queue<string> messages)
        {
            //do nothing
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            using (var host = new ServiceHost(typeof(Chat)))
            {
                var server = new Callback();

                host.Open();
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Host was started");
                var client = new ChatClient(new InstanceContext(server));
                var user = client.Add("Admin");

                while (true)
                {
                    var msg = Console.ReadLine();

                    if (msg == "!exit")
                    {
                        client.Remove(user);
                        Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Wait... saving results");
                        client.Shutdown(true);
                        break;
                    }

                    client.SendMessage(msg, user);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}