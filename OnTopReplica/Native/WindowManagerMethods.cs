using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    /// <summary>
    /// Common Win32 Window Manager native methods.
    /// </summary>
    static class WindowManagerMethods {

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr RealChildWindowFromPoint(IntPtr parent, NPoint point);

        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hwnd, ref NPoint point);

        /// <summary>
        /// Converts a point in client coordinates of a window to screen coordinates.
        /// </summary>
        /// <param name="hwnd">Handle to the window of the original point.</param>
        /// <param name="clientPoint">Point expressed in client coordinates.</param>
        /// <returns>Point expressed in screen coordinates.</returns>
        public static NPoint ClientToScreen(IntPtr hwnd, NPoint clientPoint) {
            NPoint localCopy = new NPoint(clientPoint);

            if (ClientToScreen(hwnd, ref localCopy))
                return localCopy;
            else
                return new NPoint();
        }

        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hwnd, ref NPoint point);

        /// <summary>
        /// Converts a point in screen coordinates in client coordinates relative to a window.
        /// </summary>
        /// <param name="hwnd">Handle of the window whose client coordinate system should be used.</param>
        /// <param name="screenPoint">Point expressed in screen coordinates.</param>
        /// <returns>Point expressed in client coordinates.</returns>
        public static NPoint ScreenToClient(IntPtr hwnd, NPoint screenPoint) {
            NPoint localCopy = new NPoint(screenPoint);

            if (ScreenToClient(hwnd, ref localCopy))
                return localCopy;
            else
                return new NPoint();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern bool SetForegroundWindow(IntPtr hwnd);

        public enum GetWindowMode : uint {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hwnd, GetWindowMode mode);

        /// <summary>
        /// Checks whether a window is a top-level window (has no owner nor parent window).
        /// </summary>
        /// <param name="hwnd">Handle to the window to check.</param>
        public static bool IsTopLevel(IntPtr hwnd) {
            bool hasParent = WindowManagerMethods.GetParent(hwnd).ToInt64() != 0;
            bool hasOwner = WindowManagerMethods.GetWindow(hwnd, WindowManagerMethods.GetWindowMode.GW_OWNER).ToInt64() != 0;

            return (!hasParent && !hasOwner);
        }

    }
}
