using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace OnTopReplica {
    
    class DualModeManager : IDisposable {

        public DualModeManager(MainForm form) {
            _form = form;
        }

        MainForm _form;
        bool _active = false;

        static uint _hookMsgId = NativeHooks.RegisterWindowMessage("SHELLHOOK");

        public void EnableDualMode(IEnumerable<WindowHandle> handles) {
            if (!NativeHooks.RegisterShellHookWindow(_form.Handle)) {
                Console.WriteLine("Failed to register shell hook window.");
                return;
            }

            _active = true;
        }

        public void ProcessHookMessages(Message msg) {
            if (msg.Msg == _hookMsgId) {
                int hookCode = msg.WParam.ToInt32();
                if (hookCode == NativeHooks.HSHELL_WINDOWACTIVATED ||
                    hookCode == NativeHooks.HSHELL_RUDEAPPACTIVATED) {
                    IntPtr activeHandle = msg.LParam;
                }
            }
        }

        public void Disable() {
            if (!_active)
                return;

            if (!NativeHooks.DeregisterShellHookWindow(_form.Handle))
                Console.WriteLine("Failed to deregister shell hook window.");

            _active = false;
        }

        #region IDisposable Members

        public void Dispose() {
            Disable();
        }

        #endregion

    }

}
