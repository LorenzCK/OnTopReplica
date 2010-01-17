namespace OnTopReplica {
	partial class FullscreenForm {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FullscreenForm));
            this.menuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWindows = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitFullscreenModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._thumbnail = new OnTopReplica.ThumbnailPanel();
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContext.SuspendLayout();
            this.menuWindows.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuContext
            // 
            this.menuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowsToolStripMenuItem,
            this.alwaysOnTopToolStripMenuItem,
            this.quitFullscreenModeToolStripMenuItem});
            this.menuContext.Name = "contextMenuStrip1";
            this.menuContext.Size = new System.Drawing.Size(186, 92);
            this.menuContext.Opening += new System.ComponentModel.CancelEventHandler(this.Menu_opening);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDown = this.menuWindows;
            this.windowsToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.window_multiple16;
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.windowsToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuWindows;
            this.windowsToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuWindowsTT;
            this.windowsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.Menu_Windows_opening);
            // 
            // menuWindows
            // 
            this.menuWindows.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem});
            this.menuWindows.Name = "menuWindows";
            this.menuWindows.OwnerItem = this.windowsToolStripMenuItem;
            this.menuWindows.Size = new System.Drawing.Size(118, 26);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.noneToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuWindowsNone;
            // 
            // quitFullscreenModeToolStripMenuItem
            // 
            this.quitFullscreenModeToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.close_new;
            this.quitFullscreenModeToolStripMenuItem.Name = "quitFullscreenModeToolStripMenuItem";
            this.quitFullscreenModeToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.quitFullscreenModeToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuQuitFullscreen;
            this.quitFullscreenModeToolStripMenuItem.Click += new System.EventHandler(this.Menu_Quit_click);
            // 
            // _thumbnail
            // 
            this._thumbnail.BackColor = System.Drawing.SystemColors.Control;
            this._thumbnail.ClickThrough = true;
            this._thumbnail.Cursor = System.Windows.Forms.Cursors.Default;
            this._thumbnail.Dock = System.Windows.Forms.DockStyle.Fill;
            this._thumbnail.DrawMouseRegions = false;
            this._thumbnail.FullscreenMode = false;
            this._thumbnail.GlassMode = false;
            this._thumbnail.Location = new System.Drawing.Point(0, 0);
            this._thumbnail.Name = "_thumbnail";
            this._thumbnail.ShownRegion = new System.Drawing.Rectangle(0, 0, 0, 0);
            this._thumbnail.ShowRegion = false;
            this._thumbnail.Size = new System.Drawing.Size(284, 264);
            this._thumbnail.TabIndex = 0;
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.alwaysOnTopToolStripMenuItem.Text = Strings.FullscreenAlwaysOnTop;
            this.alwaysOnTopToolStripMenuItem.Click += new System.EventHandler(this.Menu_AlwaysOnTop_click);
            // 
            // FullscreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.ContextMenuStrip = this.menuContext;
            this.Controls.Add(this._thumbnail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FullscreenForm";
            this.Text = Strings.FullscreenTitle;
            this.TransparencyKey = System.Drawing.Color.Black;
            this.menuContext.ResumeLayout(false);
            this.menuWindows.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private ThumbnailPanel _thumbnail;
		private System.Windows.Forms.ContextMenuStrip menuContext;
		private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem quitFullscreenModeToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip menuWindows;
		private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTopToolStripMenuItem;
	}
}