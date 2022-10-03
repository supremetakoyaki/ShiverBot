using ShiverBot.Network;
using System.Text;

namespace ShiverBot.Forms
{
    public partial class MainForm : Form
    {
        private readonly ConnectionManager _connectionManager;

        public MainForm()
        {
            InitializeComponent();
            _connectionManager = new();

            statusLabel.Text = "unconnected";
        }

        private void ShowMoney()
        {
            byte[]? moneyData = _connectionManager.PeekAddress("BDFB12E4", 4);
            if (moneyData != null)
            {
                int money = BitConverter.ToInt32(moneyData);
                if (money < 0 || money > 9999999)
                {
                    MessageBox.Show("error: you have an illegal amount of money in-game.");
                    moneyNumUpDown.Value = 0;
                }
                else
                {
                    moneyNumUpDown.Value = money;
                }
            }
        }

        private void ShowChunks()
        {

        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            connectButton.Enabled = false;
            connectButton.Text = "Connecting...";

            if (!_connectionManager.TryConnect(ipTextBox.Text, 6000, out byte responseCode))
            {
                MessageBox.Show($"error: {responseCode}");
                connectButton.Enabled = true;
                connectButton.Text = "Connect";
                statusLabel.Text = "unconnected";
                return;
            }
            else
            {
                string titleId = _connectionManager.GetTitleId();
                if (!titleId.ToUpper().Trim().StartsWith("0100C2500FC20000"))
                {
                    MessageBox.Show($"error: the game is not Splatoon 3.\nTitle id received is {titleId.ToUpper()}");
                    connectButton.Enabled = true;
                    connectButton.Text = "Connect";
                    statusLabel.Text = "unconnected";
                    return;
                }
            }

            connectButton.Enabled = true;
            connectButton.Text = "Disconnect";
            statusLabel.Text = "connected";
            ShowMoney();
            ShowChunks();
        }

        private void readButton_Click(object sender, EventArgs e)
        {
            if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            byte[]? data = _connectionManager.PeekAddress(addressTextBox.Text, (int)bytesToReadNumUpDown.Value);

            if (data != null)
            {
                if (saveToFileCheckBox.Checked)
                {
                    using SaveFileDialog sfd = new();
                    sfd.FileName = "data.bin";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfd.FileName, data);
                    }
                }
                else if (bytesToReadNumUpDown.Value > 128)
                {
                    MessageBox.Show("There are too many bytes to display! Please use the save file method for this.");
                }
                else
                {
                    StringBuilder sb = new();
                    foreach (byte b in data)
                    {
                        sb.Append($"{b:x2} ");
                    }

                    MessageBox.Show(sb.ToString());
                }
            }
        }

        private void writeButton_Click(object sender, EventArgs e)
        {
            if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            _connectionManager.PokeAddress(pokeAddressTextBox.Text, pokeDataTextBox.Text);
        }

        private void saveMoneyButton_Click(object sender, EventArgs e)
        {
            if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            int amountToSave = (int)moneyNumUpDown.Value;
            byte[] moneyBytes = BitConverter.GetBytes(amountToSave);
            _connectionManager.PokeAddress("BDFB12E4", Convert.ToHexString(moneyBytes, 0, 4));
        }

        private void restoreMoneyButton_Click(object sender, EventArgs e)
        {
            if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            ShowMoney();
        }
    }
}