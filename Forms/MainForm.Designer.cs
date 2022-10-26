namespace ShiverBot.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optionTabControl = new System.Windows.Forms.TabControl();
            this.gearTabPage = new System.Windows.Forms.TabPage();
            this.gearSeedFinderLabel2 = new System.Windows.Forms.Label();
            this.gearSearchButton = new System.Windows.Forms.Button();
            this.gearS3ComboBox = new System.Windows.Forms.ComboBox();
            this.gearSubAbility3Label = new System.Windows.Forms.Label();
            this.gearS2ComboBox = new System.Windows.Forms.ComboBox();
            this.gearSubAbility2Label = new System.Windows.Forms.Label();
            this.gearS1ComboBox = new System.Windows.Forms.ComboBox();
            this.gearSubAbility1Label = new System.Windows.Forms.Label();
            this.gearMAComboBox = new System.Windows.Forms.ComboBox();
            this.gearMainAbilityLabel = new System.Windows.Forms.Label();
            this.gearStarsNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.gearStarsLabel = new System.Windows.Forms.Label();
            this.gearidkExpCheckbox = new System.Windows.Forms.CheckBox();
            this.gearExpNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.currenteExpLabel = new System.Windows.Forms.Label();
            this.gearTypeComboBox = new System.Windows.Forms.ComboBox();
            this.gearTypeLabel = new System.Windows.Forms.Label();
            this.gearSeedFinderLabel = new System.Windows.Forms.Label();
            this.postPrinterTabPage = new System.Windows.Forms.TabPage();
            this.postPrinterWaitNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.postPrinterHintLabel = new System.Windows.Forms.Label();
            this.postPrinterLabel = new System.Windows.Forms.Label();
            this.printPostManuallyButton = new System.Windows.Forms.Button();
            this.browseImageButton = new System.Windows.Forms.Button();
            this.openedImagePictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bytesToReadNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.readButton = new System.Windows.Forms.Button();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.peekLabel = new System.Windows.Forms.Label();
            this.saveToFileCheckBox = new System.Windows.Forms.CheckBox();
            this.readMainButton = new System.Windows.Forms.Button();
            this.readAbsButton = new System.Windows.Forms.Button();
            this.metaDataButton = new System.Windows.Forms.Button();
            this.advancedGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.optionTabControl.SuspendLayout();
            this.gearTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gearStarsNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gearExpNumUpDown)).BeginInit();
            this.postPrinterTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postPrinterWaitNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.openedImagePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bytesToReadNumUpDown)).BeginInit();
            this.advancedGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(12, 12);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.PlaceholderText = "IP Address";
            this.ipTextBox.Size = new System.Drawing.Size(100, 23);
            this.ipTextBox.TabIndex = 0;
            this.ipTextBox.TextChanged += new System.EventHandler(this.ipTextBox_TextChanged);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(118, 11);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(80, 23);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(208, 15);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(38, 15);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "status";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optionTabControl);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(383, 253);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Features";
            // 
            // optionTabControl
            // 
            this.optionTabControl.Controls.Add(this.gearTabPage);
            this.optionTabControl.Controls.Add(this.postPrinterTabPage);
            this.optionTabControl.Location = new System.Drawing.Point(6, 22);
            this.optionTabControl.Name = "optionTabControl";
            this.optionTabControl.SelectedIndex = 0;
            this.optionTabControl.Size = new System.Drawing.Size(371, 222);
            this.optionTabControl.TabIndex = 8;
            // 
            // gearTabPage
            // 
            this.gearTabPage.Controls.Add(this.gearSeedFinderLabel2);
            this.gearTabPage.Controls.Add(this.gearSearchButton);
            this.gearTabPage.Controls.Add(this.gearS3ComboBox);
            this.gearTabPage.Controls.Add(this.gearSubAbility3Label);
            this.gearTabPage.Controls.Add(this.gearS2ComboBox);
            this.gearTabPage.Controls.Add(this.gearSubAbility2Label);
            this.gearTabPage.Controls.Add(this.gearS1ComboBox);
            this.gearTabPage.Controls.Add(this.gearSubAbility1Label);
            this.gearTabPage.Controls.Add(this.gearMAComboBox);
            this.gearTabPage.Controls.Add(this.gearMainAbilityLabel);
            this.gearTabPage.Controls.Add(this.gearStarsNumUpDown);
            this.gearTabPage.Controls.Add(this.gearStarsLabel);
            this.gearTabPage.Controls.Add(this.gearidkExpCheckbox);
            this.gearTabPage.Controls.Add(this.gearExpNumUpDown);
            this.gearTabPage.Controls.Add(this.currenteExpLabel);
            this.gearTabPage.Controls.Add(this.gearTypeComboBox);
            this.gearTabPage.Controls.Add(this.gearTypeLabel);
            this.gearTabPage.Controls.Add(this.gearSeedFinderLabel);
            this.gearTabPage.Location = new System.Drawing.Point(4, 24);
            this.gearTabPage.Name = "gearTabPage";
            this.gearTabPage.Size = new System.Drawing.Size(363, 194);
            this.gearTabPage.TabIndex = 4;
            this.gearTabPage.Text = "Gear";
            this.gearTabPage.UseVisualStyleBackColor = true;
            // 
            // gearSeedFinderLabel2
            // 
            this.gearSeedFinderLabel2.AutoSize = true;
            this.gearSeedFinderLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.gearSeedFinderLabel2.ForeColor = System.Drawing.Color.SlateGray;
            this.gearSeedFinderLabel2.Location = new System.Drawing.Point(233, 108);
            this.gearSeedFinderLabel2.Name = "gearSeedFinderLabel2";
            this.gearSeedFinderLabel2.Size = new System.Drawing.Size(119, 30);
            this.gearSeedFinderLabel2.TabIndex = 24;
            this.gearSeedFinderLabel2.Text = "Fill in everything\r\nbefore clicking search";
            this.gearSeedFinderLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // gearSearchButton
            // 
            this.gearSearchButton.Enabled = false;
            this.gearSearchButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearSearchButton.Location = new System.Drawing.Point(232, 147);
            this.gearSearchButton.Name = "gearSearchButton";
            this.gearSearchButton.Size = new System.Drawing.Size(120, 36);
            this.gearSearchButton.TabIndex = 23;
            this.gearSearchButton.Text = "Search";
            this.gearSearchButton.UseVisualStyleBackColor = true;
            this.gearSearchButton.Click += new System.EventHandler(this.gearSearchButton_Click);
            // 
            // gearS3ComboBox
            // 
            this.gearS3ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gearS3ComboBox.Enabled = false;
            this.gearS3ComboBox.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearS3ComboBox.FormattingEnabled = true;
            this.gearS3ComboBox.Items.AddRange(new object[] {
            "None",
            "Ink Saver Main",
            "Ink Saver Sub",
            "Ink Recovery Up",
            "Run Speed Up",
            "Swim Speed Up",
            "Special Charge Up",
            "Special Saver",
            "Special Power Up",
            "Quick Respawn",
            "Quick Super Jump",
            "Sub Power Up",
            "Ink Resistance Up",
            "Sub Resistance Up",
            "Intensify Action"});
            this.gearS3ComboBox.Location = new System.Drawing.Point(86, 162);
            this.gearS3ComboBox.Name = "gearS3ComboBox";
            this.gearS3ComboBox.Size = new System.Drawing.Size(118, 21);
            this.gearS3ComboBox.TabIndex = 22;
            // 
            // gearSubAbility3Label
            // 
            this.gearSubAbility3Label.AutoSize = true;
            this.gearSubAbility3Label.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearSubAbility3Label.Location = new System.Drawing.Point(6, 165);
            this.gearSubAbility3Label.Name = "gearSubAbility3Label";
            this.gearSubAbility3Label.Size = new System.Drawing.Size(74, 13);
            this.gearSubAbility3Label.TabIndex = 21;
            this.gearSubAbility3Label.Text = "Sub Ability 3:";
            // 
            // gearS2ComboBox
            // 
            this.gearS2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gearS2ComboBox.Enabled = false;
            this.gearS2ComboBox.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearS2ComboBox.FormattingEnabled = true;
            this.gearS2ComboBox.Items.AddRange(new object[] {
            "None",
            "Ink Saver Main",
            "Ink Saver Sub",
            "Ink Recovery Up",
            "Run Speed Up",
            "Swim Speed Up",
            "Special Charge Up",
            "Special Saver",
            "Special Power Up",
            "Quick Respawn",
            "Quick Super Jump",
            "Sub Power Up",
            "Ink Resistance Up",
            "Sub Resistance Up",
            "Intensify Action"});
            this.gearS2ComboBox.Location = new System.Drawing.Point(86, 135);
            this.gearS2ComboBox.Name = "gearS2ComboBox";
            this.gearS2ComboBox.Size = new System.Drawing.Size(118, 21);
            this.gearS2ComboBox.TabIndex = 20;
            // 
            // gearSubAbility2Label
            // 
            this.gearSubAbility2Label.AutoSize = true;
            this.gearSubAbility2Label.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearSubAbility2Label.Location = new System.Drawing.Point(6, 138);
            this.gearSubAbility2Label.Name = "gearSubAbility2Label";
            this.gearSubAbility2Label.Size = new System.Drawing.Size(74, 13);
            this.gearSubAbility2Label.TabIndex = 19;
            this.gearSubAbility2Label.Text = "Sub Ability 2:";
            // 
            // gearS1ComboBox
            // 
            this.gearS1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gearS1ComboBox.Enabled = false;
            this.gearS1ComboBox.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearS1ComboBox.FormattingEnabled = true;
            this.gearS1ComboBox.Items.AddRange(new object[] {
            "None",
            "Ink Saver Main",
            "Ink Saver Sub",
            "Ink Recovery Up",
            "Run Speed Up",
            "Swim Speed Up",
            "Special Charge Up",
            "Special Saver",
            "Special Power Up",
            "Quick Respawn",
            "Quick Super Jump",
            "Sub Power Up",
            "Ink Resistance Up",
            "Sub Resistance Up",
            "Intensify Action"});
            this.gearS1ComboBox.Location = new System.Drawing.Point(86, 108);
            this.gearS1ComboBox.Name = "gearS1ComboBox";
            this.gearS1ComboBox.Size = new System.Drawing.Size(118, 21);
            this.gearS1ComboBox.TabIndex = 18;
            // 
            // gearSubAbility1Label
            // 
            this.gearSubAbility1Label.AutoSize = true;
            this.gearSubAbility1Label.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearSubAbility1Label.Location = new System.Drawing.Point(6, 111);
            this.gearSubAbility1Label.Name = "gearSubAbility1Label";
            this.gearSubAbility1Label.Size = new System.Drawing.Size(72, 13);
            this.gearSubAbility1Label.TabIndex = 17;
            this.gearSubAbility1Label.Text = "Sub Ability 1:";
            // 
            // gearMAComboBox
            // 
            this.gearMAComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gearMAComboBox.Enabled = false;
            this.gearMAComboBox.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearMAComboBox.FormattingEnabled = true;
            this.gearMAComboBox.Items.AddRange(new object[] {
            "Ink Saver Main",
            "Ink Saver Sub",
            "Ink Recovery Up",
            "Run Speed Up",
            "Swim Speed Up",
            "Special Charge Up",
            "Special Saver",
            "Special Power Up",
            "Quick Respawn",
            "Quick Super Jump",
            "Sub Power Up",
            "Ink Resistance Up",
            "Sub Resistance Up",
            "Intensify Action",
            "Opening Gambit",
            "Last Ditch Effort",
            "Tenacity",
            "Comeback",
            "Ninja Squid",
            "Haunt",
            "Thermal Ink",
            "Respawn Punisher",
            "Ability Doubler",
            "Stealth Jump",
            "Object Shredder",
            "Drop Roller"});
            this.gearMAComboBox.Location = new System.Drawing.Point(86, 81);
            this.gearMAComboBox.Name = "gearMAComboBox";
            this.gearMAComboBox.Size = new System.Drawing.Size(118, 21);
            this.gearMAComboBox.TabIndex = 16;
            // 
            // gearMainAbilityLabel
            // 
            this.gearMainAbilityLabel.AutoSize = true;
            this.gearMainAbilityLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearMainAbilityLabel.Location = new System.Drawing.Point(6, 84);
            this.gearMainAbilityLabel.Name = "gearMainAbilityLabel";
            this.gearMainAbilityLabel.Size = new System.Drawing.Size(71, 13);
            this.gearMainAbilityLabel.TabIndex = 15;
            this.gearMainAbilityLabel.Text = "Main Ability:";
            // 
            // gearStarsNumUpDown
            // 
            this.gearStarsNumUpDown.Enabled = false;
            this.gearStarsNumUpDown.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearStarsNumUpDown.Location = new System.Drawing.Point(221, 26);
            this.gearStarsNumUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.gearStarsNumUpDown.Name = "gearStarsNumUpDown";
            this.gearStarsNumUpDown.Size = new System.Drawing.Size(35, 22);
            this.gearStarsNumUpDown.TabIndex = 14;
            // 
            // gearStarsLabel
            // 
            this.gearStarsLabel.AutoSize = true;
            this.gearStarsLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearStarsLabel.Location = new System.Drawing.Point(180, 29);
            this.gearStarsLabel.Name = "gearStarsLabel";
            this.gearStarsLabel.Size = new System.Drawing.Size(35, 13);
            this.gearStarsLabel.TabIndex = 3;
            this.gearStarsLabel.Text = "Stars:";
            // 
            // gearidkExpCheckbox
            // 
            this.gearidkExpCheckbox.AutoSize = true;
            this.gearidkExpCheckbox.Enabled = false;
            this.gearidkExpCheckbox.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearidkExpCheckbox.Location = new System.Drawing.Point(180, 56);
            this.gearidkExpCheckbox.Name = "gearidkExpCheckbox";
            this.gearidkExpCheckbox.Size = new System.Drawing.Size(88, 17);
            this.gearidkExpCheckbox.TabIndex = 2;
            this.gearidkExpCheckbox.Text = "I don\'t know";
            this.gearidkExpCheckbox.UseVisualStyleBackColor = true;
            this.gearidkExpCheckbox.CheckedChanged += new System.EventHandler(this.gearidkExpCheckbox_CheckedChanged);
            // 
            // gearExpNumUpDown
            // 
            this.gearExpNumUpDown.Enabled = false;
            this.gearExpNumUpDown.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearExpNumUpDown.Location = new System.Drawing.Point(86, 53);
            this.gearExpNumUpDown.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.gearExpNumUpDown.Name = "gearExpNumUpDown";
            this.gearExpNumUpDown.Size = new System.Drawing.Size(79, 22);
            this.gearExpNumUpDown.TabIndex = 1;
            // 
            // currenteExpLabel
            // 
            this.currenteExpLabel.AutoSize = true;
            this.currenteExpLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.currenteExpLabel.Location = new System.Drawing.Point(6, 57);
            this.currenteExpLabel.Name = "currenteExpLabel";
            this.currenteExpLabel.Size = new System.Drawing.Size(69, 13);
            this.currenteExpLabel.TabIndex = 0;
            this.currenteExpLabel.Text = "Current EXP:";
            // 
            // gearTypeComboBox
            // 
            this.gearTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gearTypeComboBox.Enabled = false;
            this.gearTypeComboBox.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearTypeComboBox.FormattingEnabled = true;
            this.gearTypeComboBox.Items.AddRange(new object[] {
            "Headgear",
            "Clothes",
            "Shoes"});
            this.gearTypeComboBox.Location = new System.Drawing.Point(86, 26);
            this.gearTypeComboBox.Name = "gearTypeComboBox";
            this.gearTypeComboBox.Size = new System.Drawing.Size(79, 21);
            this.gearTypeComboBox.TabIndex = 13;
            // 
            // gearTypeLabel
            // 
            this.gearTypeLabel.AutoSize = true;
            this.gearTypeLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gearTypeLabel.Location = new System.Drawing.Point(6, 30);
            this.gearTypeLabel.Name = "gearTypeLabel";
            this.gearTypeLabel.Size = new System.Drawing.Size(61, 13);
            this.gearTypeLabel.TabIndex = 12;
            this.gearTypeLabel.Text = "Gear Type:";
            // 
            // gearSeedFinderLabel
            // 
            this.gearSeedFinderLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.gearSeedFinderLabel.ForeColor = System.Drawing.Color.SaddleBrown;
            this.gearSeedFinderLabel.Location = new System.Drawing.Point(6, 3);
            this.gearSeedFinderLabel.Name = "gearSeedFinderLabel";
            this.gearSeedFinderLabel.Size = new System.Drawing.Size(348, 15);
            this.gearSeedFinderLabel.TabIndex = 11;
            this.gearSeedFinderLabel.Text = "GEAR SEED FINDER";
            this.gearSeedFinderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // postPrinterTabPage
            // 
            this.postPrinterTabPage.Controls.Add(this.postPrinterWaitNumUpDown);
            this.postPrinterTabPage.Controls.Add(this.label2);
            this.postPrinterTabPage.Controls.Add(this.postPrinterHintLabel);
            this.postPrinterTabPage.Controls.Add(this.postPrinterLabel);
            this.postPrinterTabPage.Controls.Add(this.printPostManuallyButton);
            this.postPrinterTabPage.Controls.Add(this.browseImageButton);
            this.postPrinterTabPage.Controls.Add(this.openedImagePictureBox);
            this.postPrinterTabPage.Controls.Add(this.label1);
            this.postPrinterTabPage.Location = new System.Drawing.Point(4, 24);
            this.postPrinterTabPage.Name = "postPrinterTabPage";
            this.postPrinterTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.postPrinterTabPage.Size = new System.Drawing.Size(363, 194);
            this.postPrinterTabPage.TabIndex = 5;
            this.postPrinterTabPage.Text = "Post Printer";
            this.postPrinterTabPage.UseVisualStyleBackColor = true;
            // 
            // postPrinterWaitNumUpDown
            // 
            this.postPrinterWaitNumUpDown.Location = new System.Drawing.Point(315, 168);
            this.postPrinterWaitNumUpDown.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.postPrinterWaitNumUpDown.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.postPrinterWaitNumUpDown.Name = "postPrinterWaitNumUpDown";
            this.postPrinterWaitNumUpDown.Size = new System.Drawing.Size(42, 23);
            this.postPrinterWaitNumUpDown.TabIndex = 114;
            this.postPrinterWaitNumUpDown.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 30);
            this.label2.TabIndex = 113;
            this.label2.Text = "Wait between\r\nbuttons (ms):";
            // 
            // postPrinterHintLabel
            // 
            this.postPrinterHintLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.postPrinterHintLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.postPrinterHintLabel.Location = new System.Drawing.Point(225, 132);
            this.postPrinterHintLabel.Name = "postPrinterHintLabel";
            this.postPrinterHintLabel.Size = new System.Drawing.Size(116, 26);
            this.postPrinterHintLabel.TabIndex = 112;
            this.postPrinterHintLabel.Text = "Not printing anything?\r\nRestart your Switch...";
            this.postPrinterHintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.postPrinterHintLabel.Visible = false;
            // 
            // postPrinterLabel
            // 
            this.postPrinterLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.postPrinterLabel.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.postPrinterLabel.Location = new System.Drawing.Point(127, 132);
            this.postPrinterLabel.Name = "postPrinterLabel";
            this.postPrinterLabel.Size = new System.Drawing.Size(97, 22);
            this.postPrinterLabel.TabIndex = 111;
            this.postPrinterLabel.Text = "POST PRINTER";
            this.postPrinterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // printPostManuallyButton
            // 
            this.printPostManuallyButton.Location = new System.Drawing.Point(127, 157);
            this.printPostManuallyButton.Name = "printPostManuallyButton";
            this.printPostManuallyButton.Size = new System.Drawing.Size(97, 31);
            this.printPostManuallyButton.TabIndex = 28;
            this.printPostManuallyButton.Text = "Begin printing";
            this.printPostManuallyButton.UseVisualStyleBackColor = true;
            this.printPostManuallyButton.Click += new System.EventHandler(this.printPostManuallyButton_Click);
            // 
            // browseImageButton
            // 
            this.browseImageButton.Location = new System.Drawing.Point(21, 165);
            this.browseImageButton.Name = "browseImageButton";
            this.browseImageButton.Size = new System.Drawing.Size(96, 23);
            this.browseImageButton.TabIndex = 27;
            this.browseImageButton.Text = "Browse image";
            this.browseImageButton.UseVisualStyleBackColor = true;
            this.browseImageButton.Click += new System.EventHandler(this.browseImageButton_Click);
            // 
            // openedImagePictureBox
            // 
            this.openedImagePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.openedImagePictureBox.Location = new System.Drawing.Point(21, 6);
            this.openedImagePictureBox.Name = "openedImagePictureBox";
            this.openedImagePictureBox.Size = new System.Drawing.Size(320, 120);
            this.openedImagePictureBox.TabIndex = 26;
            this.openedImagePictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.SlateGray;
            this.label1.Location = new System.Drawing.Point(21, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 30);
            this.label1.TabIndex = 25;
            this.label1.Text = "Select a 320x120\r\n1-bit .PNG image";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bytesToReadNumUpDown
            // 
            this.bytesToReadNumUpDown.Location = new System.Drawing.Point(136, 38);
            this.bytesToReadNumUpDown.Maximum = new decimal(new int[] {
            104857600,
            0,
            0,
            0});
            this.bytesToReadNumUpDown.Name = "bytesToReadNumUpDown";
            this.bytesToReadNumUpDown.Size = new System.Drawing.Size(70, 23);
            this.bytesToReadNumUpDown.TabIndex = 103;
            // 
            // readButton
            // 
            this.readButton.Location = new System.Drawing.Point(217, 53);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(77, 23);
            this.readButton.TabIndex = 105;
            this.readButton.Text = "read heap";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // addressTextBox
            // 
            this.addressTextBox.Location = new System.Drawing.Point(6, 37);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.PlaceholderText = "Address (without 0x)";
            this.addressTextBox.Size = new System.Drawing.Size(124, 23);
            this.addressTextBox.TabIndex = 102;
            // 
            // peekLabel
            // 
            this.peekLabel.AutoSize = true;
            this.peekLabel.Location = new System.Drawing.Point(6, 19);
            this.peekLabel.Name = "peekLabel";
            this.peekLabel.Size = new System.Drawing.Size(66, 15);
            this.peekLabel.TabIndex = 101;
            this.peekLabel.Text = "Peek bytes:";
            // 
            // saveToFileCheckBox
            // 
            this.saveToFileCheckBox.AutoSize = true;
            this.saveToFileCheckBox.Checked = true;
            this.saveToFileCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveToFileCheckBox.Location = new System.Drawing.Point(136, 67);
            this.saveToFileCheckBox.Name = "saveToFileCheckBox";
            this.saveToFileCheckBox.Size = new System.Drawing.Size(82, 19);
            this.saveToFileCheckBox.TabIndex = 104;
            this.saveToFileCheckBox.Text = "save to file";
            this.saveToFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // readMainButton
            // 
            this.readMainButton.Location = new System.Drawing.Point(300, 53);
            this.readMainButton.Name = "readMainButton";
            this.readMainButton.Size = new System.Drawing.Size(77, 23);
            this.readMainButton.TabIndex = 106;
            this.readMainButton.Text = "read main";
            this.readMainButton.UseVisualStyleBackColor = true;
            this.readMainButton.Click += new System.EventHandler(this.readMainButton_Click);
            // 
            // readAbsButton
            // 
            this.readAbsButton.Location = new System.Drawing.Point(217, 24);
            this.readAbsButton.Name = "readAbsButton";
            this.readAbsButton.Size = new System.Drawing.Size(96, 23);
            this.readAbsButton.TabIndex = 110;
            this.readAbsButton.Text = "read absolute";
            this.readAbsButton.UseVisualStyleBackColor = true;
            this.readAbsButton.Click += new System.EventHandler(this.readAbsButton_Click);
            // 
            // metaDataButton
            // 
            this.metaDataButton.Location = new System.Drawing.Point(319, 24);
            this.metaDataButton.Name = "metaDataButton";
            this.metaDataButton.Size = new System.Drawing.Size(58, 23);
            this.metaDataButton.TabIndex = 111;
            this.metaDataButton.Text = "meta";
            this.metaDataButton.UseVisualStyleBackColor = true;
            this.metaDataButton.Click += new System.EventHandler(this.metaDataButton_Click);
            // 
            // advancedGroupBox
            // 
            this.advancedGroupBox.Controls.Add(this.metaDataButton);
            this.advancedGroupBox.Controls.Add(this.readAbsButton);
            this.advancedGroupBox.Controls.Add(this.readMainButton);
            this.advancedGroupBox.Controls.Add(this.saveToFileCheckBox);
            this.advancedGroupBox.Controls.Add(this.peekLabel);
            this.advancedGroupBox.Controls.Add(this.addressTextBox);
            this.advancedGroupBox.Controls.Add(this.readButton);
            this.advancedGroupBox.Controls.Add(this.bytesToReadNumUpDown);
            this.advancedGroupBox.Location = new System.Drawing.Point(12, 300);
            this.advancedGroupBox.Name = "advancedGroupBox";
            this.advancedGroupBox.Size = new System.Drawing.Size(383, 95);
            this.advancedGroupBox.TabIndex = 100;
            this.advancedGroupBox.TabStop = false;
            this.advancedGroupBox.Text = "Advanced";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 406);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.advancedGroupBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.ipTextBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(423, 445);
            this.MinimumSize = new System.Drawing.Size(423, 445);
            this.Name = "MainForm";
            this.Text = "ShiverBot v1.4 — the post-banwave version";
            this.groupBox1.ResumeLayout(false);
            this.optionTabControl.ResumeLayout(false);
            this.gearTabPage.ResumeLayout(false);
            this.gearTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gearStarsNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gearExpNumUpDown)).EndInit();
            this.postPrinterTabPage.ResumeLayout(false);
            this.postPrinterTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postPrinterWaitNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.openedImagePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bytesToReadNumUpDown)).EndInit();
            this.advancedGroupBox.ResumeLayout(false);
            this.advancedGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox ipTextBox;
        private Button connectButton;
        private Label statusLabel;
        private GroupBox groupBox1;
        private TabControl optionTabControl;
        private TabPage gearTabPage;
        private Label gearSeedFinderLabel;
        private ComboBox gearMAComboBox;
        private Label gearMainAbilityLabel;
        private NumericUpDown gearStarsNumUpDown;
        private Label gearStarsLabel;
        private CheckBox gearidkExpCheckbox;
        private NumericUpDown gearExpNumUpDown;
        private Label currenteExpLabel;
        private ComboBox gearTypeComboBox;
        private Label gearTypeLabel;
        private Label gearSeedFinderLabel2;
        private Button gearSearchButton;
        private ComboBox gearS3ComboBox;
        private Label gearSubAbility3Label;
        private ComboBox gearS2ComboBox;
        private Label gearSubAbility2Label;
        private ComboBox gearS1ComboBox;
        private Label gearSubAbility1Label;
        private TabPage postPrinterTabPage;
        private Button browseImageButton;
        private Label label1;
        private PictureBox openedImagePictureBox;
        private Button printPostManuallyButton;
        private Label postPrinterLabel;
        private NumericUpDown postPrinterWaitNumUpDown;
        private Label label2;
        private Label postPrinterHintLabel;
        private NumericUpDown bytesToReadNumUpDown;
        private Button readButton;
        private TextBox addressTextBox;
        private Label peekLabel;
        private CheckBox saveToFileCheckBox;
        private Button readMainButton;
        private Button readAbsButton;
        private Button metaDataButton;
        private GroupBox advancedGroupBox;
    }
}