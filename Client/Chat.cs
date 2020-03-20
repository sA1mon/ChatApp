using System;
using System.ServiceModel;
using System.Windows.Forms;
using Client.ChatService;

namespace Client
{
    public partial class Chat : Form, IChatCallback
    {
        private ChatClient client;
        private User _me;
        public Chat()
        {
            InitializeComponent();
            var login = new LogIn(this);
            login.ShowDialog();
        }

        public void Connect(string name)
        {
            client = new ChatClient(new InstanceContext(this));
            _me = client.Add(name);
        }

        public void GetMessage(string message)
        {
            chatBox.Items.Add(message);
        }

        private async void Send(object sender, EventArgs e)
        {
            await client.SendMessageAsync(messageBox.Text, _me);
            messageBox.Text = string.Empty;
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
            {
                client.Remove(_me);
                client = null;
            }
        }
    }
}
