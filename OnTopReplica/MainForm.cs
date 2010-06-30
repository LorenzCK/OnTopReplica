using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OnTopReplica.Properties;
using VistaControls.Dwm;
using VistaControls.TaskDialog;
using System.Collections.Generic;
using OnTopReplica.Native;

namespace OnTopReplica {

    partial class MainForm : AspectRatioForm {

        //GUI elements
        ThumbnailPanel _thumbnailPanel;
        SidePanel _currentSidePanel = null;
        Panel _sidePanelContainer;

        //Window manager
        WindowManager _windowManager = new WindowManager();
        WindowHandle _lastWindowHandle = null;

        //Message pump extension
        MessagePumpManager _msgPumpManager = new MessagePumpManager();

        public MainForm() {
            InitializeComponent();
            KeepAspectRatio = false;

            //Thumbnail panel
            _thumbnailPanel = new ThumbnailPanel {
                Location = Point.Empty,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Size = ClientSize
            };
            _thumbnailPanel.CloneClick += new EventHandler<CloneClickEventArgs>(Thumbnail_CloneClick);
            Controls.Add(_thumbnailPanel);

            //Side panel
            _sidePanelContainer = new Panel {
                Location = new Point(ClientSize.Width, 0),
                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                Enabled = false,
                Visible = false,
                Size = new Size(100, ClientSize.Height),
                Padding = new Padding(4)
            };
            Controls.Add(_sidePanelContainer);

            //Set native renderer on context menus
            Asztal.Szótár.NativeToolStripRenderer.SetToolStripRenderer(
                menuContext, menuWindows, menuOpacity, menuResize, menuLanguages
            );

            //Hook keyboard handler
            this.KeyUp += new KeyEventHandler(Form_KeyUp);
            this.KeyPreview = true;

            //Init message pump extensions
            _msgPumpManager.Initialize(this);

            //Add hotkeys
            var hotKeyMgr = _msgPumpManager.Get<MessagePumpProcessors.HotKeyManager>();
            hotKeyMgr.RegisterHotKey(Native.HotKeyModifiers.Control | Native.HotKeyModifiers.Shift,
                                     Keys.O, new Native.HotKeyMethods.HotKeyHandler(HotKeyOpenHandler));
        }

        #region Child forms & controls events

        EventHandler RequestClosingHandler;

        const int SidePanelMargin = 1;
        const int ScreenBorderMargin = 10;

        bool _sidePanelDidMoveForm = false;
        Point _sidePanelPreviousFormLocation;

        /// <summary>
        /// Opens a new side panel.
        /// </summary>
        /// <param name="panel">The side panel to embed.</param>
        public void SetSidePanel(SidePanel panel) {
            if (_currentSidePanel != null) {
                CloseSidePanel();
            }

            _currentSidePanel = panel;
            _currentSidePanel.OnFirstShown(this);

            //Add and show
            _sidePanelContainer.Controls.Add(panel);
            _sidePanelContainer.Visible = _sidePanelContainer.Enabled = true;
            panel.Show();

            int intHorzMargin = panel.Margin.Horizontal + _sidePanelContainer.Padding.Horizontal; //internal margins for sidepanel
            int intVertMargin = panel.Margin.Vertical + _sidePanelContainer.Padding.Vertical;

            //Resize container
            _sidePanelContainer.ClientSize = new Size(
                panel.MinimumSize.Width + intHorzMargin,
                ClientSize.Height
            );

            int rightHorzMargin = _sidePanelContainer.Width + SidePanelMargin; //horz margin between the two controls

            //Resize rest
            MinimumSize = new Size(
                20 + panel.MinimumSize.Width + SidePanelMargin + intHorzMargin,
                panel.MinimumSize.Height + intVertMargin
            );
            ClientSize = new Size(
                ClientSize.Width + rightHorzMargin,
                ClientSize.Height
            );
            ExtraPadding = new Padding(0, 0, rightHorzMargin, 0);
            _thumbnailPanel.Size = new Size(
                ClientSize.Width - rightHorzMargin,
                ClientSize.Height
            );
            _sidePanelContainer.Location = new Point(
                ClientSize.Width - rightHorzMargin,
                0
            );

            //Move window if needed
            var screenCurr = Screen.FromControl(this);
            int pRight = Location.X + Width + ScreenBorderMargin;
            if (pRight >= screenCurr.Bounds.Width) {
                _sidePanelPreviousFormLocation = Location;
                _sidePanelDidMoveForm = true;

                Location = new Point(screenCurr.WorkingArea.Width - Width - ScreenBorderMargin, Location.Y);
            }
            else {
                _sidePanelDidMoveForm = false;
            }

            //Hook event listener
            if (RequestClosingHandler == null)
                RequestClosingHandler = new EventHandler(SidePanel_RequestClosing);
            panel.RequestClosing += RequestClosingHandler;
        }

