using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica {

    /// <summary>
    /// Describes a fullscreen mode used by OnTopReplica.
    /// </summary>
    public enum FullscreenMode {
        /// <summary>
        /// Normal non-topmost fullscreen mode.
        /// </summary>
        Normal,
        /// <summary>
        /// Topmost fullscreen mode.
        /// </summary>
        AlwaysOnTop,
        /// <summary>
        /// Clickthrough overlay mode.
        /// </summary>
        ClickThrough
    }

}
