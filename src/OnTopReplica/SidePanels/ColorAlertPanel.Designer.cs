namespace OnTopReplica.SidePanels {
    partial class ColorAlertPanel {
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
            this.groupColor = new System.Windows.Forms.GroupBox();
            this.checkEnabled = new System.Windows.Forms.CheckBox();
            this.labelColorSelection = new System.Windows.Forms.Label();
            this.checkRed = new System.Windows.Forms.CheckBox();
            this.checkOrange = new System.Windows.Forms.CheckBox();
            this.checkGray = new System.Windows.Forms.CheckBox();
            this.labelInterval = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.labelIntervalUnit = new System.Windows.Forms.Label();
            this.labelVolume = new System.Windows.Forms.Label();
            this.trackBarVolume = new System.Windows.Forms.TrackBar();
            this.labelSoundFile = new System.Windows.Forms.Label();
            this.comboSound = new System.Windows.Forms.ComboBox();
            this.btnTestAlarm = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tooltipInfo = new System.Windows.Forms.ToolTip(this.components);

            this.groupColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            this.SuspendLayout();

            // groupColor
            this.groupColor.Controls.Add(this.checkEnabled);
            this.groupColor.Controls.Add(this.labelColorSelection);
            this.groupColor.Controls.Add(this.checkRed);
            this.groupColor.Controls.Add(this.checkOrange);
            this.groupColor.Controls.Add(this.checkGray);
            this.groupColor.Controls.Add(this.labelInterval);
            this.groupColor.Controls.Add(this.numInterval);
            this.groupColor.Controls.Add(this.labelIntervalUnit);
            this.groupColor.Controls.Add(this.labelVolume);
            this.groupColor.Controls.Add(this.trackBarVolume);
            this.groupColor.Controls.Add(this.labelSoundFile);
            this.groupColor.Controls.Add(this.comboSound);
            this.groupColor.Controls.Add(this.btnTestAlarm);
            this.groupColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupColor.Location = new System.Drawing.Point(0, 0);
            this.groupColor.Name = "groupColor";
            this.groupColor.Padding = new System.Windows.Forms.Padding(10);
            this.groupColor.Size = new System.Drawing.Size(350, 310);
            this.groupColor.TabIndex = 0;
            this.groupColor.TabStop = false;
            this.groupColor.Text = "Color Alert";

            // checkEnabled
            this.checkEnabled.AutoSize = true;
            this.checkEnabled.Location = new System.Drawing.Point(13, 22);
            this.checkEnabled.Name = "checkEnabled";
            this.checkEnabled.Size = new System.Drawing.Size(150, 17);
            this.checkEnabled.TabIndex = 0;
            this.checkEnabled.Text = "Enable Color Detection";
            this.checkEnabled.UseVisualStyleBackColor = true;
            this.checkEnabled.CheckedChanged += new System.EventHandler(this.CheckEnabled_CheckedChanged);

            // labelColorSelection
            this.labelColorSelection.AutoSize = true;
            this.labelColorSelection.Location = new System.Drawing.Point(13, 50);
            this.labelColorSelection.Name = "labelColorSelection";
            this.labelColorSelection.Size = new System.Drawing.Size(100, 13);
            this.labelColorSelection.TabIndex = 1;
            this.labelColorSelection.Text = "检测颜色:";

            // checkRed
            this.checkRed.AutoSize = true;
            this.checkRed.Location = new System.Drawing.Point(30, 70);
            this.checkRed.Name = "checkRed";
            this.checkRed.Size = new System.Drawing.Size(80, 17);
            this.checkRed.TabIndex = 2;
            this.checkRed.Text = "红色 (Red)";
            this.checkRed.ForeColor = System.Drawing.Color.Red;
            this.checkRed.Checked = true;
            this.checkRed.UseVisualStyleBackColor = true;
            this.checkRed.CheckedChanged += new System.EventHandler(this.CheckColor_CheckedChanged);

            // checkOrange
            this.checkOrange.AutoSize = true;
            this.checkOrange.Location = new System.Drawing.Point(30, 93);
            this.checkOrange.Name = "checkOrange";
            this.checkOrange.Size = new System.Drawing.Size(100, 17);
            this.checkOrange.TabIndex = 3;
            this.checkOrange.Text = "橙色 (Orange)";
            this.checkOrange.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0);
            this.checkOrange.UseVisualStyleBackColor = true;
            this.checkOrange.CheckedChanged += new System.EventHandler(this.CheckColor_CheckedChanged);

            // checkGray
            this.checkGray.AutoSize = true;
            this.checkGray.Location = new System.Drawing.Point(30, 116);
            this.checkGray.Name = "checkGray";
            this.checkGray.Size = new System.Drawing.Size(80, 17);
            this.checkGray.TabIndex = 4;
            this.checkGray.Text = "灰色 (Gray)";
            this.checkGray.ForeColor = System.Drawing.Color.Gray;
            this.checkGray.UseVisualStyleBackColor = true;
            this.checkGray.CheckedChanged += new System.EventHandler(this.CheckColor_CheckedChanged);

            // labelInterval
            this.labelInterval.AutoSize = true;
            this.labelInterval.Location = new System.Drawing.Point(13, 148);
            this.labelInterval.Name = "labelInterval";
            this.labelInterval.Size = new System.Drawing.Size(125, 13);
            this.labelInterval.TabIndex = 5;
            this.labelInterval.Text = "Sample Interval (ms):";

            // numInterval
            this.numInterval.Location = new System.Drawing.Point(150, 145);
            this.numInterval.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.numInterval.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(80, 20);
            this.numInterval.TabIndex = 6;
            this.numInterval.Value = new decimal(new int[] { 500, 0, 0, 0 });
            this.numInterval.ValueChanged += new System.EventHandler(this.NumInterval_ValueChanged);

            // labelIntervalUnit
            this.labelIntervalUnit.AutoSize = true;
            this.labelIntervalUnit.Location = new System.Drawing.Point(235, 148);
            this.labelIntervalUnit.Name = "labelIntervalUnit";
            this.labelIntervalUnit.Size = new System.Drawing.Size(98, 13);
            this.labelIntervalUnit.TabIndex = 7;
            this.labelIntervalUnit.Text = "(100-10000 range)";

            // labelVolume
            this.labelVolume.AutoSize = true;
            this.labelVolume.Location = new System.Drawing.Point(13, 180);
            this.labelVolume.Name = "labelVolume";
            this.labelVolume.Size = new System.Drawing.Size(78, 13);
            this.labelVolume.TabIndex = 8;
            this.labelVolume.Text = "Alarm Volume:";

            // trackBarVolume
            this.trackBarVolume.Location = new System.Drawing.Point(100, 175);
            this.trackBarVolume.Maximum = 100;
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Size = new System.Drawing.Size(223, 45);
            this.trackBarVolume.TabIndex = 9;
            this.trackBarVolume.Value = 100;
            this.trackBarVolume.Scroll += new System.EventHandler(this.TrackBarVolume_Scroll);

            // labelSoundFile
            this.labelSoundFile.AutoSize = true;
            this.labelSoundFile.Location = new System.Drawing.Point(13, 220);
            this.labelSoundFile.Name = "labelSoundFile";
            this.labelSoundFile.Size = new System.Drawing.Size(62, 13);
            this.labelSoundFile.TabIndex = 10;
            this.labelSoundFile.Text = "Sound file:";

            // comboSound
            this.comboSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSound.FormattingEnabled = true;
            this.comboSound.Location = new System.Drawing.Point(100, 216);
            this.comboSound.Name = "comboSound";
            this.comboSound.Size = new System.Drawing.Size(223, 21);
            this.comboSound.TabIndex = 11;
            this.comboSound.SelectedIndexChanged += new System.EventHandler(this.ComboSound_SelectedIndexChanged);

            // btnTestAlarm
            this.btnTestAlarm.Location = new System.Drawing.Point(100, 250);
            this.btnTestAlarm.Name = "btnTestAlarm";
            this.btnTestAlarm.Size = new System.Drawing.Size(100, 25);
            this.btnTestAlarm.TabIndex = 12;
            this.btnTestAlarm.Text = "测试报警";
            this.btnTestAlarm.UseVisualStyleBackColor = true;
            this.btnTestAlarm.Click += new System.EventHandler(this.BtnTestAlarm_Click);

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(135, 320);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 25);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);

            // ColorAlertPanel
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupColor);
            this.Controls.Add(this.btnClose);
            this.Name = "ColorAlertPanel";
            this.Size = new System.Drawing.Size(350, 360);
            this.MinimumSize = new System.Drawing.Size(350, 360);

            this.groupColor.ResumeLayout(false);
            this.groupColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupColor;
        private System.Windows.Forms.CheckBox checkEnabled;
        private System.Windows.Forms.Label labelColorSelection;
        private System.Windows.Forms.CheckBox checkRed;
        private System.Windows.Forms.CheckBox checkOrange;
        private System.Windows.Forms.CheckBox checkGray;
        private System.Windows.Forms.Label labelInterval;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.Label labelIntervalUnit;
        private System.Windows.Forms.Label labelVolume;
        private System.Windows.Forms.TrackBar trackBarVolume;
        private System.Windows.Forms.Label labelSoundFile;
        private System.Windows.Forms.ComboBox comboSound;
        private System.Windows.Forms.Button btnTestAlarm;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip tooltipInfo;
    }
}
