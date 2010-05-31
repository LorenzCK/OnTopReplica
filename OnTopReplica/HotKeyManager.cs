using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OnTopReplica {

    /// <summary>
    /// HotKey registration helper.
    /// </summary>
    public class HotKeyManager : IDisposable {

        public HotKeyManager(Form form) {
            Owner = form;
            
        }

        public Form Owner { get; private set; }

        [Flags]
        public enum HotKeyModifiers : int {
            Alt = 0x1,
            Control = 0x2,
            Shift = 0x4,
            Windows = 0x8
        }

        const int WM_HOTKEY = 0x312;

        [DllImport("user32.dll")]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private delegate void FormDelegate(HotKeyModifiers mod, Keys key, HotKeyHandler handler);

        public void RegisterHotKey(HotKeyModifiers mod, Keys key, HotKeyHandler handler) {
            Owner.Invoke(new FormDelegate(RegisterHotKeyCore), mod, key, handler);
        }

        private void RegisterHotKeyCore(HotKeyModifiers mod, Keys keys, HotKeyHandler handler) {
            var newKey = ++_lastUsedKey;

            if (!RegisterHotKey(Owner.Handle, newKey, (int)mod, (int)keys)) {
                Console.Error.WriteLine("Failed to register {0} hot key.", newKey);
                return;
            }

            _handlers[newKey] = handler;
        }

        public void ProcessHotKeys(Message msg) {
            if (msg.Msg == WM_HOTKEY) {
                int keyId = msg.WParam.ToInt32();
                if (!_handlers.ContainsKey(keyId))
                    return;
                _handlers[keyId].Invoke();
            }
        }

        private delegate void VoidFormDelegate();

        private void UnregisterHotKeysCore() {
            foreach (var key in _handlers.Keys) {
                if (!UnregisterHotKey(Owner.Handle, key))
                    Console.Error.WriteLine("Failed to unregister {0} hot key.", key);
            }
        }

        #region IDisposable Members

        public void Dispose() {
            if (Owner != null && Owner.IsHandleCreated) {
                Owner.Invoke(new VoidFormDelegate(UnregisterHotKeysCore));
            }
        }

        #endregion

        public delegate void HotKeyHandler();

        private int _lastUsedKey = 0;

        Dictionary<int, HotKeyHandler> _handlers = new Dictionary<int, HotKeyHandler>();

    }

}
