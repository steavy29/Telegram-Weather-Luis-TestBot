using System;
using System.Windows.Forms;

namespace TelegramTestBot
{
    public partial class BotForm : Form
    {
        public BotForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            BotThread.OnTextMessage += BotThreadOnTextMessage;
        }

        private void BotThreadOnTextMessage(object sender, string e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => InsertMessage(e)));
            }
            else
            {
                InsertMessage(e);
            }
        }

        private void InsertMessage(string message)
        {
            messagesListBox.Items.Add(message);
        }

        private async void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || string.IsNullOrWhiteSpace(sendMessageTextBox.Text))
                return;

            try
            {
                await BotThread.SendMessageToLastSender(sendMessageTextBox.Text);

                InsertMessage($"me: {sendMessageTextBox.Text}");
                sendMessageTextBox.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"{ex.Message}\nTry to write to bot from your Telegram account first.");
            }
        }
    }
}
