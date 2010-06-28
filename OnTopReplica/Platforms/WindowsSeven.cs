using System;
using System.Collections.Generic;
using System.Text;
using VistaControls.Dwm;
using System.Windows.Forms;

namespace OnTopReplica.Platforms {
    class WindowsSeven : WindowsVista {

        public override bool InstallTrayIcon {
            get {
                return false;
            }
        }

        public override bool ShowsInTaskBar {
            get {
                return true;
            }
        }

        public override void InitForm(Form form) {
            base.InitForm(form);

            DwmManager.SetExludeFromPeek(form, true);
            DwmManager.SetDisallowPeek(form, true);
        }

    }
}
