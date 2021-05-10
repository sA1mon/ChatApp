using Client.ChatService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    public class Callback : IChatCallback
    {
        public async void GetMessage(string message)
        {
            await Task.Factory.StartNew(() =>
            {
                Program.MainChat.ChatBox.Items.Add(message);
                Program.MainChat.ChatBox.SelectedIndex = Program.MainChat.ChatBox.Items.Count - 1;
                Program.MainChat.ChatBox.SelectedIndex = -1;
            });
        }

        public async void GetHistory(Queue<string> messages)
        {
            await Task.Factory.StartNew(() =>
            {
                while (messages.Count > 0)
                {
                    Program.MainChat.ChatBox.Items.Add(messages.Dequeue());
                }
            });
        }
    }
}