        /// <summary>
        /// Closes the current side panel.
        /// </summary>
        public void CloseSidePanel() {
            if (_currentSidePanel == null)
                return;

            //Unhook listener
            _currentSidePanel.RequestClosing -= RequestClosingHandler;

            //Remove side panel
            _currentSidePanel.OnClosing(this);
            _sidePanelContainer.Controls.Clear();
            _sidePanelContainer.Visible = _sidePanelContainer.Enabled = false;
            _currentSidePanel = null;

            //Resize
            MinimumSize = new Size(20, 20);
            ClientSize = new Size(
                ClientSize.Width - _sidePanelContainer.Width - SidePanelMargin,
                ClientSize.Height
            );
            ExtraPadding = new Padding(0);
            _thumbnailPanel.Size = ClientSize;

            //Move window back if needed
            if (_sidePanelDidMoveForm) {
                Location = _sidePanelPreviousFormLocation;
                _sidePanelDidMoveForm = false;
            }
        }

        void SidePanel_RequestClosing(object sender, EventArgs e) {
            CloseSidePanel();
        }

        void Thumbnail_CloneClick(object sender, CloneClickEventArgs e) {
            //TODO: handle other mouse buttons
            Win32Helper.InjectFakeMouseClick(_lastWindowHandle.Handle, e.ClientClickLocation, e.IsDoubleClick);
        }

        #endregion

        #region Event override

        protected override void OnShown(EventArgs e) {
            base.OnShown(e);

            //Platform specific form initialization
            Program.Platform.InitForm(this);

            //Glassify window
            GlassEnabled = true;
        }

        protected override void OnClosing(CancelEventArgs e) {
            _msgPumpManager.Dispose();

            base.OnClosing(e);
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            this.GlassMargins = (_currentSidePanel != null) ?
                new Margins(ClientSize.Width - _sidePanelContainer.Width, 0, 0, 0) :
                new Margins(-1);
        }

        protected override void OnDeactivate(EventArgs e) {
            base.OnDeactivate(e);

            //HACK: sometimes, even if TopMost is true, the window loses its "always on top" status.
            //  This is an attempt of a fix that probably won't work...
            if (!IsFullscreen) { //fullscreen mode doesn't use TopMost
                TopMost = true;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);

            int change = (int)(e.Delta / 6.0); //assumes a mouse wheel "tick" is in the 80-120 range
            AdjustSize(change);
        }

        protected override void WndProc(ref Message m) {
            _msgPumpManager.PumpMessage(m);

            switch (m.Msg) {
                case MessagingMethods.WM_NCRBUTTONUP:
                    //Open context menu if right button clicked on caption (i.e. all of the window area because of glass)
                    if (m.WParam.ToInt32() == MessagingMethods.HTCAPTION) {
                        menuContext.Show(MousePosition);

                        m.Result = IntPtr.Zero;
                        return;
                    }
                    break;

                case MessagingMethods.WM_NCLBUTTONDBLCLK:
                    //Toggle fullscreen mode if double click on caption (whole glass area)
                    if (m.WParam.ToInt32() == MessagingMethods.HTCAPTION) {
                        IsFullscreen = !IsFullscreen;

                        m.Result = IntPtr.Zero;
                        return;
                    }
                    break;
            }

            base.WndProc(ref m);
        }

        #endregion

        #region Menu Event Handling

        private void Close_click(object sender, EventArgs e) {
            this.Close();
        }

