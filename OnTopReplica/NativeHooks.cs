using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica {

    /// <summary>
    /// Helpers for interop with native Windows hooks.
    /// </summary>
    static class NativeHooks {

        public const int HSHELL_WINDOWACTIVATED = 4;
        public const int HSHELL_RUDEAPPACTIVATED = HSHELL_WINDOWACTIVATED | HSHELL_HIGHBIT;
        const int HSHELL_HIGHBIT = 0x8000;

        /// <summary>
        /// Registers the WM_ID for a window message.
        /// </summary>
        /// <param name="wndMessageName">Name of the window message.</param>
        [DllImport("User32.dll")]
        public static extern uint RegisterWindowMessage(string wndMessageName);

        /// <summary>
        /// Registers a window as a shell hook window.
        /// </summary>
        [DllImport("User32.dll")]
        public static extern bool RegisterShellHookWindow(IntPtr hwnd);

        /// <summary>
        /// Deregisters a window as a shell hook window.
        /// </summary>
        [DllImport("User32.dll")]
        public static extern bool DeregisterShellHookWindow(IntPtr hwnd);

    }
}
