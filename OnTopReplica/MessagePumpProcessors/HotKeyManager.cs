using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Native;

namespace OnTopReplica.MessagePumpProcessors {

    /// <summary>
    /// HotKey registration helper.
    /// </summary>
    class HotKeyManager : BaseMessagePumpProcessor {

        int _lastUsedKey = 0;

        Dictionary<int, HotKeyMethods.HotKeyHandler> _handlers = new Dictionary<int, HotKeyMethods.HotKeyHandler>();

        public void RegisterHotKey(HotKeyModifiers mod, Keys keys, HotKeyMethods.HotKeyHandler handler) {
            var newKey = ++_lastUsedKey;

            if (!HotKeyMethods.RegisterHotKey(Form.Handle, newKey, (int)mod, (int)keys)) {
                Console.Error.WriteLine("Failed to register {0} hot key.", newKey);
                return;
            }

            _handlers[newKey] = handler;
        }

        public override bool Process(ref Message msg) {
            if (msg.Msg == HotKeyMethods.WM_HOTKEY) {
                int keyId = msg.WParam.ToInt32();
                if (!_handlers.ContainsKey(keyId))
                    return false;

                _handlers[keyId].Invoke();
            }

            return false;
        }

        protected override void Shutdown() {
            foreach (var key in _handlers.Keys) {
                if (!HotKeyMethods.UnregisterHotKey(Form.Handle, key)) {
                    Console.Error.WriteLine("Failed to unregister {0} hot key.", key);
                }
            }
        }
    }

}
