namespace ShiverBot.Forms
{
    partial class GearSeedFinderResultsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GearSeedFinderResultsForm));
            this.resultsDataGridView = new System.Windows.Forms.DataGridView();
            this.resultsAddressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultsSeedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // resultsDataGridView
            // 
            this.resultsDataGridView.AllowUserToAddRows = false;
            this.resultsDataGridView.AllowUserToDeleteRows = false;
            this.resultsDataGridView.AllowUserToResizeColumns = false;
            this.resultsDataGridView.AllowUserToResizeRows = false;
            this.resultsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.resultsAddressColumn,
            this.resultsSeedColumn});
            this.resultsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.resultsDataGridView.MultiSelect = false;
            this.resultsDataGridView.Name = "resultsDataGridView";
            this.resultsDataGridView.ReadOnly = true;
            this.resultsDataGridView.RowHeadersVisible = false;
            this.resultsDataGridView.RowTemplate.Height = 25;
            this.resultsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultsDataGridView.ShowEditingIcon = false;
            this.resultsDataGridView.Size = new System.Drawing.Size(320, 240);
            this.resultsDataGridView.TabIndex = 0;
            // 
            // resultsAddressColumn
            // 
            this.resultsAddressColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.resultsAddressColumn.HeaderText = "Address";
            this.resultsAddressColumn.Name = "resultsAddressColumn";
            this.resultsAddressColumn.ReadOnly = true;
            this.resultsAddressColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // resultsSeedColumn
            // 
            this.resultsSeedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.resultsSeedColumn.HeaderText = "Seed";
            this.resultsSeedColumn.Name = "resultsSeedColumn";
            this.resultsSeedColumn.ReadOnly = true;
            this.resultsSeedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // GearSeedFinderResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 240);
            this.Controls.Add(this.resultsDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GearSeedFinderResultsForm";
            this.Text = "Gear Seed Finder — Results";
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView resultsDataGridView;
        private DataGridViewTextBoxColumn resultsAddressColumn;
        private DataGridViewTextBoxColumn resultsSeedColumn;
    }
}