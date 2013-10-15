namespace OnTopReplica.SidePanels {
	partial class RegionPanel {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.groupRegions = new System.Windows.Forms.GroupBox();
            this.checkRelative = new System.Windows.Forms.CheckBox();
            this.textRegionName = new OnTopReplica.FocusedTextBox();
            this.numH = new System.Windows.Forms.NumericUpDown();
            this.numW = new System.Windows.Forms.NumericUpDown();
            this.numY = new System.Windows.Forms.NumericUpDown();
            this.numX = new System.Windows.Forms.NumericUpDown();
            this.buttonDone = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.labelCurrentRegion = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.comboRegions = new WindowsFormsAero.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupRegions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).BeginInit();
            this.SuspendLayout();
            // 
            // groupRegions
            // 
            this.groupRegions.Controls.Add(this.checkRelative);
            this.groupRegions.Controls.Add(this.textRegionName);
            this.groupRegions.Controls.Add(this.numH);
            this.groupRegions.Controls.Add(this.numW);
            this.groupRegions.Controls.Add(this.numY);
            this.groupRegions.Controls.Add(this.numX);
            this.groupRegions.Controls.Add(this.buttonDone);
            this.groupRegions.Controls.Add(this.buttonReset);
            this.groupRegions.Controls.Add(this.labelHeight);
            this.groupRegions.Controls.Add(this.labelWidth);
            this.groupRegions.Controls.Add(this.labelY);
            this.groupRegions.Controls.Add(this.labelX);
            this.groupRegions.Controls.Add(this.labelCurrentRegion);
            this.groupRegions.Controls.Add(this.buttonDelete);
            this.groupRegions.Controls.Add(this.buttonSave);
            this.groupRegions.Controls.Add(this.comboRegions);
            this.groupRegions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupRegions.Location = new System.Drawing.Point(6, 6);
            this.groupRegions.Name = "groupRegions";
            this.groupRegions.Size = new System.Drawing.Size(218, 180);
            this.groupRegions.TabIndex = 0;
            this.groupRegions.TabStop = false;
            this.groupRegions.Text = "Regions:";
            // 
            // checkRelative
            // 
            this.checkRelative.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkRelative.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkRelative.Location = new System.Drawing.Point(6, 119);
            this.checkRelative.Name = "checkRelative";
            this.checkRelative.Size = new System.Drawing.Size(206, 18);
            this.checkRelative.TabIndex = 12;
            this.checkRelative.Text = "Relative to border";
            this.checkRelative.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkRelative.UseVisualStyleBackColor = true;
            this.checkRelative.CheckedChanged += new System.EventHandler(this.CheckRelative_checked);
            // 
            // textRegionName
            // 
            this.textRegionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textRegionName.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.textRegionName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textRegionName.Location = new System.Drawing.Point(6, 44);
            this.textRegionName.Name = "textRegionName";
            this.textRegionName.Size = new System.Drawing.Size(208, 20);
            this.textRegionName.TabIndex = 11;
            this.textRegionName.Visible = false;
            this.textRegionName.ConfirmInput += new System.EventHandler(this.Save_confirm);
            this.textRegionName.AbortInput += new System.EventHandler(this.Save_lost);
            // 
            // numH
            // 
            this.numH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numH.Enabled = false;
            this.numH.Location = new System.Drawing.Point(169, 93);
            this.numH.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numH.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numH.Name = "numH";
            this.numH.Size = new System.Drawing.Size(43, 20);
            this.numH.TabIndex = 7;
            this.numH.ValueChanged += new System.EventHandler(this.RegionValueSpinner_value_change);
            // 
            // numW
            // 
            this.numW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numW.Enabled = false;
            this.numW.Location = new System.Drawing.Point(169, 67);
            this.numW.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numW.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numW.Name = "numW";
            this.numW.Size = new System.Drawing.Size(43, 20);
            this.numW.TabIndex = 6;
            this.numW.ValueChanged += new System.EventHandler(this.RegionValueSpinner_value_change);
            // 
            // numY
            // 
            this.numY.Enabled = false;
            this.numY.Location = new System.Drawing.Point(55, 93);
            this.numY.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numY.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numY.Name = "numY";
            this.numY.Size = new System.Drawing.Size(43, 20);
            this.numY.TabIndex = 5;
            this.numY.ValueChanged += new System.EventHandler(this.RegionValueSpinner_value_change);
            // 
            // numX
            // 
            this.numX.Enabled = false;
            this.numX.Location = new System.Drawing.Point(55, 67);
            this.numX.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numX.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numX.Name = "numX";
            this.numX.Size = new System.Drawing.Size(43, 20);
            this.numX.TabIndex = 4;
            this.numX.ValueChanged += new System.EventHandler(this.RegionValueSpinner_value_change);
            // 
            // buttonDone
            // 
            this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDone.Image = global::OnTopReplica.Properties.Resources.xiao_ok;
            this.buttonDone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonDone.Location = new System.Drawing.Point(142, 151);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(70, 23);
            this.buttonDone.TabIndex = 9;
            this.buttonDone.Text = global::OnTopReplica.Strings.RegionsDoneButton;
            this.buttonDone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.Close_click);
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Location = new System.Drawing.Point(66, 151);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(70, 23);
            this.buttonReset.TabIndex = 8;
            this.buttonReset.Text = global::OnTopReplica.Strings.RegionsResetButton;
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.Reset_click);
            // 
            // labelHeight
            // 
            this.labelHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHeight.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelHeight.Location = new System.Drawing.Point(104, 95);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(60, 18);
            this.labelHeight.TabIndex = 9;
            this.labelHeight.Text = "Height";
            this.labelHeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelWidth
            // 
            this.labelWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWidth.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelWidth.Location = new System.Drawing.Point(107, 69);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(57, 18);
            this.labelWidth.TabIndex = 8;
            this.labelWidth.Text = "Width";
            this.labelWidth.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelY
            // 
            this.labelY.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelY.Location = new System.Drawing.Point(6, 96);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(43, 17);
            this.labelY.TabIndex = 5;
            this.labelY.Text = "Y";
            this.labelY.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelX
            // 
            this.labelX.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelX.Location = new System.Drawing.Point(6, 70);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(43, 17);
            this.labelX.TabIndex = 4;
            this.labelX.Text = "X";
            this.labelX.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelCurrentRegion
            // 
            this.labelCurrentRegion.AutoSize = true;
            this.labelCurrentRegion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentRegion.Location = new System.Drawing.Point(6, 47);
            this.labelCurrentRegion.Name = "labelCurrentRegion";
            this.labelCurrentRegion.Size = new System.Drawing.Size(76, 13);
            this.labelCurrentRegion.TabIndex = 3;
            this.labelCurrentRegion.Text = "Current region:";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Image = global::OnTopReplica.Properties.Resources.xiao_delete;
            this.buttonDelete.Location = new System.Drawing.Point(191, 18);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(23, 23);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.Delete_click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Enabled = false;
            this.buttonSave.Image = global::OnTopReplica.Properties.Resources.xiao_add;
            this.buttonSave.Location = new System.Drawing.Point(168, 18);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(23, 23);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.Save_click);
            // 
            // comboRegions
            // 
            this.comboRegions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboRegions.CueBannerText = global::OnTopReplica.Strings.RegionsStoredRegions;
            this.comboRegions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRegions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboRegions.FormattingEnabled = true;
            this.comboRegions.Location = new System.Drawing.Point(6, 19);
            this.comboRegions.Name = "comboRegions";
            this.comboRegions.Size = new System.Drawing.Size(160, 21);
            this.comboRegions.TabIndex = 0;
            this.comboRegions.SelectedIndexChanged += new System.EventHandler(this.RegionCombo_index_change);
            // 
            // RegionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupRegions);
            this.MinimumSize = new System.Drawing.Size(230, 185);
            this.Name = "RegionPanel";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Size = new System.Drawing.Size(230, 192);
            this.groupRegions.ResumeLayout(false);
            this.groupRegions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupRegions;
		private System.Windows.Forms.Button buttonDelete;
		private System.Windows.Forms.Button buttonSave;
		private WindowsFormsAero.ComboBox comboRegions;
		private System.Windows.Forms.Button buttonDone;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.Label labelHeight;
		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.Label labelY;
		private System.Windows.Forms.Label labelX;
		private System.Windows.Forms.Label labelCurrentRegion;
		private System.Windows.Forms.NumericUpDown numH;
		private System.Windows.Forms.NumericUpDown numW;
		private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numX;
        private FocusedTextBox textRegionName;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox checkRelative;
	}
}
