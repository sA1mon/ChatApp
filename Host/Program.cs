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

                host.AddServiceEndpoint(typeof(ChatApp.IChat), new NetTcpBinding {PortSharingEnabled = true},
                    "net.tcp://192.168.0.104:8731/");
                host.Open();
                
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Host was started");
                //Console.ReadLine();
                var client = new ChatClient(new InstanceContext(server));
                var user = client.Add("Admin");

                while (true)
                {
                    var msg = Console.ReadLine();

                    switch (msg.ToLower())
                    {
                        case "!exit":
                        {
                            client.Remove(user);
                            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Wait... saving results");
                            client.Shutdown(true);
                            return;
                        }
                        case "!exit --force":
                        {
                            client.Remove(user);
                            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Wait... saving results");
                            client.Shutdown(false);
                            return;
                        }
                        default:
                        {
                            client.SendMessage(msg, user);
                            break;
                        }
                    }
                }
            }
        }
    }
}