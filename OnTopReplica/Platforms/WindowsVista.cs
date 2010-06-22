using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica.Platforms {
    class WindowsVista : PlatformSupport {
        
        public override bool CheckCompatibility() {
            if (!VistaControls.OsSupport.IsCompositionEnabled) {
                MessageBox.Show(Strings.ErrorDwmOffContent, Strings.ErrorDwmOff, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

    }
}
