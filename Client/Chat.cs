using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatApp;
using Client.ChatService;

namespace Client
{
    public partial class Chat : Form, IChatCallback
    {
        public User Me { get; set; }
        public ChatClient ChatClient { get;set; }

        public Chat()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            ShowLoginForm();
        }

        private void ShowLoginForm()
        {
            var login = new LogIn(this);
            login.ShowDialog();
        }

        public async void GetMessage(string message)
        {
            await Task.Factory.StartNew(() => chatBox.Items.Add(message));
        }

        public async void GetHistory(Queue<string> messages)
        {
            await Task.Factory.StartNew(() =>
            {
                while (messages.Count > 0)
                {
                    chatBox.Items.Add(messages.Dequeue());
                }
            });
        }

        private async void Send(object sender, EventArgs e)
        {
            await ChatClient.SendMessageAsync(messageBox.Text, Me);
            messageBox.Text = string.Empty;
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ChatClient != null)
                {
                    ChatClient.Remove(Me);
                    ChatClient.Close();
                }
            }
            catch
            {
                //do nothing
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
