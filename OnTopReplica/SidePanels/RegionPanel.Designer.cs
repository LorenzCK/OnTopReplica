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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textRegionName = new OnTopReplica.FocusedTextBox();
            this.numH = new System.Windows.Forms.NumericUpDown();
            this.numW = new System.Windows.Forms.NumericUpDown();
            this.numY = new System.Windows.Forms.NumericUpDown();
            this.numX = new System.Windows.Forms.NumericUpDown();
            this.buttonDone = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.comboRegions = new VistaControls.ComboBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textRegionName);
            this.groupBox1.Controls.Add(this.numH);
            this.groupBox1.Controls.Add(this.numW);
            this.groupBox1.Controls.Add(this.numY);
            this.groupBox1.Controls.Add(this.numX);
            this.groupBox1.Controls.Add(this.buttonDone);
            this.groupBox1.Controls.Add(this.buttonReset);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonDelete);
            this.groupBox1.Controls.Add(this.buttonSave);
            this.groupBox1.Controls.Add(this.comboRegions);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 158);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Regions:";
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
            this.numH.Enabled = false;
            this.numH.Location = new System.Drawing.Point(109, 93);
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
            this.numW.Enabled = false;
            this.numW.Location = new System.Drawing.Point(109, 67);
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
            this.numY.Location = new System.Drawing.Point(23, 93);
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
            this.numX.Location = new System.Drawing.Point(23, 67);
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
            this.buttonDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDone.Image = global::OnTopReplica.Properties.Resources.xiao_ok;
            this.buttonDone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonDone.Location = new System.Drawing.Point(142, 129);
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
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Location = new System.Drawing.Point(66, 129);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(70, 23);
            this.buttonReset.TabIndex = 8;
            this.buttonReset.Text = global::OnTopReplica.Strings.RegionsResetButton;
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.Reset_click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label6.Location = new System.Drawing.Point(71, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Height";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label7.Location = new System.Drawing.Point(71, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Location = new System.Drawing.Point(6, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Y";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label2.Location = new System.Drawing.Point(6, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "X";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current region:";
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
            this.buttonSave.UseVisualStyleBackColor = true;
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
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(230, 170);
            this.Name = "RegionPanel";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Size = new System.Drawing.Size(230, 170);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonDelete;
		private System.Windows.Forms.Button buttonSave;
		private VistaControls.ComboBox comboRegions;
		private System.Windows.Forms.Button buttonDone;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numH;
		private System.Windows.Forms.NumericUpDown numW;
		private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numX;
        private FocusedTextBox textRegionName;
	}
}
