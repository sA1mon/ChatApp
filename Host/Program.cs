using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using ChatApp;
using Host.ChatService;
using System.Runtime.Serialization.Formatters.Binary;

namespace Host
{
    [Serializable]
    internal class Callback : IChatCallback
    {
        private Queue<string> messages = new Queue<string>(100);
        public void GetMessage(string message)
        {
            Console.Write(message);

            if (messages.Count == 100)
            {
                messages.Dequeue();
                messages.Enqueue(message);
            }
            else
            {
                messages.Enqueue(message);
            }
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            using (var host = new ServiceHost(typeof(Chat)))
            {
                Callback server;
                try
                {
                    using (var fs = new FileStream("dialog.bin", FileMode.Open))
                    {
                        var bs = new BinaryFormatter();
                        server = (Callback)bs.Deserialize(fs);
                    }
                }
                catch 
                {
                    server = new Callback();
                }
                host.Open();
                Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] Host was started");
                var client = new ChatClient(new InstanceContext(server));
                var user = client.Add("Admin");

                while (true)
                {
                    var msg = Console.ReadLine();

                    if (msg == "!exit")
                    {
                        using (var fs = new FileStream("dialog.bin", FileMode.OpenOrCreate))
                        {
                            var bs = new BinaryFormatter();
                            bs.Serialize(fs, server);
                        }

                        Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] Wait... saving results");
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