using Client.ChatService;
using System;
using System.IO;
using System.Management;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Chat : Form
    {
        private User Me { get; set; }
        private ChatClient ChatClient { get; set; }
        public ListBox ChatBox => chatBox;

        public Chat()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            ShowLoginForm();
        }

        internal void Connect(string name, string ip, string port)
        {
            try
            {
                Thread.Sleep(50);
                ChatClient = new ChatClient(new InstanceContext(new Callback()),
                    new NetHttpBinding
                    {
                        CloseTimeout = new TimeSpan(1, 0, 0),
                        OpenTimeout = new TimeSpan(1, 0, 0),
                        ReceiveTimeout = new TimeSpan(1, 0, 0),
                        SendTimeout = new TimeSpan(1, 0, 0)
                    },
                    new EndpointAddress($"http://{ip}:{port}/"));

                Me = ChatClient.Add(name, GetDriveSerial());
                if (Me == null)
                    throw new NullReferenceException();

                Text += $": {name}";
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Wrong IP or Port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You've been banned from this server", "Oops", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Close();
            }
        }

        private static string GetDriveSerial()
        {
            var objectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (var info in objectSearcher.Get())
            {
                return info["SerialNumber"].ToString();
            }

            throw new DriveNotFoundException();
        }

        private void ShowLoginForm()
        {
            var login = new LogIn(this);
            login.ShowDialog();
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
                if (ChatClient == null) return;

                ChatClient.Remove(Me);
                ChatClient.Close();
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