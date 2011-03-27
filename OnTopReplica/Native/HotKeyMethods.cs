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

    /// <summary>
    /// Static native methods for HotKey management.
    /// </summary>
    static class HotKeyMethods {

        public const int WM_HOTKEY = 0x312;

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Translates a key combination specification into key code values.
        /// </summary>
        /// <param name="hotkeySpec">Key combination specification (see remarks).</param>
        /// <param name="modifiers">Modifier values.</param>
        /// <param name="keys">Key values.</param>
        /// <remarks>
        /// Specification can contain one single key value (from the enumeration System.Windows.Forms.Keys)
        /// preceded by modifier strings (each one separated by a single '+').
        /// For instance:
        /// [CTRL]+[ALT]+A
        /// or
        /// [ALT]+[SHIFT]+O
        /// </remarks>
        public static void TranslateStringToKeyValues(string hotkeySpec, out int modifiers, out int keys) {
            if (string.IsNullOrEmpty(hotkeySpec))
                throw new ArgumentNullException();

            modifiers = 0;
            keys = 0;

            if (ExtractModifier(ref hotkeySpec, "[CTRL]+"))
                modifiers |= (int)HotKeyModifiers.Control;
            if (ExtractModifier(ref hotkeySpec, "[ALT]+"))
                modifiers |= (int)HotKeyModifiers.Alt;
            if (ExtractModifier(ref hotkeySpec, "[SHIFT]+"))
                modifiers |= (int)HotKeyModifiers.Shift;

            //Attempt to translate last part (should be single key)
            try {
                var keyValue = Enum.Parse(typeof(Keys), hotkeySpec, true);
                keys = (int)keyValue;
            }
            catch (ArgumentException) {
                throw new ArgumentException("Couldn't parse key value '" + hotkeySpec + "'.");
            }
        }

        private static bool ExtractModifier(ref string spec, string modifier) {
            int modIndex = spec.IndexOf(modifier);
            if (modIndex == -1)
                return false;

            spec = spec.Remove(modIndex, modifier.Length);
            return true;
        }

    }
}
