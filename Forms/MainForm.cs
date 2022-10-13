using ShiverBot.IO;
using ShiverBot.Network;
using ShiverBot.Properties;
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

        private readonly long[] _chunkAddresses;
        private readonly long[] _foodTicketAddresses;
        private readonly long[] _drinkTicketAddresses;
        private long tableTurfSpecialAddress;
        private long tableTurfPointAddress;
        private bool ismFlag = false;

        public MainForm()
        {
            InitializeComponent();
            _connectionManager = new();
            _savedBuildReader = new();
            _chunkAddresses = new long[14];
            _foodTicketAddresses = new long[6];
            _drinkTicketAddresses = new long[14];
            tableTurfSpecialAddress = 0;
            tableTurfPointAddress = 0;

            ipTextBox.Text = Settings.Default.ipAddress;
            statusLabel.Text = "unconnected";
            gearTypeComboBox.SelectedIndex = 0;
            gearMAComboBox.SelectedIndex = 0;
            gearS1ComboBox.SelectedIndex = 0;
            gearS2ComboBox.SelectedIndex = 0;
            gearS3ComboBox.SelectedIndex = 0;

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

                    if (chunkAmount < 0 || chunkAmount > 1000)
                    {
                        MessageBox.Show($"error: retrieved an invalid amount of chunks for ability {abilityId} ({ability}). please delete all AMS cheat files, restart the game and try again.");
                        return;
                    }

                    NumericUpDown chunkNumUpDown = (NumericUpDown)chunksTabPage.Controls[$"chunk{abilityId}NumUpDown"];
                    chunkNumUpDown.Value = chunkAmount;
                    chunkNumUpDown.Enabled = true;
                    chunkNumUpDown.Tag = ability;
                    _chunkAddresses[abilityId] = address + pointer + 4;
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
                    _foodTicketAddresses[foodTicketId - 1] = foodTicketAddress + pointer + 4;
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
                    _drinkTicketAddresses[drinkTicketId] = drinkTicketAddress + pointer + 4;
                }
            }

            readyForUserInput = true;
        }

        private void ShowTableTurfData()
        {
            if (gameBuild == null)
            {
                MessageBox.Show("error: no addresses for this version loaded.");
                return;
            }

            byte[]? rankData = _connectionManager.PeekAddress(gameBuild.TableTurfRankBase, 4);
            if (rankData != null)
            {
                int expPts = BitConverter.ToInt32(rankData);
                tableTurfExpLabel.Text = $"Rank: {TableTurfLevelTable.GetLevel(expPts)} ({expPts} EXP)";
            }

            ShowTableTurfMatch();
        }

        private void ShowTableTurfMatch()
        {
            if (tableTurfSpecialAddress == 0 || tableTurfPointAddress == 0)
            {
                SetTableTurfControlsStatus(false);
                return;
            }

            int mySpecialPoints = BitConverter.ToInt32(_connectionManager.PeekAbsoluteAddress(tableTurfSpecialAddress, 4));
            int cpuSpecialPoints = BitConverter.ToInt32(_connectionManager.PeekAbsoluteAddress(tableTurfSpecialAddress + 4, 4));

            if (mySpecialPoints < 0 || mySpecialPoints > 10 || cpuSpecialPoints < 0 || cpuSpecialPoints > 10)
            {
                SetTableTurfControlsStatus(false);
                return;
            }

            int myTurfPoints = BitConverter.ToInt32(_connectionManager.PeekAbsoluteAddress(tableTurfPointAddress, 4));
            int cpuTurfPoints = BitConverter.ToInt32(_connectionManager.PeekAbsoluteAddress(tableTurfPointAddress + 4, 4));

            if (myTurfPoints < 0 || myTurfPoints > 999 || cpuTurfPoints < 0 || cpuTurfPoints > 999)
            {
                SetTableTurfControlsStatus(false);
                return;
            }

            readyForUserInput = false;
            tableTurfSpecialNumUpDown.Value = mySpecialPoints;
            tableTurfPointsNumUpDown.Value = myTurfPoints;
            tableTurfCpuSpecialNumUpDown.Value = cpuSpecialPoints;
            tableTurfCpuPointsNumUpDown.Value = cpuTurfPoints;
            SetTableTurfControlsStatus(true);
            readyForUserInput = true;
        }

        private void SetGearSeedFinderBoxesStatus(bool flag)
        {
            gearTypeComboBox.Enabled = flag;
            gearidkExpCheckbox.Enabled = flag;
            if (flag && gearidkExpCheckbox.Checked)
            {
                gearExpNumUpDown.Enabled = false;
            }
            else
            {
                gearExpNumUpDown.Enabled = flag;
            }
            gearStarsNumUpDown.Enabled = flag;
            gearMAComboBox.Enabled = flag;
            gearS1ComboBox.Enabled = flag;
            gearS2ComboBox.Enabled = flag;
            gearS3ComboBox.Enabled = flag;
            gearSearchButton.Enabled = flag;
        }

        private void SetTableTurfControlsStatus(bool flag)
        {
            if (!flag)
            {
                tableTurfPointAddress = 0;
                tableTurfSpecialAddress = 0;
                tableTurfPointLockCheckbox.Checked = false;
                tableTurfSpecialLockCheckbox.Checked = false;
                tableTurfCpuPointLockCheckbox.Checked = false;
                tableTurfCpuSpecialLockCheckbox.Checked = false;
                _connectionManager.UnfreezeAllAddresses();
            }

            tableTurfPointsNumUpDown.Enabled = flag;
            tableTurfSpecialNumUpDown.Enabled = flag;
            tableTurfCpuPointsNumUpDown.Enabled = flag;
            tableTurfCpuSpecialNumUpDown.Enabled = flag;
            tableTurfPointLockCheckbox.Enabled = flag;
            tableTurfSpecialLockCheckbox.Enabled = flag;
            tableTurfCpuPointLockCheckbox.Enabled = flag;
            tableTurfCpuSpecialLockCheckbox.Enabled = flag;
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
            SetGearSeedFinderBoxesStatus(false);
            SetTableTurfControlsStatus(false);
        }

        private void UpdateThread()
        {
            while (true)
            {
                try
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

                        if (InvokeRequired)
                        {
                            Invoke(() =>
                            {
                                updatingLabel.Visible = false;
                            });
                        }
                        else
                        {
                            updatingLabel.Visible = false;
                        }

                    }

                    autoUpdateCount--;
                }
                catch
                {

                }
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
                if (responseCode == 0)
                {
                    MessageBox.Show($"error: failed to connect to the IP address.");
                }
                else
                {
                    MessageBox.Show($"error: code {responseCode}");
                }
                connectButton.Enabled = true;
                connectButton.Text = "Connect";
                statusLabel.Text = "unconnected";
                return;
            }
            else
            {
                string titleId = _connectionManager.SendCommandAsIs("getTitleID", 33)[..16];
                if (titleId != "0100C2500FC20000")
                {
                    MessageBox.Show($"error: the game is not Splatoon 3.\nReceived title id is {titleId}");
                    connectButton.Enabled = true;
                    connectButton.Text = "Connect";
                    statusLabel.Text = "unconnected";
                    return;
                }

                string buildId = _connectionManager.SendCommandAsIs("getBuildID", 33)[..16];
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
            SetGearSeedFinderBoxesStatus(true);
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

            int bytesToRead = (int)bytesToReadNumUpDown.Value;
            if (bytesToRead > 128 && !saveToFileCheckBox.Checked)
            {
                MessageBox.Show("There are too many bytes to display! Please use the save file method for this.");
                return;
            }

            if (bytesToRead < 4194304)
            {
                byte[]? data = _connectionManager.PeekAddress(addressTextBox.Text, (int)bytesToReadNumUpDown.Value);
                if (data == null)
                {
                    return;
                }

                if (saveToFileCheckBox.Checked)
                {
                    using SaveFileDialog sfd = new();
                    sfd.FileName = $"HEAP+{addressTextBox.Text} ({bytesToRead}).bin";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfd.FileName, data);
                    }
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
            else if (bytesToRead > 4194304 && MessageBox.Show("This might take several minutes. Are you sure you want to proceed?", "prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                using SaveFileDialog sfd = new();
                sfd.FileName = $"HEAP+{addressTextBox.Text} ({bytesToRead}).bin";
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                ChonkReader.GetData(addressTextBox.Text, bytesToRead, 0, _connectionManager, sfd.FileName);
                MessageBox.Show("finished~!");
            }
        }

        private void readMainButton_Click(object sender, EventArgs e)
        {
            if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            int bytesToRead = (int)bytesToReadNumUpDown.Value;
            if (bytesToRead > 128 && !saveToFileCheckBox.Checked)
            {
                MessageBox.Show("There are too many bytes to display! Please use the save file method for this.");
                return;
            }

            if (bytesToRead < 4194304)
            {
                byte[]? data = _connectionManager.PeekMainAddress(addressTextBox.Text, (int)bytesToReadNumUpDown.Value);
                if (data == null)
                {
                    return;
                }

                if (saveToFileCheckBox.Checked)
                {
                    using SaveFileDialog sfd = new();
                    sfd.FileName = $"MAIN+{addressTextBox.Text} ({bytesToRead}).bin";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfd.FileName, data);
                    }
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
            else if (bytesToRead > 4194304 && MessageBox.Show("This might take several minutes. Are you sure you want to proceed?", "prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                using SaveFileDialog sfd = new();
                sfd.FileName = $"MAIN+{addressTextBox.Text} ({bytesToRead}).bin";
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                ChonkReader.GetData(addressTextBox.Text, bytesToRead, 1, _connectionManager, sfd.FileName);
                MessageBox.Show("finished~!");
            }
        }

        private void readAbsButton_Click(object sender, EventArgs e)
        {
            if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            int bytesToRead = (int)bytesToReadNumUpDown.Value;
            if (bytesToRead > 128 && !saveToFileCheckBox.Checked)
            {
                MessageBox.Show("There are too many bytes to display! Please use the save file method for this.");
                return;
            }

            if (bytesToRead < 4194304)
            {
                byte[]? data = _connectionManager.PeekAbsoluteAddress(addressTextBox.Text, (int)bytesToReadNumUpDown.Value);
                if (data == null)
                {
                    return;
                }

                if (saveToFileCheckBox.Checked)
                {
                    using SaveFileDialog sfd = new();
                    sfd.FileName = $"0+{addressTextBox.Text} ({bytesToRead}).bin";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfd.FileName, data);
                    }
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
            else if (bytesToRead > 4194304 && MessageBox.Show("This might take several minutes. Are you sure you want to proceed?", "prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                using SaveFileDialog sfd = new();
                sfd.FileName = $"0+{addressTextBox.Text} ({bytesToRead}).bin";
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                ChonkReader.GetData(addressTextBox.Text, bytesToRead, 2, _connectionManager, sfd.FileName);
                MessageBox.Show("finished~!");
            }
        }

        private void metaDataButton_Click(object sender, EventArgs e)
        {
            if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            string titleId = _connectionManager.SendCommandAsIs("getTitleID", 33)[..16];
            string buildId = _connectionManager.SendCommandAsIs("getBuildID", 33)[..16];
            string languageCode = _connectionManager.SendCommandAsIs("getSystemLanguage", 4)[..1];
            string mainAddress = _connectionManager.SendCommandAsIs("getMainNsoBase", 33)[..16];
            string heapAddress = _connectionManager.SendCommandAsIs("getHeapBase", 33)[..16];

            StringBuilder sb = new();
            sb.AppendLine($"Title ID: {titleId}");
            sb.AppendLine($"Build ID: {buildId}");
            sb.AppendLine($"Language: {languageCode}");
            sb.AppendLine($"Main Address: 0x{mainAddress}");
            sb.AppendLine($"Heap Address: 0x{heapAddress}");
            sb.AppendLine();
            sb.AppendLine("(to copy, CTRL+C while in this dialog)");
            MessageBox.Show(sb.ToString(), "metadata");
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
            _connectionManager.PokeAddress(_chunkAddresses[0], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[1], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[2], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[3], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[4], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[5], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[6], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[7], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[8], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[9], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[10], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[11], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[12], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_chunkAddresses[13], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_foodTicketAddresses[0], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_foodTicketAddresses[1], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_foodTicketAddresses[2], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_foodTicketAddresses[3], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_foodTicketAddresses[4], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_foodTicketAddresses[5], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[0], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[1], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[2], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[3], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[4], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[5], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[6], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[7], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[8], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[9], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[10], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[11], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[12], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
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
            _connectionManager.PokeAddress(_drinkTicketAddresses[13], $"{(amount & 0x000000FF) << 24 | (amount & 0x0000FF00) << 8 | (amount & 0x00FF0000) >> 8 | (amount & 0xFF000000) >> 24:X8}");
        }

        private void gearidkExpCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            gearExpNumUpDown.Enabled = !gearidkExpCheckbox.Checked;
        }

        private void gearSearchButton_Click(object sender, EventArgs e)
        {
            if (!readyForUserInput || gameBuild == null)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            long baseAddress = Convert.ToInt64(gameBuild.GearBase, 16);
            baseAddress += gearTypeComboBox.SelectedIndex * 0x2C020;

            int maxGearAmount = 1023;
            int gearRamSize = 176;
            byte[]? searchData = _connectionManager.PeekAddress(baseAddress, maxGearAmount * gearRamSize);
            if (searchData == null)
            {
                MessageBox.Show("error: failed to search");
                return;
            }

            List<(long, uint)> results = new();

            int pointer = 0;
            for (int i = 0; i < maxGearAmount; i++)
            {
                int gearExp = BitConverter.ToInt32(searchData.AsSpan()[pointer..(pointer + 4)]);
                int stars = BitConverter.ToInt32(searchData.AsSpan()[(pointer + 8)..(pointer + 12)]);
                int mainAbility = BitConverter.ToInt32(searchData.AsSpan()[(pointer + 12)..(pointer + 16)]) - 1;
                int sub1 = BitConverter.ToInt32(searchData.AsSpan()[(pointer + 36)..(pointer + 40)]);
                int sub2 = BitConverter.ToInt32(searchData.AsSpan()[(pointer + 40)..(pointer + 44)]);
                int sub3 = BitConverter.ToInt32(searchData.AsSpan()[(pointer + 44)..(pointer + 48)]);
                uint gearSeed = BitConverter.ToUInt32(searchData.AsSpan()[(pointer + 56)..(pointer + 60)]);

                if (stars == gearStarsNumUpDown.Value
                    && mainAbility == gearMAComboBox.SelectedIndex
                    && sub1 == gearS1ComboBox.SelectedIndex
                    && sub2 == gearS2ComboBox.SelectedIndex
                    && sub3 == gearS3ComboBox.SelectedIndex
                    && (gearidkExpCheckbox.Checked || gearExp == gearExpNumUpDown.Value))
                {
                    results.Add((baseAddress + pointer, gearSeed));
                }

                pointer += gearRamSize;
            }

            if (results.Count == 0)
            {
                MessageBox.Show("no results");
            }
            else
            {
                GearSeedFinderResultsForm resultsForm = new();
                resultsForm.ShowResults(results);
                resultsForm.ShowDialog();
            }
        }

        private void tableTurfRetrieveMatchButton_Click(object sender, EventArgs e)
        {
            if (!readyForUserInput || gameBuild == null)
            {
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }

            tableTurfSpecialAddress = _connectionManager.TrackStepsAddress(gameBuild.TableTurf.SpecialBaseAddress, gameBuild.TableTurf.SpecialMemorySteps, 1);
            tableTurfPointAddress = _connectionManager.TrackStepsAddress(gameBuild.TableTurf.PointsBaseAddress, gameBuild.TableTurf.PointsMemorySteps, 1);
            ShowTableTurfData();
        }

        private void tableTurfPointLockCheckbox_CheckedChanged(object sender, EventArgs e)
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

            if (tableTurfPointLockCheckbox.Checked)
            {
                uint points = (uint)tableTurfPointsNumUpDown.Value;
                _connectionManager.FreezeAddress(tableTurfPointAddress, $"{(points & 0x000000FF) << 24 | (points & 0x0000FF00) << 8 | (points & 0x00FF0000) >> 8 | (points & 0xFF000000) >> 24:X8}");
            }
            else
            {
                _connectionManager.UnfreezeAddress(tableTurfPointAddress);
            }
        }

        private void tableTurfPointsNumUpDown_ValueChanged(object sender, EventArgs e)
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

            uint points = (uint)tableTurfPointsNumUpDown.Value;
            _connectionManager.PokeAddress(tableTurfPointAddress, $"{(points & 0x000000FF) << 24 | (points & 0x0000FF00) << 8 | (points & 0x00FF0000) >> 8 | (points & 0xFF000000) >> 24:X8}");
        }

        private void tableTurfSpecialLockCheckbox_CheckedChanged(object sender, EventArgs e)
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

            if (tableTurfSpecialLockCheckbox.Checked)
            {
                uint special = (uint)tableTurfSpecialNumUpDown.Value;
                _connectionManager.FreezeAddress(tableTurfSpecialAddress, $"{(special & 0x000000FF) << 24 | (special & 0x0000FF00) << 8 | (special & 0x00FF0000) >> 8 | (special & 0xFF000000) >> 24:X8}");
            }
            else
            {
                _connectionManager.UnfreezeAddress(tableTurfSpecialAddress);
            }
        }

        private void tableTurfSpecialNumUpDown_ValueChanged(object sender, EventArgs e)
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

            uint special = (uint)tableTurfSpecialNumUpDown.Value;
            _connectionManager.PokeAddress(tableTurfSpecialAddress, $"{(special & 0x000000FF) << 24 | (special & 0x0000FF00) << 8 | (special & 0x00FF0000) >> 8 | (special & 0xFF000000) >> 24:X8}");
        }

        private void tableTurfCpuPointsNumUpDown_ValueChanged(object sender, EventArgs e)
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

            uint points = (uint)tableTurfCpuPointsNumUpDown.Value;
            _connectionManager.PokeAddress(tableTurfPointAddress + 4, $"{(points & 0x000000FF) << 24 | (points & 0x0000FF00) << 8 | (points & 0x00FF0000) >> 8 | (points & 0xFF000000) >> 24:X8}");

        }

        private void tableTurfCpuPointLockCheckbox_CheckedChanged(object sender, EventArgs e)
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

            if (tableTurfCpuPointLockCheckbox.Checked)
            {
                uint points = (uint)tableTurfCpuPointsNumUpDown.Value;
                _connectionManager.FreezeAddress(tableTurfPointAddress + 4, $"{(points & 0x000000FF) << 24 | (points & 0x0000FF00) << 8 | (points & 0x00FF0000) >> 8 | (points & 0xFF000000) >> 24:X8}");
            }
            else
            {
                _connectionManager.UnfreezeAddress(tableTurfPointAddress + 4);
            }
        }

        private void tableTurfCpuSpecialNumUpDown_ValueChanged(object sender, EventArgs e)
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

            uint special = (uint)tableTurfCpuSpecialNumUpDown.Value;
            _connectionManager.PokeAddress(tableTurfSpecialAddress + 4, $"{(special & 0x000000FF) << 24 | (special & 0x0000FF00) << 8 | (special & 0x00FF0000) >> 8 | (special & 0xFF000000) >> 24:X8}");
        }

        private void tableTurfCpuSpecialLockCheckbox_CheckedChanged(object sender, EventArgs e)
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

            if (tableTurfCpuSpecialLockCheckbox.Checked)
            {
                uint special = (uint)tableTurfCpuSpecialNumUpDown.Value;
                _connectionManager.FreezeAddress(tableTurfSpecialAddress + 4, $"{(special & 0x000000FF) << 24 | (special & 0x0000FF00) << 8 | (special & 0x00FF0000) >> 8 | (special & 0xFF000000) >> 24:X8}");
            }
            else
            {
                _connectionManager.UnfreezeAddress(tableTurfSpecialAddress + 4);
            }
        }

        private void ipTextBox_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.ipAddress = ipTextBox.Text;
            Settings.Default.Save();
        }
    }
}