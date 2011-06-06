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
            this.labelVersion = new WindowsFormsAero.ThemeText.ThemedLabel();
            this.panelContents = new OnTopReplica.SidePanels.AboutPanelContents();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVersion.Location = new System.Drawing.Point(0, 270);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.labelVersion.Size = new System.Drawing.Size(265, 20);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "Version";
            this.labelVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.labelVersion.TextAlignVertical = System.Windows.Forms.VisualStyles.VerticalAlignment.Bottom;
            // 
            // panelContents
            // 
            this.panelContents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContents.AutoScroll = true;
            this.panelContents.Location = new System.Drawing.Point(6, 6);
            this.panelContents.Margin = new System.Windows.Forms.Padding(6);
            this.panelContents.Name = "panelContents";
            this.panelContents.Size = new System.Drawing.Size(253, 255);
            this.panelContents.TabIndex = 1;
            // 
            // AboutPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContents);
            this.Controls.Add(this.labelVersion);
            this.MinimumSize = new System.Drawing.Size(265, 290);
            this.Name = "AboutPanel";
            this.Size = new System.Drawing.Size(265, 290);
            this.ResumeLayout(false);

        }

        #endregion

        private WindowsFormsAero.ThemeText.ThemedLabel labelVersion;
        private AboutPanelContents panelContents;

    }
}
