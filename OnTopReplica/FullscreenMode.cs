using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using OnTopReplica.Properties;

namespace OnTopReplica {
    /// <summary>
    /// Describes a fullscreen mode.
    /// </summary>
    enum FullscreenMode {
        Standard,
        Fullscreen,
        AllScreens
    }

    static class FullscreenModeExtensions {

        /// <summary>
        /// Gets the fullscreen mode as an enumeration value.
        /// </summary>
        public static FullscreenMode GetFullscreenMode(this Settings settings) {
            FullscreenMode retMode = FullscreenMode.Standard;

            Enum.TryParse<FullscreenMode>(settings.FullscreenMode, out retMode);

            return retMode;
        }

        /// <summary>
        /// Sets the fullscreen mode.
        /// </summary>
        public static void SetFullscreenMode(this Settings settings, FullscreenMode mode) {
            settings.FullscreenMode = mode.ToString();
        }

    }
}
