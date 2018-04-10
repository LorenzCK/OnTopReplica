using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica.Native {
    /// <summary>
    /// Native Win32 sizing codes (used by WM_SIZING message).
    /// </summary>
    static class WMSZ {
        public const int LEFT = 1;
        public const int RIGHT = 2;
        public const int TOP = 3;
        public const int BOTTOM = 6;
    }
}
