using System;
using System.Windows.Forms;
using VistaControls.Dwm;

namespace OnTopReplica.Platforms {

    class WindowsVista : PlatformSupport {
        
        public override bool CheckCompatibility() {
            if (!VistaControls.OsSupport.IsCompositionEnabled) {
                MessageBox.Show(Strings.ErrorDwmOffContent, Strings.ErrorDwmOff, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        NotificationIcon _icon;

        public override void PostHandleFormInit(MainForm form) {
            //Do not show in task bar, but display notify icon
            //NOTE: this effectively makes Windows ignore the Flip 3D policy set above (on Windows 7)
            //NOTE: this also makes HotKey registration critically fail on Windows 7
            form.ShowInTaskbar = false;

            DwmManager.SetWindowFlip3dPolicy(form, Flip3DPolicy.ExcludeAbove);
            
            _icon = new NotificationIcon(form);
        }

        public override void CloseForm(MainForm form) {
            if (_icon != null) {
                _icon.Dispose();
                _icon = null;
            }
        }

        public override bool IsHidden(MainForm form) {
            return !form.Visible;
        }

        public override void HideForm(MainForm form) {
            form.Hide();
        }

        public override void RestoreForm(MainForm form) {
            form.Show();
        }

    }

}
