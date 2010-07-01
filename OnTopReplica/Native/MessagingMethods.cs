using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    /// <summary>
    /// Common methods for Win32 messaging.
    /// </summary>
    static class MessagingMethods {

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [Flags]
        public enum SendMessageTimeoutFlags : uint {
            AbortIfHung = 2,
            Block = 1,
            Normal = 0
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageTimeout(IntPtr hwnd, uint message, IntPtr wparam, IntPtr lparam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = false)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public static IntPtr MakeLParam(int LoWord, int HiWord) {
            return new IntPtr((HiWord << 16) | (LoWord & 0xffff));
        }

    }
}
