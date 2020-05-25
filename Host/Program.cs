using ChatApp;
using Host.ChatService;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text.RegularExpressions;

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
                host.Open();

                WriteLineWithTime("Host was started");
                WriteLineWithTime("Admin logged in");
                var client = new ChatClient(new InstanceContext(server));
                var temp = host.SingletonInstance;
                var user = client.Add("Server", "");

                while (true)
                {
                    var msg = Console.ReadLine();
                    var regex = new Regex(@"^(?<command>!\w+)(\s(?<arg>(\-\-)?\w+))?");

                    if (regex.IsMatch(msg))
                    {
                        var command = regex.Replace(msg, @"${command}");
                        switch (command)
                        {
                            case "!exit":
                                {
                                    var args = regex.Replace(msg, "${arg}");
                                    client.Remove(user);
                                    WriteLineWithTime("Wait... saving results");
                                    client.Shutdown(args != "--force");

                                    return;
                                }
                            case "!ban":
                                {
                                    var suspect = regex.Replace(msg, "${arg}");
                                    if (client.Ban(suspect))
                                    {
                                        client.SendMessage($"{suspect} was banned.", new User());
                                    }
                                    break;
                                }
                            case "!unban":
                                {
                                    var suspect = regex.Replace(msg, "${arg}");
                                    if (client.Unban(suspect))
                                        WriteLineWithTime($"{suspect} unbanned.");
                                    break;
                                }
                            default:
                                {
                                    WriteLineWithTime("Wrong command");
                                    break;
                                }
                        }
                    }
                    else
                    {
                        WriteLineWithTime("Wrong command");
                    }
                }
            }
        }
    }
}