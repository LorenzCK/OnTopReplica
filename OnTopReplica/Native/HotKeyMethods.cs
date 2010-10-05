using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OnTopReplica.Native {

    [Flags]
    public enum HotKeyModifiers : int {
        Alt = 0x1,
        Control = 0x2,
        Shift = 0x4,
        Windows = 0x8
    }

    static class HotKeyMethods {

        public delegate void HotKeyHandler();

        public const int WM_HOTKEY = 0x312;

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public delegate void FormDelegate(HotKeyModifiers mod, Keys key, HotKeyHandler handler);

    }
}