        private void Menu_opening(object sender, CancelEventArgs e) {
            //Cancel if currently in "fullscreen" mode or a side panel is open
            if (IsFullscreen || _currentSidePanel != null) {
                e.Cancel = true;
                return;
            }

            selectRegionToolStripMenuItem.Enabled = _thumbnailPanel.IsShowingThumbnail;
            switchToWindowToolStripMenuItem.Enabled = _thumbnailPanel.IsShowingThumbnail;
            resizeToolStripMenuItem.Enabled = _thumbnailPanel.IsShowingThumbnail;
            chromeToolStripMenuItem.Checked = (FormBorderStyle == FormBorderStyle.Sizable);
            forwardClicksToolStripMenuItem.Checked = _thumbnailPanel.ReportThumbnailClicks;
        }

        private void Menu_Close_click(object sender, EventArgs e) {
            this.Close();
        }

        private void Menu_About_click(object sender, EventArgs e) {
            this.Hide();

            using (var box = new AboutForm()) {
                box.Location = RecenterLocation(this, box);
                box.ShowDialog();
                Location = RecenterLocation(box, this);
            }

            this.Show();
        }

        private void Menu_Language_click(object sender, EventArgs e) {
            ToolStripItem tsi = (ToolStripItem)sender;

            string langCode = tsi.Tag as string;

            if (Program.ForceGlobalLanguageChange(langCode))
                this.Close();
            else
                MessageBox.Show("Error");
        }

        void Menu_Windows_itemclick(object sender, EventArgs e) {
            //Ensure the menu is closed
            menuContext.Close();

            //Get clicked item and window index from tag
            ToolStripItem tsi = (ToolStripItem)sender;

            //Handle special "none" window
            if (tsi.Tag == null) {
                UnsetThumbnail();
                return;
            }

            var selectionData = (WindowListHelper.WindowSelectionData)tsi.Tag;
            SetThumbnail(selectionData.Handle, selectionData.Region);
        }

        private void Menu_Switch_click(object sender, EventArgs e) {
            if (_lastWindowHandle == null)
                return;

            Program.Platform.HideForm(this);
            Native.WindowManagerMethods.SetForegroundWindow(_lastWindowHandle.Handle);
        }

        private void Menu_Group_click(object sender, EventArgs e) {
            SetSidePanel(new SidePanels.GroupSwitchPanel());
        }

        private void Menu_Forward_click(object sender, EventArgs e) {
            if (Settings.Default.FirstTimeClickForwarding && !_thumbnailPanel.ReportThumbnailClicks) {
                TaskDialog dlg = new TaskDialog(Strings.InfoClickForwarding, Strings.InfoClickForwardingTitle, Strings.InfoClickForwardingContent) {
                    CommonButtons = TaskDialogButton.Yes | TaskDialogButton.No
                };
                if (dlg.Show(this).CommonButton == Result.No)
                    return;

                Settings.Default.FirstTimeClickForwarding = false;
            }

            _thumbnailPanel.ReportThumbnailClicks = !_thumbnailPanel.ReportThumbnailClicks;
        }

        private void Menu_Opacity_opening(object sender, CancelEventArgs e) {
            ToolStripMenuItem[] items = {
				toolStripMenuItem1,
				toolStripMenuItem2,
				toolStripMenuItem3,
				toolStripMenuItem4
			};

            foreach (ToolStripMenuItem i in items) {
                if ((double)i.Tag == this.Opacity)
                    i.Checked = true;
                else
                    i.Checked = false;
            }
        }

        private void Menu_Opacity_click(object sender, EventArgs e) {
            //Get clicked menu item
            ToolStripMenuItem tsi = sender as ToolStripMenuItem;

            if (tsi != null) {
                //Get opacity from the tag
                this.Opacity = (double)tsi.Tag;
            }
        }

        private void Menu_Region_click(object sender, EventArgs e) {
            SetSidePanel(new OnTopReplica.SidePanels.RegionPanel());
        }

        private void Menu_Resize_opening(object sender, CancelEventArgs e) {
            if (!_thumbnailPanel.IsShowingThumbnail)
                e.Cancel = true;
        }

