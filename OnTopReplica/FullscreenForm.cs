using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VistaControls.TaskDialog;
using OnTopReplica.Properties;

namespace OnTopReplica {

    partial class FullscreenForm : Form {

        public FullscreenForm() {
            InitializeComponent();

            _thumbnail.GlassMode = true;

            //Set mode
            Mode = (Settings.Default.FullscreenAlwaysOnTop) ? FullscreenMode.AlwaysOnTop : FullscreenMode.Normal;

            //Set native renderer on context menu
            Asztal.Szótár.NativeToolStripRenderer.SetToolStripRenderer(new Control[] {
				menuContext, menuWindows
			});
        }

        WindowHandle _lastHandle;
        WindowManager _manager = new WindowManager(WindowManager.EnumerationMode.TaskWindows);

        public void DisplayFullscreen(Screen screen, WindowHandle window) {
            _lastHandle = window;

            //Init thumbnail
            _thumbnail.SetThumbnailHandle(window);

            //Form setup
            this.Location = screen.WorkingArea.Location;
            this.Size = screen.WorkingArea.Size;
        }

        public void CloseFullscreen() {
            this.Visible = false;

            _thumbnail.UnsetThumbnail();
        }

        public Rectangle ShownRegion {
            get {
                return _thumbnail.ShownRegion;
            }
            set {
                _thumbnail.ShownRegion = value;
            }
        }

        public bool ShowRegion {
            get {
                return _thumbnail.ShowRegion;
            }
            set {
                _thumbnail.ShowRegion = value;
            }
        }

        public WindowHandle LastWindowHandle {
            get {
                return _lastHandle;
            }
        }

        #region Event handling

        public event EventHandler<CloseRequestEventArgs> CloseRequest;

        protected virtual void OnCloseRequest() {
            if (CloseRequest != null)
                CloseRequest(this, new CloseRequestEventArgs {
                    LastWindowHandle = _lastHandle,
                    LastRegion = (_thumbnail.ShowRegion) ? (Rectangle?)_thumbnail.ShownRegion : null
                });
        }

        protected override void OnDoubleClick(EventArgs e) {
            OnCloseRequest();

            base.OnDoubleClick(e);
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                e.Handled = true;
                OnCloseRequest();
            }
            else if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Alt) {
                e.Handled = true;
                OnCloseRequest();
            }

            base.OnKeyUp(e);
        }

        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);

            //Never close
            OnCloseRequest();
            e.Cancel = true;
        }

        protected override void  OnActivated(EventArgs e) {
            base.OnActivated(e);

            //Disable "click through" on show: this prevents case in which the user
            //cannot return to standard mode closing and re-opening the fullscreen mode
            if (Mode == FullscreenMode.ClickThrough)
                Mode = (Settings.Default.FullscreenAlwaysOnTop) ? FullscreenMode.AlwaysOnTop : FullscreenMode.Normal;
        }

        #endregion

        #region Mode

        FullscreenMode _mode;

        public FullscreenMode Mode {
            get {
                return _mode;
            }
            set {
                _mode = value;
                Settings.Default.FullscreenAlwaysOnTop = (value != FullscreenMode.Normal);

                //Top most if always on top or click through
                this.TopMost = (value != FullscreenMode.Normal);
                this.TransparencyKey = (value == FullscreenMode.ClickThrough) ? Color.Black : Color.White;

                this.Invalidate();
            }
        }

        #endregion

        #region Menus

        private void Menu_Windows_opening(object sender, EventArgs e) {
            _manager.Refresh(WindowManager.EnumerationMode.TaskWindows);

            WindowListHelper.PopulateMenu(this, _manager, menuWindows, _lastHandle, new EventHandler(Menu_Window_click));
        }

        void Menu_Window_click(object sender, EventArgs e) {
            //Ensure menu is closed
            menuContext.Close();

            //Get clicked item and window index from tag
            ToolStripItem tsi = (ToolStripItem)sender;

            var windowData = tsi.Tag as WindowListHelper.WindowSelectionData;

            //Handle "-none-" window request
            if (windowData == null) {
                OnCloseRequest();
                return;
            }

            try {
                _thumbnail.SetThumbnailHandle(windowData.Handle);
                if (windowData.Region != null) {
                    _thumbnail.ShownRegion = windowData.Region.Rect;
                    _thumbnail.ShowRegion = true;
                }
                else {
                    _thumbnail.ShowRegion = false;
                }
                _lastHandle = windowData.Handle;
            }
            catch (Exception) {
                OnCloseRequest();
            }
        }

        private void Menu_Mode_opening(object sender, EventArgs e) {
            menuModeStandard.Checked = (Mode == FullscreenMode.Normal);
            menuModeOnTop.Checked = (Mode == FullscreenMode.AlwaysOnTop);
            menuModeClickThrough.Checked = (Mode == FullscreenMode.ClickThrough);
        }

        private void Menu_Mode_standard(object sender, EventArgs e) {
            Mode = FullscreenMode.Normal;
        }

        private void Menu_Mode_ontop(object sender, EventArgs e) {
            Mode = FullscreenMode.AlwaysOnTop;
        }

        private void Menu_Mode_clickthrough(object sender, EventArgs e) {
            if (!CheckFirstTimeClickThrough())
                return;

            Mode = FullscreenMode.ClickThrough;
        }

        private void Menu_Opacity_100(object sender, EventArgs e) {
            this.Opacity = 1.0;
        }

        private void Menu_Opacity_75(object sender, EventArgs e) {
            this.Opacity = 0.75;
        }

        private void Menu_Opacity_50(object sender, EventArgs e) {
            this.Opacity = 0.5;
        }

        private void Menu_Opacity_25(object sender, EventArgs e) {
            this.Opacity = 0.25;
        }

        private void Menu_Opacity_opening(object sender, EventArgs e) {
            menuOpacity100.Checked = (Opacity == 1.0);
            menuOpacity75.Checked = (Opacity == 0.75);
            menuOpacity50.Checked = (Opacity == 0.5);
            menuOpacity25.Checked = (Opacity == 0.25);
        }

        private void Menu_Quit_click(object sender, EventArgs e) {
            OnCloseRequest();
        }

        #endregion

        /// <summary>Check if the user uses click-through for the first time and asks confirmation.</summary>
        /// <returns>Returns whether to switch to click through mode or not.</returns>
        private bool CheckFirstTimeClickThrough() {
            if (Settings.Default.FirstTimeClickThrough) {
                //Alert the user about click through
                TaskDialog dlg = new TaskDialog(Strings.InfoClickThrough, Strings.InfoClickThroughTitle, Strings.InfoClickThroughInformation);
                dlg.CommonIcon = TaskDialogIcon.Information;
                dlg.ExpandedControlText = Strings.ErrorDetailButton;
                dlg.ExpandedInformation = Strings.InfoClickThroughDetails;
                dlg.UseCommandLinks = true;
                dlg.CustomButtons = new CustomButton[] {
					new CustomButton(Result.Yes, Strings.InfoClickThroughOk),
					new CustomButton(Result.No, Strings.InfoClickThroughNo)
				};

                var result = dlg.Show(this);
                return result.CommonButton == Result.Yes;

                //Settings.Default.ClickThrough = (dlg.Show(this).CommonButton == Result.Yes);
            }

            Settings.Default.FirstTimeClickThrough = false;
            return true;
        }

    }

}
