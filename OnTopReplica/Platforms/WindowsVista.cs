using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using VistaControls.Dwm;
using OnTopReplica.Properties;
using VistaControls.TaskDialog;

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

        public override void InitForm(MainForm form) {
            DwmManager.SetWindowFlip3dPolicy(form, Flip3DPolicy.ExcludeAbove);
            
            //Do not show in task bar, but display notify icon
            //NOTE: this effectively makes Windows ignore the Flip 3D policy set above (on Windows 7)
            //NOTE: this also makes HotKey registration critically fail on Windows 7
            form.ShowInTaskbar = false;

            //Install tray icon
            _icon = new NotificationIcon(form);
        }

        public override void ShutdownApp() {
            if (_icon != null) {
                _icon.Dispose();
                _icon = null;
            }
        }

    }

}
