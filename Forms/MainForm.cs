using ShiverBot.Imaging;
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

        private CanolaPost? openedImage;


        public MainForm()
        {
            InitializeComponent();
            _connectionManager = new();
            _savedBuildReader = new();

            ipTextBox.Text = Settings.Default.ipAddress;
            statusLabel.Text = "not connected";
            gearTypeComboBox.SelectedIndex = 0;
            gearMAComboBox.SelectedIndex = 0;
            gearS1ComboBox.SelectedIndex = 0;
            gearS2ComboBox.SelectedIndex = 0;
            gearS3ComboBox.SelectedIndex = 0;
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

        private void TryDisconnect()
        {
            _connectionManager.TryDisconnect();
            readyForUserInput = false;
            connectButton.Enabled = true;
            connectButton.Text = "Connect";
            statusLabel.Text = "unconnected";

            SetGearSeedFinderBoxesStatus(false);
        }

        private void PostPrintThread()
        {
            if (openedImage == null || !_connectionManager.IsSwitchConnected)
            {
                return;
            }

            string? clickSequence = openedImage.GetNextClickSequence();
            while (printPostManuallyButton.Tag is 1 && clickSequence is not null)
            {
                _connectionManager.SendCommandAsIs($"clickSeq {clickSequence.Replace("nu", postPrinterWaitNumUpDown.Value.ToString())}\r\n", 256);
                Thread.Sleep(10);
                clickSequence = openedImage.GetNextClickSequence();
            }

            Invoke(() =>
            {
                printPostManuallyButton.Tag = null;
                printPostManuallyButton.Text = "Begin printing";
                openedImage.ResetPointer();
                postPrinterHintLabel.Visible = false;
            });

            _connectionManager.SendMessage("click PLUS\r\n");
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

            if (!_connectionManager.TryConnect(ipTextBox.Text, 6000, out string error))
            {
                if (error == string.Empty)
                {
                    MessageBox.Show($"error: failed to connect to the IP address.");
                }
                else
                {
                    MessageBox.Show($"error:\r\n{error}");
                }
                connectButton.Enabled = true;
                connectButton.Text = "Connect";
                statusLabel.Text = "not connected";
                return;
            }
            else
            {
                string titleId = _connectionManager.SendCommandAsIs("getTitleID", 33)[..16].Trim();
                if (titleId != "0100C2500FC20000" && titleId != "100C2500FC20000")
                {
                    MessageBox.Show($"error: the game is not Splatoon 3.\nReceived title id is {titleId}");
                    connectButton.Enabled = true;
                    connectButton.Text = "Connect";
                    statusLabel.Text = "not connected";
                    return;
                }

                string buildId = _connectionManager.SendCommandAsIs("getBuildID", 33)[..16].Trim();
                string languageCode = _connectionManager.SendCommandAsIs("getSystemLanguage", 5)[..2].Trim();
                if (languageCode != "1" && _savedBuildReader.GetBuild($"{buildId}.{languageCode}") is not null)
                {
                    buildId += $".{languageCode}";
                }
                SavedBuild? build = _savedBuildReader.GetBuild(buildId);
                if (build == null)
                {
                    MessageBox.Show($"error: no addresses found for this build id.\nReceived build id is {buildId}");
                    connectButton.Enabled = true;
                    connectButton.Text = "Connect";
                    statusLabel.Text = "not connected";
                    return;
                }

                gameBuild = build;
            }

            connectButton.Enabled = true;
            connectButton.Text = "Disconnect";
            statusLabel.Text = "connected";
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
            string languageCode = _connectionManager.SendCommandAsIs("getSystemLanguage", 5)[..2].Trim();
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

        private void creatorLabel_Click(object sender, EventArgs e)
        {
            OpenUrl("http://github.com/supremetakoyaki");
        }

        private void creatorLabel2_Click(object sender, EventArgs e)
        {
            OpenUrl("http://github.com/supremetakoyaki");
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

        private void ipTextBox_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.ipAddress = ipTextBox.Text;
            Settings.Default.Save();
        }

        private void browseImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "PNG images|*.png";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                if (Image.FromFile(ofd.FileName) is Image loadedImage)
                {
                    Bitmap bitmap = new(loadedImage);
                    if (bitmap.Height != 120 || bitmap.Width != 320)
                    {
                        MessageBox.Show("error: image must be 320x120.");
                        return;
                    }

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            if (bitmap.GetPixel(x, y) != Color.FromArgb(255, 255, 255, 255) && bitmap.GetPixel(x, y) != Color.FromArgb(255, 0, 0, 0))
                            {
                                MessageBox.Show($"error: not a 1-bit image.\nat position ({x},{y}), a color that was neither black or white was found.\ncolor: {bitmap.GetPixel(x, y)}", "error");
                                return;
                            }
                        }
                    }

                    openedImagePictureBox.Image = bitmap;
                    openedImage = new(bitmap);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while reading image file:\n{ex}", "error");
            }
        }

        private void printPostManuallyButton_Click(object sender, EventArgs e)
        {
            if (openedImage == null)
            {
                MessageBox.Show("no opened image >:(");
                return;
            }
            else if (!_connectionManager.IsSwitchConnected)
            {
                MessageBox.Show("not connected >:(");
                return;
            }
            else if (printPostManuallyButton.Tag is 1)
            {
                printPostManuallyButton.Tag = null;
                _connectionManager.SendMessage("clickCancel\r\n");
                Thread.Sleep(10);
                _connectionManager.SendMessage("click PLUS\r\n");
                printPostManuallyButton.Text = "Begin printing";
                openedImage.ResetPointer();
                postPrinterHintLabel.Visible = false;
                return;
            }

            if (printPostManuallyButton.Tag is not 1 && MessageBox.Show("Open the post drawer, position yourself at the left and topmost pixel and set the pencil size to the smallest one.\nwhen you're ready, click Yes.\r\nIf nothing prints, restart your Switch and try again.", "Instructions", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }

            Task.Factory.StartNew(PostPrintThread);
            printPostManuallyButton.Tag = 1;
            printPostManuallyButton.Text = "Stop printing";
            postPrinterHintLabel.Visible = true;
        }
    }
}