        private void Menu_Resize_Double(object sender, EventArgs e) {
            FitToThumbnail(2.0);
        }

        private void Menu_Resize_FitToWindow(object sender, EventArgs e) {
            FitToThumbnail(1.0);
        }

        private void Menu_Resize_Half(object sender, EventArgs e) {
            FitToThumbnail(0.5);
        }

        private void Menu_Resize_Quarter(object sender, EventArgs e) {
            FitToThumbnail(0.25);
        }

        private void Menu_Resize_Fullscreen(object sender, EventArgs e) {
            IsFullscreen = true;
        }

        private void Menu_Position_TopLeft(object sender, EventArgs e) {
            var screen = Screen.FromControl(this);

            Location = new Point(
                screen.WorkingArea.Left - ChromeBorderHorizontal,
                screen.WorkingArea.Top - ChromeBorderVertical
            );
        }

        private void Menu_Position_TopRight(object sender, EventArgs e) {
            var screen = Screen.FromControl(this);

            Location = new Point(
                screen.WorkingArea.Width - Size.Width + ChromeBorderHorizontal,
                screen.WorkingArea.Top - ChromeBorderVertical
            );
        }

        private void Menu_Position_BottomLeft(object sender, EventArgs e) {
            var screen = Screen.FromControl(this);

            Location = new Point(
                screen.WorkingArea.Left - ChromeBorderHorizontal,
                screen.WorkingArea.Height - Size.Height + ChromeBorderVertical
            );
        }

        private void Menu_Position_BottomRight(object sender, EventArgs e) {
            var screen = Screen.FromControl(this);

            Location = new Point(
                screen.WorkingArea.Width - Size.Width + ChromeBorderHorizontal,
                screen.WorkingArea.Height - Size.Height + ChromeBorderVertical
            );
        }

        private void Menu_Reduce_click(object sender, EventArgs e) {
            //Hide form in a platform specific way
            Program.Platform.HideForm(this);
        }

        private void Menu_Windows_opening(object sender, EventArgs e) {
            //Refresh window list
            _windowManager.Refresh(WindowManager.EnumerationMode.TaskWindows);

            WindowListHelper.PopulateMenu(this, _windowManager, menuWindows, _lastWindowHandle, new EventHandler(Menu_Windows_itemclick));
        }

        private void Menu_Chrome_click(object sender, EventArgs e) {
            if (FormBorderStyle == FormBorderStyle.Sizable) {
                FormBorderStyle = FormBorderStyle.None;
                Location = new Point {
                    X = Location.X + SystemInformation.FrameBorderSize.Width,
                    Y = Location.Y + SystemInformation.FrameBorderSize.Height
                };
            }
            else {
                FormBorderStyle = FormBorderStyle.Sizable;
                Location = new Point {
                    X = Location.X - SystemInformation.FrameBorderSize.Width,
                    Y = Location.Y - SystemInformation.FrameBorderSize.Height
                };
            }

            Invalidate();
        }

        #endregion

        #region Event handling

        void Form_KeyUp(object sender, KeyEventArgs e) {
            //ALT
            if (e.Modifiers == Keys.Alt) {
                if (e.KeyCode == Keys.Enter) {
                    e.Handled = true;
                    IsFullscreen = !IsFullscreen;
                }

                else if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) {
                    FitToThumbnail(0.25);
                }

                else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) {
                    FitToThumbnail(0.5);
                }

                else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0) {
                    FitToThumbnail(1.0);
                }

