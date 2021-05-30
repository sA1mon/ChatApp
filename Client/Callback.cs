using Client.ChatService;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Callback : IChatCallback
    {
        public async void GetMessage(byte[] message, string senderName)
        {
            var data = Program.MainChat.Rsa.Decrypt(message);
            var textData = $"{senderName}: {Encoding.Default.GetString(data)}";

            await Task.Factory.StartNew(() =>
            {
                Program.MainChat.ChatBox.Items.Add(textData);
                Program.MainChat.ChatBox.SelectedIndex = Program.MainChat.ChatBox.Items.Count - 1;
                Program.MainChat.ChatBox.SelectedIndex = -1;
            });
        }
    }
}