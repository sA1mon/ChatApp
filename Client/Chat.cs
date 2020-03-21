using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using ChatApp;
using Client.ChatService;

namespace Client
{
    public partial class Chat : Form, IChatCallback
    {
        private ChatClient _client;
        private User _me;

        private event EventHandler UserConnected;
        private event EventHandler UserDisconnected;

        public Chat()
        {
            InitializeComponent();
            var login = new LogIn(this);
            login.ShowDialog();
        }

        public void Connect(string name)
        {
            Thread.Sleep(100);
            _client = new ChatClient(new InstanceContext(this));
            _me = _client.Add(name);
            Text += $": {name}";
            //UserConnected?.Invoke(this, EventArgs.Empty);
        }

        public void GetMessage(string message)
        {
            chatBox.Items.Add(message);
        }

        public void GetHistory(Queue<string> messages)
        {
            chatBox.Items.Clear();

            while (messages.Count > 0)
            {
                chatBox.Items.Add(messages.Dequeue());
            }
        }

        private async void Send(object sender, EventArgs e)
        {
            await _client.SendMessageAsync(messageBox.Text, _me);
            messageBox.Text = string.Empty;
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_client != null)
            {
                _client.Remove(_me);
                //UserDisconnected?.Invoke(this, EventArgs.Empty);
                _client.Close();
            }
        }

        private void messageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Send(this, null);
            }
        }
    }
}
