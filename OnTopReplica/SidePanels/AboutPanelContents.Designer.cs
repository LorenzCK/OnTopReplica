namespace OnTopReplica.SidePanels {
    partial class AboutPanelContents {
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
            this.labelTranslators = new System.Windows.Forms.Label();
            this.linkCredits = new System.Windows.Forms.LinkLabel();
            this.progressUpdate = new WindowsFormsAero.ProgressBar();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.lblUpdateDisclaimer = new System.Windows.Forms.Label();
            this.labeledDivider2 = new WindowsFormsAero.LabeledDivider();
            this.labeledDivider1 = new WindowsFormsAero.LabeledDivider();
            this.linkHomepage = new System.Windows.Forms.LinkLabel();
            this.linkAuthor = new System.Windows.Forms.LinkLabel();
            this.lblSlogan = new System.Windows.Forms.Label();
            this.labeledDivider3 = new WindowsFormsAero.LabeledDivider();
            this.linkLicense = new System.Windows.Forms.LinkLabel();
            this.labeledDivider4 = new WindowsFormsAero.LabeledDivider();
            this.linkContribute = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // labelTranslators
            // 
            this.labelTranslators.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTranslators.AutoEllipsis = true;
            this.labelTranslators.Location = new System.Drawing.Point(0, 289);
            this.labelTranslators.Name = "labelTranslators";
            this.labelTranslators.Size = new System.Drawing.Size(240, 101);
            this.labelTranslators.TabIndex = 31;
            this.labelTranslators.Text = "Translators:";
            // 
            // linkCredits
            // 
            this.linkCredits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.linkCredits.AutoEllipsis = true;
            this.linkCredits.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkCredits.Location = new System.Drawing.Point(0, 223);
            this.linkCredits.Name = "linkCredits";
            this.linkCredits.Size = new System.Drawing.Size(240, 57);
            this.linkCredits.TabIndex = 30;
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
            this.progressUpdate.Size = new System.Drawing.Size(132, 23);
            this.progressUpdate.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressUpdate.TabIndex = 29;
            this.progressUpdate.Visible = false;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdate.Location = new System.Drawing.Point(138, 155);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(102, 23);
            this.buttonUpdate.TabIndex = 28;
            this.buttonUpdate.Text = global::OnTopReplica.Strings.AboutUpdatesCheckNow;
            this.buttonUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.UpdateButton_click);
            // 
            // lblUpdateDisclaimer
            // 
            this.lblUpdateDisclaimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdateDisclaimer.AutoEllipsis = true;
            this.lblUpdateDisclaimer.Location = new System.Drawing.Point(0, 108);
            this.lblUpdateDisclaimer.Name = "lblUpdateDisclaimer";
            this.lblUpdateDisclaimer.Size = new System.Drawing.Size(240, 44);
            this.lblUpdateDisclaimer.TabIndex = 27;
            this.lblUpdateDisclaimer.Text = "OnTopReplica automatically checks for updates at every start up.";
            // 
            // labeledDivider2
            // 
            this.labeledDivider2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labeledDivider2.DividerColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(191)))), ((int)(((byte)(222)))));
            this.labeledDivider2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(170)))));
            this.labeledDivider2.Location = new System.Drawing.Point(0, 197);
            this.labeledDivider2.Name = "labeledDivider2";
            this.labeledDivider2.Size = new System.Drawing.Size(240, 23);
            this.labeledDivider2.TabIndex = 26;
            this.labeledDivider2.Text = global::OnTopReplica.Strings.AboutDividerCredits;
            // 
            // labeledDivider1
            // 
            this.labeledDivider1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labeledDivider1.DividerColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(191)))), ((int)(((byte)(222)))));
            this.labeledDivider1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(170)))));
            this.labeledDivider1.Location = new System.Drawing.Point(0, 82);
            this.labeledDivider1.Name = "labeledDivider1";
            this.labeledDivider1.Size = new System.Drawing.Size(240, 23);
            this.labeledDivider1.TabIndex = 25;
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
            this.linkHomepage.TabIndex = 23;
            this.linkHomepage.TabStop = true;
            this.linkHomepage.Text = "http://ontopreplica.codeplex.com";
            this.linkHomepage.UseCompatibleTextRendering = true;
            this.linkHomepage.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkHomepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkHomepage_clicked);
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
            this.linkAuthor.TabIndex = 22;
            this.linkAuthor.TabStop = true;
            this.linkAuthor.Text = "%AUTHOR%";
            this.linkAuthor.UseCompatibleTextRendering = true;
            this.linkAuthor.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkAuthor_clicked);
            // 
            // lblSlogan
            // 
            this.lblSlogan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSlogan.AutoEllipsis = true;
            this.lblSlogan.BackColor = System.Drawing.Color.Transparent;
            this.lblSlogan.Location = new System.Drawing.Point(0, 0);
            this.lblSlogan.Name = "lblSlogan";
            this.lblSlogan.Size = new System.Drawing.Size(240, 33);
            this.lblSlogan.TabIndex = 24;
            this.lblSlogan.Text = "A lightweight, real-time, always on top thumbnail of a window of your choice.";
            // 
            // labeledDivider3
            // 
            this.labeledDivider3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labeledDivider3.DividerColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(191)))), ((int)(((byte)(222)))));
            this.labeledDivider3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(170)))));
            this.labeledDivider3.Location = new System.Drawing.Point(0, 393);
            this.labeledDivider3.Name = "labeledDivider3";
            this.labeledDivider3.Size = new System.Drawing.Size(240, 23);
            this.labeledDivider3.TabIndex = 32;
            this.labeledDivider3.Text = "License";
            // 
            // linkLicense
            // 
            this.linkLicense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLicense.AutoEllipsis = true;
            this.linkLicense.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLicense.Location = new System.Drawing.Point(0, 419);
            this.linkLicense.Name = "linkLicense";
            this.linkLicense.Size = new System.Drawing.Size(240, 81);
            this.linkLicense.TabIndex = 33;
            this.linkLicense.TabStop = true;
            this.linkLicense.Text = "%LICENSE%";
            this.linkLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLicense_click);
            // 
            // labeledDivider4
            // 
            this.labeledDivider4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labeledDivider4.DividerColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(191)))), ((int)(((byte)(222)))));
            this.labeledDivider4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(170)))));
            this.labeledDivider4.Location = new System.Drawing.Point(0, 503);
            this.labeledDivider4.Name = "labeledDivider4";
            this.labeledDivider4.Size = new System.Drawing.Size(240, 23);
            this.labeledDivider4.TabIndex = 34;
            this.labeledDivider4.Text = "Contribute";
            // 
            // linkContribute
            // 
            this.linkContribute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.linkContribute.AutoEllipsis = true;
            this.linkContribute.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkContribute.Location = new System.Drawing.Point(0, 529);
            this.linkContribute.Name = "linkContribute";
            this.linkContribute.Size = new System.Drawing.Size(240, 84);
            this.linkContribute.TabIndex = 35;
            this.linkContribute.TabStop = true;
            this.linkContribute.Text = "%CONTRIBUTE%";
            this.linkContribute.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkContribute_clicked);
            // 
            // AboutPanelContents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.linkContribute);
            this.Controls.Add(this.labeledDivider4);
            this.Controls.Add(this.linkLicense);
            this.Controls.Add(this.labeledDivider3);
            this.Controls.Add(this.labelTranslators);
            this.Controls.Add(this.linkCredits);
            this.Controls.Add(this.progressUpdate);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.lblUpdateDisclaimer);
            this.Controls.Add(this.labeledDivider2);
            this.Controls.Add(this.labeledDivider1);
            this.Controls.Add(this.linkHomepage);
            this.Controls.Add(this.linkAuthor);
            this.Controls.Add(this.lblSlogan);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AboutPanelContents";
            this.Size = new System.Drawing.Size(243, 613);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTranslators;
        private System.Windows.Forms.LinkLabel linkCredits;
        private WindowsFormsAero.ProgressBar progressUpdate;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Label lblUpdateDisclaimer;
        private WindowsFormsAero.LabeledDivider labeledDivider2;
        private WindowsFormsAero.LabeledDivider labeledDivider1;
        private System.Windows.Forms.LinkLabel linkHomepage;
        private System.Windows.Forms.LinkLabel linkAuthor;
        private System.Windows.Forms.Label lblSlogan;
        private WindowsFormsAero.LabeledDivider labeledDivider3;
        private System.Windows.Forms.LinkLabel linkLicense;
        private WindowsFormsAero.LabeledDivider labeledDivider4;
        private System.Windows.Forms.LinkLabel linkContribute;

    }
}
