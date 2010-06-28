using System;
using System.Collections.Generic;
using System.Text;
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

        public override void InitForm(Form form) {
            DwmManager.SetWindowFlip3dPolicy(form, Flip3DPolicy.ExcludeAbove);
        }

    }
}
