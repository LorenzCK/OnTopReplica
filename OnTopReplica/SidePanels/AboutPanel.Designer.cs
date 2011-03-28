namespace OnTopReplica.SidePanels {
    partial class AboutPanel {
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.labelTranslators = new System.Windows.Forms.Label();
            this.linkCredits = new System.Windows.Forms.LinkLabel();
            this.progressUpdate = new VistaControls.ProgressBar();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.lblUpdateDisclaimer = new System.Windows.Forms.Label();
            this.labeledDivider2 = new VistaControls.LabeledDivider();
            this.labeledDivider1 = new VistaControls.LabeledDivider();
            this.linkHomepage = new System.Windows.Forms.LinkLabel();
            this.linkAuthor = new System.Windows.Forms.LinkLabel();
            this.lblSlogan = new System.Windows.Forms.Label();
            this.thlabelVersion = new VistaControls.ThemeText.ThemedLabel();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.AutoScroll = true;
            this.panelMain.Controls.Add(this.labelTranslators);
            this.panelMain.Controls.Add(this.linkCredits);
            this.panelMain.Controls.Add(this.progressUpdate);
            this.panelMain.Controls.Add(this.buttonUpdate);
            this.panelMain.Controls.Add(this.lblUpdateDisclaimer);
            this.panelMain.Controls.Add(this.labeledDivider2);
            this.panelMain.Controls.Add(this.labeledDivider1);
            this.panelMain.Controls.Add(this.linkHomepage);
            this.panelMain.Controls.Add(this.linkAuthor);
            this.panelMain.Controls.Add(this.lblSlogan);
            this.panelMain.Location = new System.Drawing.Point(6, 6);
            this.panelMain.Margin = new System.Windows.Forms.Padding(6);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(243, 258);
            this.panelMain.TabIndex = 0;
            // 
            // labelTranslators
            // 
            this.labelTranslators.Location = new System.Drawing.Point(0, 289);
            this.labelTranslators.Name = "labelTranslators";
            this.labelTranslators.Size = new System.Drawing.Size(226, 61);
            this.labelTranslators.TabIndex = 21;
            this.labelTranslators.Text = "Translators: asdasd";
            // 
            // linkCredits
            // 
            this.linkCredits.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkCredits.Location = new System.Drawing.Point(0, 223);
            this.linkCredits.Name = "linkCredits";
            this.linkCredits.Size = new System.Drawing.Size(226, 57);
            this.linkCredits.TabIndex = 20;
            this.linkCredits.TabStop = true;
            this.linkCredits.Text = "%CREDITS%";
            this.linkCredits.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkCredits_click);
            // 
            // progressUpdate
            // 
            this.progressUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressUpdate.Location = new System.Drawing.Point(0, 155);
            this.progressUpdate.Name = "progressUpdate";
            this.progressUpdate.Size = new System.Drawing.Size(118, 23);
            this.progressUpdate.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressUpdate.TabIndex = 19;
            this.progressUpdate.Visible = false;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdate.Location = new System.Drawing.Point(124, 155);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(102, 23);
            this.buttonUpdate.TabIndex = 18;
            this.buttonUpdate.Text = global::OnTopReplica.Strings.AboutUpdatesCheckNow;
            this.buttonUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(ButtonUpdate_click);
            // 
            // lblUpdateDisclaimer
            // 
            this.lblUpdateDisclaimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdateDisclaimer.Location = new System.Drawing.Point(0, 108);
            this.lblUpdateDisclaimer.Name = "lblUpdateDisclaimer";
            this.lblUpdateDisclaimer.Size = new System.Drawing.Size(225, 44);
            this.lblUpdateDisclaimer.TabIndex = 16;
            this.lblUpdateDisclaimer.Text = Strings.AboutUpdatesDisclaimer;
            // 
            // labeledDivider2
            // 
            this.labeledDivider2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labeledDivider2.DividerColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(191)))), ((int)(((byte)(222)))));
            this.labeledDivider2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labeledDivider2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(170)))));
            this.labeledDivider2.Location = new System.Drawing.Point(0, 197);
            this.labeledDivider2.Name = "labeledDivider2";
            this.labeledDivider2.Size = new System.Drawing.Size(223, 23);
            this.labeledDivider2.TabIndex = 15;
            this.labeledDivider2.Text = global::OnTopReplica.Strings.AboutDividerCredits;
            // 
            // labeledDivider1
            // 
            this.labeledDivider1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labeledDivider1.DividerColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(191)))), ((int)(((byte)(222)))));
            this.labeledDivider1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labeledDivider1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(170)))));
            this.labeledDivider1.Location = new System.Drawing.Point(0, 82);
            this.labeledDivider1.Name = "labeledDivider1";
            this.labeledDivider1.Size = new System.Drawing.Size(226, 23);
            this.labeledDivider1.TabIndex = 14;
            this.labeledDivider1.Text = global::OnTopReplica.Strings.AboutDividerUpdates;
            // 
            // linkHomepage
            // 
            this.linkHomepage.AutoSize = true;
            this.linkHomepage.BackColor = System.Drawing.Color.Transparent;
            this.linkHomepage.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkHomepage.Location = new System.Drawing.Point(0, 54);
            this.linkHomepage.Name = "linkHomepage";
            this.linkHomepage.Size = new System.Drawing.Size(167, 17);
            this.linkHomepage.TabIndex = 12;
            this.linkHomepage.TabStop = true;
            this.linkHomepage.Text = "http://ontopreplica.codeplex.com";
            this.linkHomepage.UseCompatibleTextRendering = true;
            this.linkHomepage.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkHomepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkWebsite_click);
            // 
            // linkAuthor
            // 
            this.linkAuthor.AutoSize = true;
            this.linkAuthor.BackColor = System.Drawing.Color.Transparent;
            this.linkAuthor.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkAuthor.LinkColor = System.Drawing.Color.Blue;
            this.linkAuthor.Location = new System.Drawing.Point(0, 33);
            this.linkAuthor.Name = "linkAuthor";
            this.linkAuthor.Size = new System.Drawing.Size(72, 17);
            this.linkAuthor.TabIndex = 11;
            this.linkAuthor.TabStop = true;
            this.linkAuthor.Text = "%AUTHOR%";
            this.linkAuthor.UseCompatibleTextRendering = true;
            this.linkAuthor.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkAuthor_click);
            // 
            // lblSlogan
            // 
            this.lblSlogan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSlogan.BackColor = System.Drawing.Color.Transparent;
            this.lblSlogan.Location = new System.Drawing.Point(0, 0);
            this.lblSlogan.Name = "lblSlogan";
            this.lblSlogan.Size = new System.Drawing.Size(226, 33);
            this.lblSlogan.TabIndex = 13;
            this.lblSlogan.Text = Strings.AboutSlogan;
            // 
            // thlabelVersion
            // 
            this.thlabelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.thlabelVersion.GlowSize = 7;
            this.thlabelVersion.Location = new System.Drawing.Point(0, 272);
            this.thlabelVersion.Margin = new System.Windows.Forms.Padding(0);
            this.thlabelVersion.Name = "thlabelVersion";
            this.thlabelVersion.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.thlabelVersion.Size = new System.Drawing.Size(255, 18);
            this.thlabelVersion.TabIndex = 1;
            this.thlabelVersion.Text = "%VERSION%";
            this.thlabelVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.thlabelVersion.TextAlignVertical = System.Windows.Forms.VisualStyles.VerticalAlignment.Bottom;
            // 
            // AboutPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.thlabelVersion);
            this.Controls.Add(this.panelMain);
            this.MinimumSize = new System.Drawing.Size(255, 290);
            this.Name = "AboutPanel";
            this.Size = new System.Drawing.Size(255, 290);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private VistaControls.ThemeText.ThemedLabel thlabelVersion;
        private System.Windows.Forms.LinkLabel linkHomepage;
        private System.Windows.Forms.LinkLabel linkAuthor;
        private System.Windows.Forms.Label lblSlogan;
        private VistaControls.LabeledDivider labeledDivider1;
        private System.Windows.Forms.Label lblUpdateDisclaimer;
        private VistaControls.LabeledDivider labeledDivider2;
        private VistaControls.ProgressBar progressUpdate;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Label labelTranslators;
        private System.Windows.Forms.LinkLabel linkCredits;
    }
}
