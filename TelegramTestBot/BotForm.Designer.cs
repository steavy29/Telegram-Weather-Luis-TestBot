namespace TelegramTestBot
{
    partial class BotForm
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
            this.messagesListBox = new System.Windows.Forms.ListBox();
            this.sendMessageTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // messagesListBox
            // 
            this.messagesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messagesListBox.FormattingEnabled = true;
            this.messagesListBox.Location = new System.Drawing.Point(12, 12);
            this.messagesListBox.Name = "messagesListBox";
            this.messagesListBox.Size = new System.Drawing.Size(281, 316);
            this.messagesListBox.TabIndex = 0;
            // 
            // sendMessageTextBox
            // 
            this.sendMessageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sendMessageTextBox.Location = new System.Drawing.Point(13, 338);
            this.sendMessageTextBox.Name = "sendMessageTextBox";
            this.sendMessageTextBox.Size = new System.Drawing.Size(280, 20);
            this.sendMessageTextBox.TabIndex = 1;
            this.sendMessageTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxKeyDown);
            // 
            // BotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 368);
            this.Controls.Add(this.sendMessageTextBox);
            this.Controls.Add(this.messagesListBox);
            this.Name = "BotForm";
            this.Text = "Bot Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox messagesListBox;
        private System.Windows.Forms.TextBox sendMessageTextBox;
    }
}

