using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    /// <summary>
    /// Common Win32 error handling methods.
    /// </summary>
    static class ErrorMethods {

        [DllImport("kernel32.dll")]
        static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
           int dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer,
           uint nSize, IntPtr Arguments);

        /// <summary>
        /// Gets a string representation of a Win32 error code.
        /// </summary>
        /// <param name="msgCode">ID of the Win32 error code.</param>
        /// <returns>String representation of the error.</returns>
        public static string GetErrorMessage(int msgCode) {
            var sb = new StringBuilder(300);
            FormatMessage((uint)(0x00001000), IntPtr.Zero, msgCode, 0, sb, 299, IntPtr.Zero);
            return sb.ToString();
        }

        /// <summary>
        /// Gets a string representation of the last Win32 error on this thread.
        /// </summary>
        public static string GetLastErrorMessage() {
            int errorCode = Marshal.GetLastWin32Error();
            return GetErrorMessage(errorCode);
        }

    }
}
