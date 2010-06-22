using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
