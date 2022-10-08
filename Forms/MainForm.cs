using ShiverBot.IO;
using ShiverBot.Network;
using ShiverBot.Thunder;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ShiverBot.Forms
{
    public partial class MainForm : Form
    {
        private readonly ConnectionManager _connectionManager;
        private readonly SavedBuildReader _savedBuildReader;
        private SavedBuild? gameBuild;

        private bool readyForUserInput = false;
        private bool advancedWarningShown = false;
        private int autoUpdateCount = 10;

        private long[] chunkAddresses;
        private long[] foodTicketAddresses;
        private long[] drinkTicketAddresses;
        private bool ismFlag = false;

        public MainForm()
        {
            InitializeComponent();
            _connectionManager = new();
            _savedBuildReader = new();
            chunkAddresses = new long[14];
            foodTicketAddresses = new long[6];
            drinkTicketAddresses = new long[14];

            statusLabel.Text = "unconnected";

            Task.Factory.StartNew(UpdateThread);
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
                    MessageBox.Show("error: address peeking returned an invalid amount of money. please delete all AMS cheat files, restart the game and try again.");
                    moneyNumUpDown.Value = 0;
                }
                else
                {
                    moneyNumUpDown.Value = money;
                    moneyNumUpDown.Enabled = true;
                }
            }
        }

        private void ShowChunks()
        {
            if (gameBuild == null)
            {
                MessageBox.Show("error: no addresses for this version loaded.");
                return;
            }

            readyForUserInput = false;

            long address = Convert.ToInt64(gameBuild.ChunkBaseAddress, 16);
            byte[]? chunkData = _connectionManager.PeekAddress(address, 768);
            if (chunkData == null)
            {
                MessageBox.Show("error: failed to retrieve ability chunks data. please connect your switch to the internet and try again.");
                return;
            }

            for (int i = 0; i < 16; i++)
            {
                int pointer = i * 0x30;
                int abilityId = BitConverter.ToInt32(chunkData.AsSpan()[pointer..(pointer + 4)]);

                if (abilityId > -1 && abilityId < 14)
                {
                    GearAbility ability = (GearAbility)abilityId;

                    int chunkAmount = BitConverter.ToInt32(chunkData.AsSpan()[(pointer + 4)..(pointer + 8)]);
                    if (abilityId == 0 && chunkAmount == 0 && ismFlag)
                    {
                        continue;
                    }
                    else if (abilityId == 0 && chunkAmount > 0)
                    {
                        ismFlag = true;
                    }

                    if (chunkAmount < 0 || chunkAmount > 999)
                    {
                        MessageBox.Show($"error: retrieved an invalid amount of chunks for ability {abilityId} ({ability}). please delete all AMS cheat files, restart the game and try again.");
                        return;
                    }
                    
                    NumericUpDown chunkNumUpDown = (NumericUpDown)chunksTabPage.Controls[$"chunk{abilityId}NumUpDown"];
                    chunkNumUpDown.Value = chunkAmount;
                    chunkNumUpDown.Enabled = true;
                    chunkNumUpDown.Tag = ability;
                    chunkAddresses[abilityId] = address + pointer + 4;
                }
            }

            readyForUserInput = true;
        }

        private void ShowFoodAndDrinkTickets()
        {
            if (gameBuild == null)
            {
                MessageBox.Show("error: no addresses for this version loaded.");
                return;
            }

            readyForUserInput = false;

            // food
            long foodTicketAddress = Convert.ToInt64(gameBuild.FoodTicketBase, 16);
            byte[]? foodTicketData = _connectionManager.PeekAddress(foodTicketAddress, 288);
            if (foodTicketData == null)
            {
                MessageBox.Show("error: failed to retrieve food tickets data. please connect your switch to the internet and try again.");
                return;
            }

            for (int i = 0; i < 6; i++)
            {
                int pointer = i * 0x30;
                int foodTicketId = BitConverter.ToInt32(foodTicketData.AsSpan()[pointer..(pointer + 4)]);

                if (foodTicketId > 0 && foodTicketId < 7)
                {
                    FoodTicket foodTicket = (FoodTicket)foodTicketId;

                    int foodTicketAmount = BitConverter.ToInt32(foodTicketData.AsSpan()[(pointer + 4)..(pointer + 8)]);
                    if (foodTicketAmount < 0 || foodTicketAmount > 99)
                    {
                        MessageBox.Show($"error: retrieved an invalid amount of food tickets {foodTicketId} ({foodTicket}). please delete all AMS cheat files, restart the game and try again.");
                        return;
                    }

                    NumericUpDown foodNumUpDown = (NumericUpDown)foodTicketsTabPage.Controls[$"food{foodTicketId}NumUpDown"];
                    foodNumUpDown.Value = foodTicketAmount;
                    foodNumUpDown.Enabled = true;
                    foodNumUpDown.Tag = foodTicket;
                    foodTicketAddresses[foodTicketId - 1] = foodTicketAddress + pointer + 4;
                }
            }

            // drink
            long drinkTicketAddress = Convert.ToInt64(gameBuild.DrinkTicketBase, 16);
            byte[]? drinkTicketData = _connectionManager.PeekAddress(drinkTicketAddress, 672);
            if (drinkTicketData == null)
            {
                MessageBox.Show("error: failed to retrieve food tickets data. please connect your switch to the internet and try again.");
                return;
            }

            for (int i = 0; i < 14; i++)
            {
                int pointer = i * 0x30;
                int drinkTicketId = BitConverter.ToInt32(drinkTicketData.AsSpan()[pointer..(pointer + 4)]);

                if (drinkTicketId > -1 && drinkTicketId < 14)
                {
                    GearAbility drinkTicket = (GearAbility)drinkTicketId;

                    int drinkTicketAmount = BitConverter.ToInt32(drinkTicketData.AsSpan()[(pointer + 4)..(pointer + 8)]);
                    if (drinkTicketAmount < 0 || drinkTicketAmount > 99)
                    {
                        MessageBox.Show($"error: retrieved an invalid amount of drink tickets {drinkTicketId} ({drinkTicket}). please delete all AMS cheat files, restart the game and try again.");
                        return;
                    }

                    NumericUpDown drinkNumUpDown = (NumericUpDown)drinkTicketsTabPage.Controls[$"drink{drinkTicketId}NumUpDown"];
                    drinkNumUpDown.Value = drinkTicketAmount;
                    drinkNumUpDown.Enabled = true;
                    drinkNumUpDown.Tag = drinkTicket;
                    drinkTicketAddresses[drinkTicketId] = drinkTicketAddress + pointer + 4;
                }
            }

            readyForUserInput = true;
        }

        private void ShowTableTurfData()
        {

        }

        private void TryDisconnect()
        {
            _connectionManager.TryDisconnect();
            readyForUserInput = false;
            connectButton.Enabled = true;
            connectButton.Text = "Connect";
            statusLabel.Text = "unconnected";

            for (int i = 0; i < 14; i++)
            {
                NumericUpDown chunkNumUpDown = (NumericUpDown)chunksTabPage.Controls[$"chunk{i}NumUpDown"];
                chunkNumUpDown.Enabled = false;
                chunkNumUpDown.Value = 0;
            }
            moneyNumUpDown.Enabled = false;
            moneyNumUpDown.Value = 0;
        }

        private void UpdateThread()
        {
            while (true)
            {
                if (autoUpdateCheckbox.Checked && _connectionManager.IsSwitchConnected)
                {
                    Invoke(() =>
                    {
                        if (!updatingLabel.Visible)
                        {
                            updatingLabel.Visible = true;
                        }

                        if (autoUpdateCount <= 0)
                        {
                            updatingLabel.Text = "updating...";
                            ShowMoney();
                            ShowChunks();
                            ShowFoodAndDrinkTickets();
                            ShowTableTurfData();
                            autoUpdateCount = 10;
                        }
                        updatingLabel.Text = $"updating in {autoUpdateCount}s...";
                    });
                }
                else
                {
                    if (autoUpdateCount < 0)
                    {
                        autoUpdateCount = 10;
                    }

                    Invoke(() =>
                    {
                        updatingLabel.Visible = false;
                    });
                }

                autoUpdateCount--;
                Thread.Sleep(999);
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (_connectionManager.IsSwitchConnected)
            {
                TryDisconnect();
                return;
            }

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
                    MessageBox.Show($"error: the game is not Splatoon 3.\nReceived title id is {titleId}");
                    connectButton.Enabled = true;
                    connectButton.Text = "Connect";
                    statusLabel.Text = "unconnected";
                    return;
                }

                string buildId = _connectionManager.GetBuildId()[..16];
                SavedBuild? build = _savedBuildReader.GetBuild(buildId);
                if (build == null)
                {
                    MessageBox.Show($"error: no addresses found for this build id.\nReceived build id is {buildId}");
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
            ShowFoodAndDrinkTickets();
            ShowTableTurfData();
            readyForUserInput = true;
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
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

        private void readMainButton_Click(object sender, EventArgs e)
        {
            if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            byte[]? data = _connectionManager.PeekMainAddress(addressTextBox.Text, (int)bytesToReadNumUpDown.Value);

            if (data != null)
            {
                if (saveToFileCheckBox.Checked)
                {
                    using SaveFileDialog sfd = new();
                    sfd.FileName = "dataMain.bin";
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
            else if (!advancedWarningShown)
            {
                if (MessageBox.Show("BEWARE: editing anything that isn't the tool's easy editing features increases your chances of getting banned.", "warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
                advancedWarningShown = true;
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

        private void chunk0NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk0NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[0], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk1NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk1NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[1], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk2NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk2NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[2], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk3NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk3NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[3], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk4NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk4NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[4], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk5NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk5NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[5], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk6NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk6NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[6], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk7NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk7NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[7], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk8NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk8NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[8], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk9NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk9NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[9], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk10NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk10NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[10], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk11NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk11NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[11], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk12NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk12NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[12], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void chunk13NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)chunk13NumUpDown.Value;
            _connectionManager.PokeAddress(chunkAddresses[13], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void autoUpdateCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (_connectionManager.IsSwitchConnected)
            {
                updatingLabel.Visible = autoUpdateCheckbox.Checked;
            }
        }

        private void creatorLabel_Click(object sender, EventArgs e)
        {
            OpenUrl("http://github.com/supremetakoyaki");
        }

        private void creatorLabel2_Click(object sender, EventArgs e)
        {
            OpenUrl("http://github.com/supremetakoyaki");
        }

        private void food1NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)food1NumUpDown.Value;
            _connectionManager.PokeAddress(foodTicketAddresses[0], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void food2NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)food2NumUpDown.Value;
            _connectionManager.PokeAddress(foodTicketAddresses[1], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void food3NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)food3NumUpDown.Value;
            _connectionManager.PokeAddress(foodTicketAddresses[2], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void food4NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)food4NumUpDown.Value;
            _connectionManager.PokeAddress(foodTicketAddresses[3], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void food5NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)food5NumUpDown.Value;
            _connectionManager.PokeAddress(foodTicketAddresses[4], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void food6NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)food6NumUpDown.Value;
            _connectionManager.PokeAddress(foodTicketAddresses[5], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink0NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink0NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[0], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink1NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink1NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[1], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink2NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink2NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[2], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink3NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink3NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[3], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink4NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink4NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[4], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink5NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink5NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[5], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink6NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink6NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[6], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink7NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink7NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[7], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink8NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink8NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[8], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink9NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink9NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[9], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink10NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink10NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[10], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink11NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink11NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[11], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink12NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink12NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[12], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void drink13NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!readyForUserInput)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            uint amount = (uint)drink13NumUpDown.Value;
            _connectionManager.PokeAddress(drinkTicketAddresses[13], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }
    }
}