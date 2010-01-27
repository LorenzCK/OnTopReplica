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
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuModeStandard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuModeOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuModeClickThrough = new System.Windows.Forms.ToolStripMenuItem();
            this.opacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpacity100 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpacity75 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpacity50 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpacity25 = new System.Windows.Forms.ToolStripMenuItem();
            this.quitFullscreenModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._thumbnail = new OnTopReplica.ThumbnailPanel();
            this.menuContext.SuspendLayout();
            this.menuWindows.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuContext
            // 
            this.menuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowsToolStripMenuItem,
            this.modeToolStripMenuItem,
            this.opacityToolStripMenuItem,
            this.quitFullscreenModeToolStripMenuItem});
            this.menuContext.Name = "contextMenuStrip1";
            this.menuContext.Size = new System.Drawing.Size(186, 114);
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
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuModeStandard,
            this.menuModeOnTop,
            this.menuModeClickThrough});
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.modeToolStripMenuItem.Text = Strings.FullscreenMode;
            this.modeToolStripMenuItem.DropDownOpening += new System.EventHandler(this.Menu_Mode_opening);
            // 
            // menuModeStandard
            // 
            this.menuModeStandard.Name = "menuModeStandard";
            this.menuModeStandard.Size = new System.Drawing.Size(152, 22);
            this.menuModeStandard.Text = Strings.FullscreenModeNormal;
            this.menuModeStandard.ToolTipText = Strings.FullscreenModeNormalTT;
            this.menuModeStandard.Click += new System.EventHandler(this.Menu_Mode_standard);
            // 
            // menuModeOnTop
            // 
            this.menuModeOnTop.Name = "menuModeOnTop";
            this.menuModeOnTop.Size = new System.Drawing.Size(152, 22);
            this.menuModeOnTop.Text = Strings.FullscreenModeAlwaysOnTop;
            this.menuModeOnTop.ToolTipText = Strings.FullscreenModeAlwaysOnTopTT;
            this.menuModeOnTop.Click += new System.EventHandler(this.Menu_Mode_ontop);
            // 
            // menuModeClickThrough
            // 
            this.menuModeClickThrough.Name = "menuModeClickThrough";
            this.menuModeClickThrough.Size = new System.Drawing.Size(152, 22);
            this.menuModeClickThrough.Text = Strings.FullscreenModeClickThrough;
            this.menuModeClickThrough.ToolTipText = Strings.FullscreenModeClickThroughTT;
            this.menuModeClickThrough.Click += new System.EventHandler(this.Menu_Mode_clickthrough);
            // 
            // opacityToolStripMenuItem
            // 
            this.opacityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpacity100,
            this.menuOpacity75,
            this.menuOpacity50,
            this.menuOpacity25});
            this.opacityToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.window_opacity16;
            this.opacityToolStripMenuItem.Name = "opacityToolStripMenuItem";
            this.opacityToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.opacityToolStripMenuItem.Text = Strings.MenuOpacity;
            this.opacityToolStripMenuItem.DropDownOpening += new System.EventHandler(this.Menu_Opacity_opening);
            // 
            // menuOpacity100
            // 
            this.menuOpacity100.Name = "menuOpacity100";
            this.menuOpacity100.Size = new System.Drawing.Size(153, 22);
            this.menuOpacity100.Text = Strings.MenuOp100;
            this.menuOpacity100.ToolTipText = Strings.MenuOp100TT;
            this.menuOpacity100.Click += new System.EventHandler(this.Menu_Opacity_100);
            // 
            // menuOpacity75
            // 
            this.menuOpacity75.Name = "menuOpacity75";
            this.menuOpacity75.Size = new System.Drawing.Size(153, 22);
            this.menuOpacity75.Text = Strings.MenuOp75;
            this.menuOpacity75.ToolTipText = Strings.MenuOp75TT;
            this.menuOpacity75.Click += new System.EventHandler(this.Menu_Opacity_75);
            // 
            // menuOpacity50
            // 
            this.menuOpacity50.Name = "menuOpacity50";
            this.menuOpacity50.Size = new System.Drawing.Size(153, 22);
            this.menuOpacity50.Text = Strings.MenuOp50;
            this.menuOpacity50.ToolTipText = Strings.MenuOp50TT;
            this.menuOpacity50.Click += new System.EventHandler(this.Menu_Opacity_50);
            // 
            // menuOpacity25
            // 
            this.menuOpacity25.Name = "menuOpacity25";
            this.menuOpacity25.Size = new System.Drawing.Size(153, 22);
            this.menuOpacity25.Text = Strings.MenuOp25;
            this.menuOpacity25.ToolTipText = Strings.MenuOp25TT;
            this.menuOpacity25.Click += new System.EventHandler(this.Menu_Opacity_25);
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
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuModeStandard;
        private System.Windows.Forms.ToolStripMenuItem menuModeOnTop;
        private System.Windows.Forms.ToolStripMenuItem menuModeClickThrough;
        private System.Windows.Forms.ToolStripMenuItem opacityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuOpacity100;
        private System.Windows.Forms.ToolStripMenuItem menuOpacity75;
        private System.Windows.Forms.ToolStripMenuItem menuOpacity50;
        private System.Windows.Forms.ToolStripMenuItem menuOpacity25;
	}
}