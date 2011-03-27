namespace OnTopReplica.SidePanels {
    partial class OptionsPanel {
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
            this.btnClose = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.groupHotkeys = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHotKeyShowHide = new System.Windows.Forms.Label();
            this.txtHotKeyShowHide = new OnTopReplica.HotKeyTextBox();
            this.lblHotKeyClone = new System.Windows.Forms.Label();
            this.txtHotKeyClone = new OnTopReplica.HotKeyTextBox();
            this.groupLanguage = new System.Windows.Forms.GroupBox();
            this.comboLanguage = new OnTopReplica.ImageComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.groupHotkeys.SuspendLayout();
            this.groupLanguage.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(185, 217);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = Strings.MenuClose;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.Close_click);
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.AutoScroll = true;
            this.panelMain.Controls.Add(this.groupHotkeys);
            this.panelMain.Controls.Add(this.groupLanguage);
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(260, 211);
            this.panelMain.TabIndex = 1;
            // 
            // groupHotkeys
            // 
            this.groupHotkeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupHotkeys.Controls.Add(this.label1);
            this.groupHotkeys.Controls.Add(this.lblHotKeyShowHide);
            this.groupHotkeys.Controls.Add(this.txtHotKeyShowHide);
            this.groupHotkeys.Controls.Add(this.lblHotKeyClone);
            this.groupHotkeys.Controls.Add(this.txtHotKeyClone);
            this.groupHotkeys.Location = new System.Drawing.Point(3, 77);
            this.groupHotkeys.Name = "groupHotkeys";
            this.groupHotkeys.Size = new System.Drawing.Size(254, 113);
            this.groupHotkeys.TabIndex = 1;
            this.groupHotkeys.TabStop = false;
            this.groupHotkeys.Text = Strings.SettingsHotKeyTitle;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(6, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 43);
            this.label1.TabIndex = 4;
            this.label1.Text = Strings.SettingsHotKeyDescription;
            // 
            // lblHotKeyShowHide
            // 
            this.lblHotKeyShowHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHotKeyShowHide.Location = new System.Drawing.Point(170, 22);
            this.lblHotKeyShowHide.Name = "lblHotKeyShowHide";
            this.lblHotKeyShowHide.Size = new System.Drawing.Size(78, 17);
            this.lblHotKeyShowHide.TabIndex = 3;
            this.lblHotKeyShowHide.Text = Strings.SettingsHotKeyShowHide;
            // 
            // txtHotKeyShowHide
            // 
            this.txtHotKeyShowHide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHotKeyShowHide.Location = new System.Drawing.Point(6, 19);
            this.txtHotKeyShowHide.Name = "txtHotKeyShowHide";
            this.txtHotKeyShowHide.ReadOnly = true;
            this.txtHotKeyShowHide.Size = new System.Drawing.Size(158, 20);
            this.txtHotKeyShowHide.TabIndex = 2;
            // 
            // lblHotKeyClone
            // 
            this.lblHotKeyClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHotKeyClone.Location = new System.Drawing.Point(170, 48);
            this.lblHotKeyClone.Name = "lblHotKeyClone";
            this.lblHotKeyClone.Size = new System.Drawing.Size(78, 29);
            this.lblHotKeyClone.TabIndex = 1;
            this.lblHotKeyClone.Text = Strings.SettingsHotKeyClone;
            // 
            // txtHotKeyClone
            // 
            this.txtHotKeyClone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHotKeyClone.Location = new System.Drawing.Point(6, 45);
            this.txtHotKeyClone.Name = "txtHotKeyClone";
            this.txtHotKeyClone.ReadOnly = true;
            this.txtHotKeyClone.Size = new System.Drawing.Size(158, 20);
            this.txtHotKeyClone.TabIndex = 0;
            // 
            // groupLanguage
            // 
            this.groupLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLanguage.Controls.Add(this.comboLanguage);
            this.groupLanguage.Controls.Add(this.lblLanguage);
            this.groupLanguage.Location = new System.Drawing.Point(3, 3);
            this.groupLanguage.Name = "groupLanguage";
            this.groupLanguage.Size = new System.Drawing.Size(254, 68);
            this.groupLanguage.TabIndex = 0;
            this.groupLanguage.TabStop = false;
            this.groupLanguage.Text = Strings.SettingsLanguageTitle;
            // 
            // comboLanguage
            // 
            this.comboLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLanguage.FormattingEnabled = true;
            this.comboLanguage.IconList = null;
            this.comboLanguage.Location = new System.Drawing.Point(9, 19);
            this.comboLanguage.Name = "comboLanguage";
            this.comboLanguage.Size = new System.Drawing.Size(239, 21);
            this.comboLanguage.TabIndex = 2;
            this.comboLanguage.SelectedIndexChanged += new System.EventHandler(this.LanguageBox_IndexChange);
            // 
            // lblLanguage
            // 
            this.lblLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLanguage.Location = new System.Drawing.Point(6, 43);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(242, 22);
            this.lblLanguage.TabIndex = 1;
            this.lblLanguage.Text = Strings.SettingsRestartRequired;
            // 
            // OptionsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.btnClose);
            this.MinimumSize = new System.Drawing.Size(260, 240);
            this.Name = "OptionsPanel";
            this.Size = new System.Drawing.Size(260, 240);
            this.panelMain.ResumeLayout(false);
            this.groupHotkeys.ResumeLayout(false);
            this.groupHotkeys.PerformLayout();
            this.groupLanguage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.GroupBox groupLanguage;
        private System.Windows.Forms.Label lblLanguage;
        private ImageComboBox comboLanguage;
        private System.Windows.Forms.GroupBox groupHotkeys;
        private HotKeyTextBox txtHotKeyClone;
        private System.Windows.Forms.Label lblHotKeyShowHide;
        private HotKeyTextBox txtHotKeyShowHide;
        private System.Windows.Forms.Label lblHotKeyClone;
        private System.Windows.Forms.Label label1;
    }
}
