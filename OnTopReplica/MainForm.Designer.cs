namespace OnTopReplica
{
    partial class MainForm
    {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWindows = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchToWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectRegionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forwardClicksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextOpacity = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpacity = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuResize = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.doubleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fitToWindowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.halfToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.quarterToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.fullscreenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.recallLastPositionAndSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chromeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reduceToIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.italianoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIconContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContext.SuspendLayout();
            this.menuWindows.SuspendLayout();
            this.menuOpacity.SuspendLayout();
            this.menuResize.SuspendLayout();
            this.menuLanguages.SuspendLayout();
            this.menuIconContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuContext
            // 
            this.menuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextWindows,
            this.switchToWindowToolStripMenuItem,
            this.selectRegionToolStripMenuItem,
            this.forwardClicksToolStripMenuItem,
            this.menuContextOpacity,
            this.resizeToolStripMenuItem,
            this.dockToolStripMenuItem,
            this.chromeToolStripMenuItem,
            this.reduceToIconToolStripMenuItem,
            this.toolStripSeparator1,
            this.languageToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.menuContextClose});
            this.menuContext.Name = "menuContext";
            this.menuContext.Size = new System.Drawing.Size(169, 296);
            this.menuContext.Opening += new System.ComponentModel.CancelEventHandler(this.Menu_opening);
            // 
            // menuContextWindows
            // 
            this.menuContextWindows.DropDown = this.menuWindows;
            this.menuContextWindows.Image = global::OnTopReplica.Properties.Resources.window_multiple16;
            this.menuContextWindows.Name = "menuContextWindows";
            this.menuContextWindows.Size = new System.Drawing.Size(168, 22);
            this.menuContextWindows.Text = global::OnTopReplica.Strings.MenuWindows;
            this.menuContextWindows.ToolTipText = global::OnTopReplica.Strings.MenuWindowsTT;
            this.menuContextWindows.DropDownOpening += new System.EventHandler(this.Menu_Windows_opening);
            // 
            // menuWindows
            // 
            this.menuWindows.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem});
            this.menuWindows.Name = "menuWindows";
            this.menuWindows.OwnerItem = this.menuContextWindows;
            this.menuWindows.Size = new System.Drawing.Size(118, 26);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.noneToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuWindowsNone;
            // 
            // switchToWindowToolStripMenuItem
            // 
            this.switchToWindowToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.window_switch;
            this.switchToWindowToolStripMenuItem.Name = "switchToWindowToolStripMenuItem";
            this.switchToWindowToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.switchToWindowToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuSwitch;
            this.switchToWindowToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuSwitchTT;
            this.switchToWindowToolStripMenuItem.Click += new System.EventHandler(this.Menu_Switch_click);
            // 
            // selectRegionToolStripMenuItem
            // 
            this.selectRegionToolStripMenuItem.Enabled = false;
            this.selectRegionToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.regions;
            this.selectRegionToolStripMenuItem.Name = "selectRegionToolStripMenuItem";
            this.selectRegionToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.selectRegionToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuRegion;
            this.selectRegionToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuRegionTT;
            this.selectRegionToolStripMenuItem.Click += new System.EventHandler(this.Menu_Region_click);
            // 
            // forwardClicksToolStripMenuItem
            // 
            this.forwardClicksToolStripMenuItem.Name = "forwardClicksToolStripMenuItem";
            this.forwardClicksToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.forwardClicksToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuClickForwarding;
            this.forwardClicksToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuClickForwardingTT;
            this.forwardClicksToolStripMenuItem.Click += new System.EventHandler(this.Menu_Forward_click);
            // 
            // menuContextOpacity
            // 
            this.menuContextOpacity.DropDown = this.menuOpacity;
            this.menuContextOpacity.Image = global::OnTopReplica.Properties.Resources.window_opacity16;
            this.menuContextOpacity.Name = "menuContextOpacity";
            this.menuContextOpacity.Size = new System.Drawing.Size(168, 22);
            this.menuContextOpacity.Text = global::OnTopReplica.Strings.MenuOpacity;
            // 
            // menuOpacity
            // 
            this.menuOpacity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripSeparator2,
            this.toolStripMenuItem5});
            this.menuOpacity.Name = "menuOpacity";
            this.menuOpacity.OwnerItem = this.menuContextOpacity;
            this.menuOpacity.ShowCheckMargin = true;
            this.menuOpacity.ShowImageMargin = false;
            this.menuOpacity.Size = new System.Drawing.Size(154, 142);
            this.menuOpacity.Opening += new System.ComponentModel.CancelEventHandler(this.Menu_Opacity_opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Checked = true;
            this.toolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem1.Tag = 1.0;
            this.toolStripMenuItem1.Text = global::OnTopReplica.Strings.MenuOp100;
            this.toolStripMenuItem1.ToolTipText = global::OnTopReplica.Strings.MenuOp100TT;
            this.toolStripMenuItem1.Click += new System.EventHandler(this.Menu_Opacity_click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem2.Tag = 0.75;
            this.toolStripMenuItem2.Text = global::OnTopReplica.Strings.MenuOp75;
            this.toolStripMenuItem2.ToolTipText = global::OnTopReplica.Strings.MenuOp75TT;
            this.toolStripMenuItem2.Click += new System.EventHandler(this.Menu_Opacity_click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem3.Tag = 0.5;
            this.toolStripMenuItem3.Text = global::OnTopReplica.Strings.MenuOp50;
            this.toolStripMenuItem3.ToolTipText = global::OnTopReplica.Strings.MenuOp50TT;
            this.toolStripMenuItem3.Click += new System.EventHandler(this.Menu_Opacity_click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem4.Tag = 0.25;
            this.toolStripMenuItem4.Text = global::OnTopReplica.Strings.MenuOp25;
            this.toolStripMenuItem4.ToolTipText = global::OnTopReplica.Strings.MenuOp25TT;
            this.toolStripMenuItem4.Click += new System.EventHandler(this.Menu_Opacity_click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(150, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Checked = true;
            this.toolStripMenuItem5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem5.Text = global::OnTopReplica.Strings.MenuGlass;
            this.toolStripMenuItem5.ToolTipText = global::OnTopReplica.Strings.MenuGlassTT;
            this.toolStripMenuItem5.Click += new System.EventHandler(this.Menu_Opacity_Glass_click);
            // 
            // resizeToolStripMenuItem
            // 
            this.resizeToolStripMenuItem.DropDown = this.menuResize;
            this.resizeToolStripMenuItem.Enabled = false;
            this.resizeToolStripMenuItem.Name = "resizeToolStripMenuItem";
            this.resizeToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.resizeToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuResize;
            // 
            // menuResize
            // 
            this.menuResize.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.doubleToolStripMenuItem1,
            this.fitToWindowToolStripMenuItem1,
            this.halfToolStripMenuItem1,
            this.quarterToolStripMenuItem1,
            this.toolStripSeparator3,
            this.fullscreenToolStripMenuItem1});
            this.menuResize.Name = "menuResize";
            this.menuResize.Size = new System.Drawing.Size(165, 120);
            this.menuResize.Opening += new System.ComponentModel.CancelEventHandler(this.Menu_Resize_opening);
            // 
            // doubleToolStripMenuItem1
            // 
            this.doubleToolStripMenuItem1.Name = "doubleToolStripMenuItem1";
            this.doubleToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.doubleToolStripMenuItem1.Text = global::OnTopReplica.Strings.MenuFitDouble;
            this.doubleToolStripMenuItem1.Click += new System.EventHandler(this.Menu_Resize_Double);
            // 
            // fitToWindowToolStripMenuItem1
            // 
            this.fitToWindowToolStripMenuItem1.Name = "fitToWindowToolStripMenuItem1";
            this.fitToWindowToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.fitToWindowToolStripMenuItem1.Text = global::OnTopReplica.Strings.MenuFitOriginal;
            this.fitToWindowToolStripMenuItem1.Click += new System.EventHandler(this.Menu_Resize_FitToWindow);
            // 
            // halfToolStripMenuItem1
            // 
            this.halfToolStripMenuItem1.Name = "halfToolStripMenuItem1";
            this.halfToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.halfToolStripMenuItem1.Text = global::OnTopReplica.Strings.MenuFitHalf;
            this.halfToolStripMenuItem1.Click += new System.EventHandler(this.Menu_Resize_Half);
            // 
            // quarterToolStripMenuItem1
            // 
            this.quarterToolStripMenuItem1.Name = "quarterToolStripMenuItem1";
            this.quarterToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.quarterToolStripMenuItem1.Text = global::OnTopReplica.Strings.MenuFitQuarter;
            this.quarterToolStripMenuItem1.Click += new System.EventHandler(this.Menu_Resize_Quarter);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(161, 6);
            // 
            // fullscreenToolStripMenuItem1
            // 
            this.fullscreenToolStripMenuItem1.Name = "fullscreenToolStripMenuItem1";
            this.fullscreenToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.fullscreenToolStripMenuItem1.Text = global::OnTopReplica.Strings.MenuFitFullscreen;
            this.fullscreenToolStripMenuItem1.Click += new System.EventHandler(this.Menu_Resize_Fullscreen);
            // 
            // dockToolStripMenuItem
            // 
            this.dockToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.topLeftToolStripMenuItem,
            this.topRightToolStripMenuItem,
            this.bottomLeftToolStripMenuItem,
            this.bottomRightToolStripMenuItem,
            this.toolStripSeparator4,
            this.recallLastPositionAndSizeToolStripMenuItem});
            this.dockToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.pos_null;
            this.dockToolStripMenuItem.Name = "dockToolStripMenuItem";
            this.dockToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.dockToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuPosition;
            this.dockToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuPositionTT;
            // 
            // topLeftToolStripMenuItem
            // 
            this.topLeftToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.pos_topleft;
            this.topLeftToolStripMenuItem.Name = "topLeftToolStripMenuItem";
            this.topLeftToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.topLeftToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuPosTopLeft;
            this.topLeftToolStripMenuItem.Click += new System.EventHandler(this.Menu_Position_TopLeft);
            // 
            // topRightToolStripMenuItem
            // 
            this.topRightToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.pos_topright;
            this.topRightToolStripMenuItem.Name = "topRightToolStripMenuItem";
            this.topRightToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.topRightToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuPosTopRight;
            this.topRightToolStripMenuItem.Click += new System.EventHandler(this.Menu_Position_TopRight);
            // 
            // bottomLeftToolStripMenuItem
            // 
            this.bottomLeftToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.pos_bottomleft;
            this.bottomLeftToolStripMenuItem.Name = "bottomLeftToolStripMenuItem";
            this.bottomLeftToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.bottomLeftToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuPosBottomLeft;
            this.bottomLeftToolStripMenuItem.Click += new System.EventHandler(this.Menu_Position_BottomLeft);
            // 
            // bottomRightToolStripMenuItem
            // 
            this.bottomRightToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.pos_bottomright;
            this.bottomRightToolStripMenuItem.Name = "bottomRightToolStripMenuItem";
            this.bottomRightToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.bottomRightToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuPosBottomRight;
            this.bottomRightToolStripMenuItem.Click += new System.EventHandler(this.Menu_Position_BottomRight);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(214, 6);
            // 
            // recallLastPositionAndSizeToolStripMenuItem
            // 
            this.recallLastPositionAndSizeToolStripMenuItem.Name = "recallLastPositionAndSizeToolStripMenuItem";
            this.recallLastPositionAndSizeToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.recallLastPositionAndSizeToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuRecall;
            this.recallLastPositionAndSizeToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuRecallTT;
            this.recallLastPositionAndSizeToolStripMenuItem.Click += new System.EventHandler(this.Menu_Position_Recall_click);
            // 
            // chromeToolStripMenuItem
            // 
            this.chromeToolStripMenuItem.Name = "chromeToolStripMenuItem";
            this.chromeToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.chromeToolStripMenuItem.Text = Strings.MenuChrome;
            this.chromeToolStripMenuItem.ToolTipText = Strings.MenuChromeTT;
            this.chromeToolStripMenuItem.Click += new System.EventHandler(this.Menu_Chrome_click);
            // 
            // reduceToIconToolStripMenuItem
            // 
            this.reduceToIconToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.reduce;
            this.reduceToIconToolStripMenuItem.Name = "reduceToIconToolStripMenuItem";
            this.reduceToIconToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.reduceToIconToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuReduce;
            this.reduceToIconToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuReduceTT;
            this.reduceToIconToolStripMenuItem.Click += new System.EventHandler(this.Menu_Reduce_click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDown = this.menuLanguages;
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.languageToolStripMenuItem.Text = global::OnTopReplica.Strings.Language;
            // 
            // menuLanguages
            // 
            this.menuLanguages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.italianoToolStripMenuItem});
            this.menuLanguages.Name = "menuLanguages";
            this.menuLanguages.OwnerItem = this.languageToolStripMenuItem;
            this.menuLanguages.Size = new System.Drawing.Size(114, 48);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.flag_usa;
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.englishToolStripMenuItem.Tag = "en-US";
            this.englishToolStripMenuItem.Text = "English";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.Menu_Language_click);
            // 
            // italianoToolStripMenuItem
            // 
            this.italianoToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.flag_ita;
            this.italianoToolStripMenuItem.Name = "italianoToolStripMenuItem";
            this.italianoToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.italianoToolStripMenuItem.Tag = "it-IT";
            this.italianoToolStripMenuItem.Text = "Italiano";
            this.italianoToolStripMenuItem.Click += new System.EventHandler(this.Menu_Language_click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.aboutToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuAbout;
            this.aboutToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuAboutTT;
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.Menu_About_click);
            // 
            // menuContextClose
            // 
            this.menuContextClose.Image = global::OnTopReplica.Properties.Resources.close_new;
            this.menuContextClose.Name = "menuContextClose";
            this.menuContextClose.Size = new System.Drawing.Size(168, 22);
            this.menuContextClose.Text = global::OnTopReplica.Strings.MenuClose;
            this.menuContextClose.Click += new System.EventHandler(this.Menu_Close_click);
            // 
            // menuIconContext
            // 
            this.menuIconContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.resetWindowToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuIconContext.Name = "menuIconContext";
            this.menuIconContext.Size = new System.Drawing.Size(148, 70);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.icon;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.openToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuOpen;
            this.openToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuOpenTT;
            this.openToolStripMenuItem.Click += new System.EventHandler(this.IconContextOpen_click);
            // 
            // resetWindowToolStripMenuItem
            // 
            this.resetWindowToolStripMenuItem.Name = "resetWindowToolStripMenuItem";
            this.resetWindowToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.resetWindowToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuReset;
            this.resetWindowToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuResetTT;
            this.resetWindowToolStripMenuItem.Click += new System.EventHandler(this.IconContextReset_click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::OnTopReplica.Properties.Resources.close_new;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.exitToolStripMenuItem.Text = global::OnTopReplica.Strings.MenuClose;
            this.exitToolStripMenuItem.ToolTipText = global::OnTopReplica.Strings.MenuCloseTT;
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.IconContextExit_click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(264, 204);
            this.ContextMenuStrip = this.menuContext;
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(39, 40);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.DoubleClick += new System.EventHandler(this.Form_doubleclick);
            this.menuContext.ResumeLayout(false);
            this.menuWindows.ResumeLayout(false);
            this.menuOpacity.ResumeLayout(false);
            this.menuResize.ResumeLayout(false);
            this.menuLanguages.ResumeLayout(false);
            this.menuIconContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuContext;
        private System.Windows.Forms.ToolStripMenuItem menuContextWindows;
        private System.Windows.Forms.ToolStripMenuItem menuContextClose;
		private System.Windows.Forms.ContextMenuStrip menuWindows;
        private System.Windows.Forms.ToolStripMenuItem menuContextOpacity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ContextMenuStrip menuOpacity;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reduceToIconToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectRegionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
		private System.Windows.Forms.ContextMenuStrip menuIconContext;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resetWindowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem switchToWindowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dockToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem topLeftToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem topRightToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bottomLeftToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bottomRightToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem recallLastPositionAndSizeToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip menuResize;
		private System.Windows.Forms.ToolStripMenuItem doubleToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem fitToWindowToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem halfToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem quarterToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem forwardClicksToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip menuLanguages;
		private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem italianoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem chromeToolStripMenuItem;
    }
}

