using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Native;
using OnTopReplica.Properties;

namespace OnTopReplica.MessagePumpProcessors {

    /// <summary>
    /// HotKey registration helper.
    /// </summary>
    class HotKeyManager : BaseMessagePumpProcessor {

        public HotKeyManager() {
            Enabled = true;
        }

        delegate void HotKeyHandler();

        /// <summary>
        /// Wraps hot key handler registration data.
        /// </summary>
        private class HotKeyHandlerRegistration : IDisposable {
            private HotKeyHandlerRegistration() {
            }

            private HotKeyHandlerRegistration(IntPtr hwnd, int key, HotKeyHandler handler) {
                if (hwnd == IntPtr.Zero)
                    throw new ArgumentException();
                if (handler == null)
                    throw new ArgumentNullException();

                _hwnd = hwnd;
                RegistrationKey = key;
                Handler = handler;
            }

            static int _lastUsedKey = 0;

            /// <summary>
            /// Registers a new hotkey and returns a handle to the registration.
            /// </summary>
            /// <returns>Returns null on failure.</returns>
            public static HotKeyHandlerRegistration Register(Form owner, int keyCode, int modifiers, HotKeyHandler handler) {
                var key = ++_lastUsedKey;

                if (!HotKeyMethods.RegisterHotKey(owner.Handle, key, modifiers, keyCode)) {
                    Console.Error.WriteLine("Failed to create hotkey on keys {0}.", keyCode);
                    return null;
                }

                return new HotKeyHandlerRegistration(owner.Handle, key, handler);
            }

            IntPtr _hwnd;
            public int RegistrationKey { get; private set; }
            public HotKeyHandler Handler { get; private set; }

            public void Dispose() {
                if (!HotKeyMethods.UnregisterHotKey(_hwnd, RegistrationKey)) {
                    Console.Error.WriteLine("Failed to unregister hotkey #{0}.", RegistrationKey);
                }
            }
        }

        Dictionary<int, HotKeyHandlerRegistration> _handlers = new Dictionary<int, HotKeyHandlerRegistration>();

        public override void Initialize(MainForm form) {
            base.Initialize(form);

            RefreshHotkeys();
        }

        public override bool Process(ref Message msg) {
            if (Enabled && msg.Msg == HotKeyMethods.WM_HOTKEY) {
                int keyId = msg.WParam.ToInt32();
                if (!_handlers.ContainsKey(keyId))
                    return false;

                _handlers[keyId].Handler.Invoke();
            }

            return false;
        }

        public bool Enabled { get; set; }

        /// <summary>
        /// Refreshes registered hotkeys from Settings.
        /// </summary>
        /// <remarks>
        /// Application settings contain hotkey registration strings that are used
        /// automatically by this registration process.
        /// </remarks>
        public void RefreshHotkeys() {
            ClearHandlers();

            RegisterHandler(Settings.Default.HotKeyCloneCurrent, HotKeyCloneHandler);
            RegisterHandler(Settings.Default.HotKeyShowHide, HotKeyShowHideHandler);
        }

        private void RegisterHandler(string spec, HotKeyHandler handler) {
            if (string.IsNullOrEmpty(spec))
                return; //this can happen and is allowed => simply don't register
            if (handler == null)
                throw new ArgumentNullException();

            int modifiers = 0, keyCode = 0;

            try {
                HotKeyMethods.TranslateStringToKeyValues(spec, out modifiers, out keyCode);
            }
            catch (ArgumentException) {
                //TODO: swallowed exception
                return;
            }

            var reg = HotKeyHandlerRegistration.Register(Form, keyCode, modifiers, handler);
            if(reg != null)
                _handlers.Add(reg.RegistrationKey, reg);
        }

        private void ClearHandlers() {
            foreach (var hotkey in _handlers) {
                hotkey.Value.Dispose();
            }
            _handlers.Clear();
        }

        protected override void Shutdown() {
            ClearHandlers();
        }

        #region Hotkey callbacks

        /// <summary>
        /// Handles "show/hide" hotkey. Ensures the form is in restored state and switches
        /// between shown and hidden states.
        /// </summary>
        void HotKeyShowHideHandler() {
            if (Form.IsFullscreen)
                Form.IsFullscreen = false;

            if (!Program.Platform.IsHidden(Form)) {
                Program.Platform.HideForm(Form);
            }
            else {
                Form.EnsureMainFormVisible();
            }
        }

        /// <summary>
        /// Handles the "clone current" hotkey.
        /// </summary>
        void HotKeyCloneHandler() {
            var handle = Win32Helper.GetCurrentForegroundWindow();
            if (handle.Handle == Form.Handle)
                return;

            Form.SetThumbnail(handle, null);
        }

        #endregion

    }

}
