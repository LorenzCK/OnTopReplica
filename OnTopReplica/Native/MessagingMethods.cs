using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    /// <summary>
    /// Common methods for Win32 messaging.
    /// </summary>
    static class MessagingMethods {

        public const int WM_GETICON = 0x7f;
        public const int WM_SIZING = 0x214;

        public const int WMSZ_LEFT = 1;
        public const int WMSZ_RIGHT = 2;
        public const int WMSZ_TOP = 3;
        public const int WMSZ_BOTTOM = 6;

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

        public const int HTTRANSPARENT = -1;
        public const int HTCLIENT = 1;
        public const int HTCAPTION = 2;

        public const int WM_NCHITTEST = 0x84;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_NCRBUTTONUP = 0x00A5;

        public const int MK_LBUTTON = 0x0001;

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MAXIMIZE = 61458;
        public const int SC_RESTORE = 61490;

        public static IntPtr MakeLParam(int LoWord, int HiWord) {
            return new IntPtr((HiWord << 16) | (LoWord & 0xffff));
        }

    }
}
