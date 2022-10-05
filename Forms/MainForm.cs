using ShiverBot.IO;
using ShiverBot.Network;
using System.Text;

namespace ShiverBot.Forms
{
    public partial class MainForm : Form
    {
        private readonly ConnectionManager _connectionManager;
        private readonly SavedBuildReader _savedBuildReader;
        private SavedBuild? gameBuild;

        public MainForm()
        {
            InitializeComponent();
            _connectionManager = new();
            _savedBuildReader = new();

            statusLabel.Text = "unconnected";
        }

        private void ShowMoney()
        {
            if (gameBuild == null)
            {
                MessageBox.Show("error: no addresses for this version loaded.");
                return;
            }

            byte[]? moneyData = _connectionManager.PeekAddress(gameBuild.MoneyAddress, 4);
            if (moneyData != null)
            {
                int money = BitConverter.ToInt32(moneyData);
                if (money < 0 || money > 9999999)
                {
                    MessageBox.Show("error: address peeking returned an invalid amount of money. please delete all AMS cheat files and restart the game.");
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
                string titleId = _connectionManager.GetTitleId()[..16];
                if (titleId != "0100C2500FC20000")
                {
                    MessageBox.Show($"error: the game is not Splatoon 3.\nTitle id received is {titleId}");
                    connectButton.Enabled = true;
                    connectButton.Text = "Connect";
                    statusLabel.Text = "unconnected";
                    return;
                }

                string buildId = _connectionManager.GetBuildId()[..16];
                SavedBuild? build = _savedBuildReader.GetBuild(buildId);
                if (build == null)
                {
                    MessageBox.Show($"error: no addresses found for this build id. Build id received is {buildId}");
                    connectButton.Enabled = true;
                    connectButton.Text = "Connect";
                    statusLabel.Text = "unconnected";
                    return;
                }
                gameBuild = build;
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
            else if (gameBuild == null)
            {
                MessageBox.Show("error: no addresses for this version loaded.");
                return;
            }

            int amountToSave = (int)moneyNumUpDown.Value;
            byte[] moneyBytes = BitConverter.GetBytes(amountToSave);
            _connectionManager.PokeAddress(gameBuild.MoneyAddress, Convert.ToHexString(moneyBytes, 0, 4));
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