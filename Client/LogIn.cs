using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public class LogIn : Form
    {
        private void InitializeComponent()
        {
            this.nameBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.ipTB = new System.Windows.Forms.TextBox();
            this.portTB = new System.Windows.Forms.TextBox();
            this.ipLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(21, 106);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(245, 22);
            this.nameBox.TabIndex = 0;
            this.nameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameBox_KeyDown);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(295, 106);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(122, 23);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(21, 83);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(45, 17);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Name";
            // 
            // ipTB
            // 
            this.ipTB.Location = new System.Drawing.Point(21, 40);
            this.ipTB.Name = "ipTB";
            this.ipTB.Size = new System.Drawing.Size(245, 22);
            this.ipTB.TabIndex = 3;
            this.ipTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameBox_KeyDown);
            // 
            // portTB
            // 
            this.portTB.Location = new System.Drawing.Point(295, 40);
            this.portTB.Name = "portTB";
            this.portTB.Size = new System.Drawing.Size(62, 22);
            this.portTB.TabIndex = 4;
            this.portTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameBox_KeyDown);
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(21, 17);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(20, 17);
            this.ipLabel.TabIndex = 5;
            this.ipLabel.Text = "IP";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(292, 17);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(34, 17);
            this.portLabel.TabIndex = 5;
            this.portLabel.Text = "Port";
            // 
            // LogIn
            // 
            this.ClientSize = new System.Drawing.Size(429, 153);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.portTB);
            this.Controls.Add(this.ipTB);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.nameBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private TextBox nameBox;
        private Button connectButton;
        private Label nameLabel;
        private TextBox ipTB;
        private TextBox portTB;
        private Label ipLabel;
        private Label portLabel;
        private readonly Chat _parrent;


        public LogIn(Chat sender)
        {
            InitializeComponent();
            _parrent = sender;
        }

        private void connectButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                ipTB.Text = ipTB.Text.TrimEnd(' ');
                portTB.Text = portTB.Text.TrimEnd(' ');
                var ipChecker =
                    new Regex(
                        @"((?<=\s)|^)((\d|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5])\.){3}(\d|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5])(\s|$)");
                if (!ipChecker.IsMatch(ipTB.Text))
                    throw new ArgumentException();

                if (!int.TryParse(portTB.Text, out var port) && (port <= 0 || port > 65535))
                    throw new ArgumentException();

                Task.Factory.StartNew(() => _parrent.Connect(nameBox.Text, ipTB.Text, portTB.Text));
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Wrong IP or port format.");
                return;
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Wrong IP or port. Unable to connect.");
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong.");
                _parrent.Close();
                return;
            }

            Close();
        }

        private void nameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                connectButton_Click(this, null);
            }
        }
    }
}