using Client.ChatService;
using System;
using System.IO;
using System.Management;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Chat : Form
    {
        private User Me { get; set; }
        private Rsa.Rsa _rsa;
        private ChatClient ChatClient { get; set; }
        public ListBox ChatBox => chatBox;
        public Rsa.Rsa Rsa => _rsa;

        public Chat()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            _rsa = new Rsa.Rsa();
            ShowLoginForm();
            
        }

        internal void Connect(string name, string ip, string port)
        {
            try
            {
                Thread.Sleep(150);
                ChatClient = new ChatClient(new InstanceContext(new Callback()),
                    new NetHttpBinding
                    {
                        CloseTimeout = new TimeSpan(1, 0, 0),
                        OpenTimeout = new TimeSpan(1, 0, 0),
                        ReceiveTimeout = new TimeSpan(1, 0, 0),
                        SendTimeout = new TimeSpan(1, 0, 0)
                    },
                    new EndpointAddress($"http://{ip}:{port}/"));

                Me = ChatClient.Add(name, GetDriveSerial(), _rsa.Key);
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
            login.Show(this);
        }

        private async void Send(object sender, EventArgs e)
        {
            var data = Encoding.UTF32.GetBytes(messageBox.Text);
            messageBox.Text = string.Empty;

            try
            {
                foreach (var user in await ChatClient.GetUsersAsync())
                {
                    var encrypted = _rsa.Crypt(data, user.Key);
                    await ChatClient.SendMessageAsync(encrypted, Me, user);
                }
            }
            catch
            {
                MessageBox.Show("Произшла ошибка! Проверьте соединение с Интернетом", 
                    "Ошибка", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                Close();
            }
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

        private void Chat_Shown(object sender, EventArgs e)
        {
            Hide();
        }
    }
}