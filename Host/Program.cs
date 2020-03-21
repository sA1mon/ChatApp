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
        private static void WriteLineWithTime(string msg)
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {msg}");
        }

        private static void Main()
        {
            using (var host = new ServiceHost(typeof(Chat)))
            {
                var server = new Callback();

                host.AddServiceEndpoint(typeof(ChatApp.IChat), new NetHttpBinding(), "http://192.168.0.104:8731/");
                //host.AddServiceEndpoint(typeof(ChatApp.IChat), new NetTcpBinding {PortSharingEnabled = true},
                //    "net.tcp://192.168.0.104:8731/");
                host.Open();
                
                WriteLineWithTime("Host was started");
                WriteLineWithTime("Press any key to log in as Admin");
                Console.ReadKey();
                WriteLineWithTime("You are logged in");
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
                            WriteLineWithTime("Wait... saving results");
                            client.Shutdown(true);
                            return;
                        }
                        case "!exit --force":
                        {
                            client.Remove(user);
                            WriteLineWithTime("Wait... stop the server");
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