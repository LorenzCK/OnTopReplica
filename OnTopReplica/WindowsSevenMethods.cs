using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica {
    
    static class WindowsSevenMethods {

        [DllImport("shell32.dll")]
        internal static extern void SetCurrentProcessExplicitAppUserModelID(
            [MarshalAs(UnmanagedType.LPWStr)] string AppID);

        [DllImport("shell32.dll")]
        internal static extern void GetCurrentProcessExplicitAppUserModelID(
            [Out(), MarshalAs(UnmanagedType.LPWStr)] out string AppID);

    }

}
