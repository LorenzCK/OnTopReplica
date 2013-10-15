using OnTopReplica.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica {
    class FullscreenFormManager {

        private readonly MainForm _mainForm;

        public FullscreenFormManager(MainForm form) {
            _mainForm = form;
            IsFullscreen = false;
        }

        Point _preFullscreenLocation;
        Size _preFullscreenSize;
        FormBorderStyle _preFullscreenBorderStyle;

        public bool IsFullscreen {
            get;
            private set;
        }

        public void SwitchFullscreen() {
            SwitchFullscreen(Settings.Default.GetFullscreenMode());
        }

        public void SwitchFullscreen(FullscreenMode mode) {
            if (IsFullscreen) {
                MoveToFullscreenMode(mode);
                return;
            }

            if (!_mainForm.ThumbnailPanel.IsShowingThumbnail)
                return;

            //On switch, always hide side panels
            _mainForm.CloseSidePanel();

            //Store state
            _preFullscreenLocation = _mainForm.Location;
            _preFullscreenSize = _mainForm.ClientSize;
            _preFullscreenBorderStyle = _mainForm.FormBorderStyle;

            //Change state to fullscreen
            _mainForm.FormBorderStyle = FormBorderStyle.None;
            MoveToFullscreenMode(mode);

            CommonCompleteSwitch(true);
        }

        private void MoveToFullscreenMode(FullscreenMode mode) {
            var screens = Screen.AllScreens;

            var currentScreen = Screen.FromControl(_mainForm);
            Size size = _mainForm.Size;
            Point location = _mainForm.Location;

            switch (mode) {
                case FullscreenMode.Standard:
                default:
                    size = currentScreen.WorkingArea.Size;
                    location = currentScreen.WorkingArea.Location;
                    break;

                case FullscreenMode.Fullscreen:
                    size = currentScreen.Bounds.Size;
                    location = currentScreen.Bounds.Location;
                    break;

                case FullscreenMode.AllScreens:
                    size = SystemInformation.VirtualScreen.Size;
                    location = SystemInformation.VirtualScreen.Location;
                    break;
            }

            _mainForm.Size = size;
            _mainForm.Location = location;
        }

        public void SwitchBack() {
            if (!IsFullscreen)
                return;

            //Restore state
            _mainForm.FormBorderStyle = _preFullscreenBorderStyle;
            _mainForm.Location = _preFullscreenLocation;
            _mainForm.ClientSize = _preFullscreenSize;
            _mainForm.RefreshAspectRatio();

            CommonCompleteSwitch(false);
        }

        private void CommonCompleteSwitch(bool enabled) {
            //UI stuff switching
            _mainForm.GlassEnabled = !enabled;
            _mainForm.TopMost = !enabled;
            _mainForm.HandleMouseMove = !enabled;

            IsFullscreen = enabled;

            Program.Platform.OnFormStateChange(_mainForm);
        }

        public void Toggle() {
            if (IsFullscreen)
                SwitchBack();
            else
                SwitchFullscreen(Settings.Default.GetFullscreenMode());
        }

    }
}
