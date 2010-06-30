using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    /// <summary>
    /// Common Win32 methods for operating on windows.
    /// </summary>
    static class WindowMethods {

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        public enum WindowLong {
            WndProc = (-4),
            HInstance = (-6),
            HwndParent = (-8),
            Style = (-16),
            ExStyle = (-20),
            UserData = (-21),
            Id = (-12)
        }

        public enum ClassLong {
            Icon = -14,
            IconSmall = -34
        }

        [Flags]
        public enum WindowStyles : long {
            None = 0,
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
                return GetWindowLongPtr64(hWnd, (int)i);
            }
            else {
                return new IntPtr(GetWindowLong32(hWnd, (int)i));
            }
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr GetClassLongPtr(IntPtr hWnd, ClassLong i) {
            if (IntPtr.Size == 8) {
                return GetClassLong64(hWnd, (int)i);
            }
            else {
                return new IntPtr(GetClassLong32(hWnd, (int)i));
            }
        }

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtrW")]
        static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongW")]
        static extern int GetClassLong32(IntPtr hWnd, int nIndex);


        [DllImport("user32.dll")]
        public static extern IntPtr GetMenu(IntPtr hwnd);

    }
}
