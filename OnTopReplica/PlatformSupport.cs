using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Platforms;

namespace OnTopReplica {
    abstract class PlatformSupport {

        public static PlatformSupport Create() {
            var os = Environment.OSVersion;
            
            if (os.Platform != PlatformID.Win32NT)
                return new Other();

            if (os.Version.Major == 6) {
                if (os.Version.Minor >= 1)
                    return new WindowsSeven();
                else
                    return new WindowsVista();
            }
            else {
                //Generic NT
                return new WindowsXp();
            }
        }

        /// <summary>
        /// Checks whether OnTopReplica is compatible with the platform.
        /// </summary>
        /// <returns>Returns false if OnTopReplica cannot run.</returns>
        public abstract bool CheckCompatibility();

        /// <summary>
        /// Gets whether OnTopReplica should be displayed in the task bar.
        /// </summary>
        public virtual bool ShowsInTaskBar {
            get {
                return false;
            }
        }

        /// <summary>
        /// Gets whether OnTopReplica should install a tray icon.
        /// </summary>
        public virtual bool InstallTrayIcon {
            get {
                return true;
            }
        }

    }
}
