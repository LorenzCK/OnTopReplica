using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    /// <summary>
    /// Common Win32 methods for operating on windows.
    /// </summary>
    static class WindowMethods {

        public static System.Drawing.Point GetCursorPos() {
            NPoint ret;
            if (GetCursorPosInternal(out ret))
                return ret.ToPoint();
            else {
#if DEBUG
                throw new InvalidOperationException("Unable to GetCursorPos");
#else
                return default(System.Drawing.Point);
#endif
            }
        }

        [DllImport("user32.dll", EntryPoint="GetCursorPos")]
        private static extern bool GetCursorPosInternal(out NPoint point);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr handle, out NRectangle rect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr handle, out NRectangle rect);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        /// <summary>
        /// Gets a window's text via API call.
        /// </summary>
        /// <param name="hwnd">Window handle.</param>
        /// <returns>Title of the window.</returns>
        public static string GetWindowText(IntPtr hwnd) {
            int length = GetWindowTextLength(hwnd);

            if (length > 0) {
                StringBuilder sb = new StringBuilder(length + 1);
                if (WindowMethods.GetWindowText(hwnd, sb, sb.Capacity) > 0)
                    return sb.ToString();
                else
                    return String.Empty;
            }
            else
                return String.Empty;
        }

        public enum WindowLong {
            WndProc = (-4),
            HInstance = (-6),
            HwndParent = (-8),
            Style = (-16),
            ExStyle = (-20),
            UserData = (-21),
            Id = (-12)
        }

        [Flags]
        public enum WindowStyles : long {
            None = 0,
            Child = 0x40000000L,
            Disabled = 0x8000000L,
            Minimize = 0x20000000L,
            MinimizeBox = 0x20000L,
            Visible = 0x10000000L
        }

        [Flags]
        public enum WindowExStyles : long {
            AppWindow = 0x40000,
            Layered = 0x80000,
            NoActivate = 0x8000000L,
            ToolWindow = 0x80,
            TopMost = 8,
            Transparent = 0x20
        }

        public static IntPtr GetWindowLong(IntPtr hWnd, WindowLong i) {
            if (IntPtr.Size == 8) {
                return GetWindowLongPtr64(hWnd, i);
            }
            else {
                return new IntPtr(GetWindowLong32(hWnd, i));
            }
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern int GetWindowLong32(IntPtr hWnd, WindowLong nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, WindowLong nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, WindowLong i, IntPtr dwNewLong) {
            if (IntPtr.Size == 8) {
                return SetWindowLongPtr64(hWnd, i, dwNewLong);
            }
            else {
                return new IntPtr(SetWindowLong32(hWnd, i, dwNewLong.ToInt32()));
            }
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        static extern int SetWindowLong32(IntPtr hWnd, WindowLong nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, WindowLong nIndex, IntPtr dwNewLong);

        #region Window class

        const int MaxClassLength = 255;

        public static string GetWindowClass(IntPtr hwnd) {
            var sb = new StringBuilder(MaxClassLength + 1);
            RealGetWindowClass(hwnd, sb, MaxClassLength);
            return sb.ToString();
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint RealGetWindowClass(IntPtr hwnd, [Out] StringBuilder lpString, uint maxCount);

        public enum ClassLong {
            Icon = -14,
            IconSmall = -34
        }

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtrW")]
        static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongW")]
        static extern int GetClassLong32(IntPtr hWnd, int nIndex);

        public static IntPtr GetClassLong(IntPtr hWnd, ClassLong i) {
            if (IntPtr.Size == 8) {
                return GetClassLong64(hWnd, (int)i);
            }
            else {
                return new IntPtr(GetClassLong32(hWnd, (int)i));
            }
        }

        #endregion

        [DllImport("user32.dll")]
        public static extern IntPtr GetMenu(IntPtr hwnd);

        /// <summary>
        /// Converts client size rectangle to window rectangle, according to window styles.
        /// </summary>
        /// <param name="clientRectangle">Client area bounding box.</param>
        /// <param name="windowStyle">Style of window to compute.</param>
        /// <param name="extendedWindowStyle">Extended style of window to compute.</param>
        public static NRectangle ConvertClientToWindowRect(NRectangle clientRectangle, long windowStyle, long extendedWindowStyle) {
            NRectangle tmp = clientRectangle;
            if (AdjustWindowRectEx(ref tmp, windowStyle, false, extendedWindowStyle)) {
                return tmp;
            }
            else {
#if DEBUG
                throw new InvalidOperationException("Failed to convert client rectangle to window rectangle");
#else
                return clientRectangle;
#endif
            }
        }

        [DllImport("user32.dll")]
        private static extern bool AdjustWindowRectEx(ref NRectangle clientToWindowRect, long windowStyle, bool hasMenu, long extendedWindowStyle);

    }
}