                else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4) {
                    FitToThumbnail(2.0);
                }
            }

            //ESCAPE
            else if (e.KeyCode == Keys.Escape) {
                //Toggle fullscreen
                if (IsFullscreen) {
                    IsFullscreen = false;
                }
                //Disable click forwarding
                else if (_thumbnailPanel.ReportThumbnailClicks) {
                    _thumbnailPanel.ReportThumbnailClicks = false;
                }
            }
        }

        void HotKeyOpenHandler() {
            if (IsFullscreen)
                IsFullscreen = false;

            if (Visible && WindowState != FormWindowState.Minimized) {
                Program.Platform.HideForm(this);
            }
            else {
                EnsureMainFormVisible();
            }
        }

        #endregion

        #region Fullscreen

        bool _isFullscreen = false;
        Point _preFullscreenLocation;
        Size _preFullscreenSize;

        public bool IsFullscreen {
            get {
                return _isFullscreen;
            }
            set {
                if (IsFullscreen == value)
                    return;

                CloseSidePanel(); //on switch, always hide side panels
                GlassEnabled = !value;
                FormBorderStyle = (value) ? FormBorderStyle.None : FormBorderStyle.Sizable;
                TopMost = !value;

                //Location and size
                if (value) {
                    _preFullscreenLocation = Location;
                    _preFullscreenSize = Size;

                    var currentScreen = Screen.FromControl(this);
                    Size = currentScreen.WorkingArea.Size;
                    Location = currentScreen.WorkingArea.Location;
                }
                else {
                    Location = _preFullscreenLocation;
                    Size = _preFullscreenSize;
                }

                _isFullscreen = value;
            }
        }

        #endregion

        #region Thumbnail operation

        /// <summary>
        /// Sets a new thumbnail.
        /// </summary>
        /// <param name="handle">Handle to the window to clone.</param>
        /// <param name="region">Region of the window to clone.</param>
        public void SetThumbnail(WindowHandle handle, StoredRegion region) {
            try {
                _lastWindowHandle = handle;
                _thumbnailPanel.SetThumbnailHandle(handle);

                if (region != null)
                    _thumbnailPanel.SelectedRegion = region.Rect;
                else
                    _thumbnailPanel.ConstrainToRegion = false;
            }
            catch (Exception ex) {
                ThumbnailError(ex, false, Strings.ErrorUnableToCreateThumbnail);
            }

            //Set aspect ratio (this will resize the form)
            SetAspectRatio(_thumbnailPanel.ThumbnailOriginalSize);
        }

        /// <summary>
        /// Enables group mode on a list of window handles.
        /// </summary>
        /// <param name="handles">List of window handles.</param>
        public void SetThumbnailGroup(IList<WindowHandle> handles) {
            if (handles.Count == 0)
                return;

            //At last one thumbnail
            SetThumbnail(handles[0], null);

            //Handle if no real group
            if (handles.Count == 1)
                return;

            _lastWindowHandle = null;
            _msgPumpManager.Get<MessagePumpProcessors.GroupSwitchManager>().EnableGroupMode(handles);
        }

        /// <summary>
        /// Disables the cloned thumbnail.
        /// </summary>
        public void UnsetThumbnail() {
            //Unset handle
            _lastWindowHandle = null;
            _thumbnailPanel.UnsetThumbnail();

            //Disable aspect ratio
            KeepAspectRatio = false;
        }

        /// <summary>
        /// Gets or sets the region displayed of the current thumbnail.
        /// </summary>
        public Rectangle? SelectedThumbnailRegion {
            get {
                if (!_thumbnailPanel.IsShowingThumbnail || !_thumbnailPanel.ConstrainToRegion)
                    return null;

                return _thumbnailPanel.SelectedRegion;
            }
            set {
                if (!_thumbnailPanel.IsShowingThumbnail)
                    return;

                if (value.HasValue) {
                    _thumbnailPanel.SelectedRegion = value.Value;
                    SetAspectRatio(value.Value.Size);
                }
                else {
                    _thumbnailPanel.ConstrainToRegion = false;
                    SetAspectRatio(_thumbnailPanel.ThumbnailOriginalSize);
                }
            }
        }

        /// <summary>
        /// Gets the form's thumbnail panel.
        /// </summary>
        public ThumbnailPanel ThumbnailPanel {
            get {
                return _thumbnailPanel;
            }
        }

        /// <summary>
        /// Gets the form's message pump manager.
        /// </summary>
        public MessagePumpManager MessagePumpManager {
            get {
                return _msgPumpManager;
            }
        }

        private void ThumbnailError(Exception ex, bool suppress, string title) {
            if (!suppress) {
                ShowErrorDialog(title, Strings.ErrorGenericThumbnailHandleError, ex.Message);
            }

            UnsetThumbnail();
        }

        /// <summary>Automatically sizes the window in order to accomodate the thumbnail p times.</summary>
        /// <param name="p">Scale of the thumbnail to consider.</param>
        private void FitToThumbnail(double p) {
            try {
                Size originalSize = _thumbnailPanel.ThumbnailOriginalSize;
                Size fittedSize = new Size((int)(originalSize.Width * p), (int)(originalSize.Height * p));
                ClientSize = fittedSize;
            }
            catch (Exception ex) {
                ThumbnailError(ex, false, Strings.ErrorUnableToFit);
            }
        }

        #endregion

        #region GUI stuff

        private Point RecenterLocation(Control original, Control final) {
            int origX = original.Location.X + original.Size.Width / 2;
            int origY = original.Location.Y + original.Size.Height / 2;

            int finX = origX - final.Size.Width / 2;
            int finY = origY - final.Size.Height / 2;

            //Check boundaries
            var screen = Screen.FromControl(final);
            if (finX < screen.WorkingArea.X)
                finX = screen.WorkingArea.X;
            if (finX + final.Size.Width > screen.WorkingArea.Width)
                finX = screen.WorkingArea.Width - final.Size.Width;
            if (finY < screen.WorkingArea.Y)
                finY = screen.WorkingArea.Y;
            if (finY + final.Size.Height > screen.WorkingArea.Height)
                finY = screen.WorkingArea.Height - final.Size.Height;

            return new Point(finX, finY);
        }

        private int ChromeBorderVertical {
            get {
                if (FormBorderStyle == FormBorderStyle.Sizable)
                    return SystemInformation.FrameBorderSize.Height;
                else
                    return 0;
            }
        }

        private int ChromeBorderHorizontal {
            get {
                if (FormBorderStyle == FormBorderStyle.Sizable)
                    return SystemInformation.FrameBorderSize.Width;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Displays an error task dialog.
        /// </summary>
        /// <param name="mainInstruction">Main instruction of the error dialog.</param>
        /// <param name="explanation">Detailed informations about the error.</param>
        /// <param name="errorMessage">Expanded error codes/messages.</param>
        private void ShowErrorDialog(string mainInstruction, string explanation, string errorMessage) {
            TaskDialog dlg = new TaskDialog(mainInstruction, Strings.ErrorGenericTitle, explanation) {
                CommonIcon = TaskDialogIcon.Stop,
                IsExpanded = false
            };

            if (!string.IsNullOrEmpty(errorMessage)) {
                dlg.ExpandedInformation = Strings.ErrorGenericInfoText + errorMessage;
                dlg.ExpandedControlText = Strings.ErrorGenericInfoButton;
            }

            dlg.Show(this);
        }

        /// <summary>
        /// Ensures that the main form is visible (either closing the fullscreen mode or reactivating from task icon).
        /// </summary>
        public void EnsureMainFormVisible() {
            if (IsFullscreen)
                IsFullscreen = false;

            //Ensure main form is shown
            WindowState = FormWindowState.Normal;
            Show();
            Activate();
            TopMost = true;
        }

        /// <summary>
        /// Opens a confirmation dialog to confirm whether to reset the main form or not.
        /// </summary>
        public void ResetMainFormWithConfirmation() {
            var dlg = new TaskDialog(Strings.AskReset, Strings.AskResetTitle, Strings.AskResetContent);
            dlg.UseCommandLinks = true;
            dlg.CustomButtons = new CustomButton[] {
				new CustomButton(Result.OK, Strings.AskResetButtonOk),
				new CustomButton(Result.Cancel, Strings.ButtonCancel)
			};
            dlg.CommonIcon = TaskDialogIcon.Information;

            if (dlg.Show(this).CommonButton == Result.OK) {
                ResetMainForm();
            }
        }

        /// <summary>
        /// Resets the main form to its initial state.
        /// </summary>
        public void ResetMainForm() {
            //Reset form settings
            UnsetThumbnail();
            CloseSidePanel();

            //Reset location and size (edge of the screen, min size)
            Point nuLoc = Screen.PrimaryScreen.WorkingArea.Location;
            nuLoc.Offset(40, 40);
            Location = nuLoc;
            Size = MinimumSize;

            this.Show();
            this.Activate();
        }

        #endregion

    }
}
