namespace Client
{
    partial class Chat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chatBox = new System.Windows.Forms.ListBox();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.sendB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chatBox
            // 
            this.chatBox.FormattingEnabled = true;
            this.chatBox.HorizontalScrollbar = true;
            this.chatBox.Location = new System.Drawing.Point(9, 10);
            this.chatBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chatBox.Name = "chatBox";
            this.chatBox.Size = new System.Drawing.Size(512, 407);
            this.chatBox.TabIndex = 0;
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(9, 436);
            this.messageBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(432, 20);
            this.messageBox.TabIndex = 1;
            this.messageBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.messageBox_KeyDown);
            // 
            // sendB
            // 
            this.sendB.Location = new System.Drawing.Point(454, 436);
            this.sendB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.sendB.Name = "sendB";
            this.sendB.Size = new System.Drawing.Size(67, 19);
            this.sendB.TabIndex = 2;
            this.sendB.Text = "Send";
            this.sendB.UseVisualStyleBackColor = true;
            this.sendB.Click += new System.EventHandler(this.Send);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 478);
            this.Controls.Add(this.sendB);
            this.Controls.Add(this.messageBox);
            this.Controls.Add(this.chatBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "Chat";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Chat_FormClosing);
            this.Shown += new System.EventHandler(this.Chat_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox chatBox;
        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.Button sendB;
    }
}

