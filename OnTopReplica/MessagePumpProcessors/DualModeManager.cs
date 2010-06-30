using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using OnTopReplica.Native;

namespace OnTopReplica.MessagePumpProcessors {
    
    class DualModeManager : BaseMessagePumpProcessor {

        public DualModeManager() {
            _hookMsgId = HookMethods.RegisterWindowMessage("SHELLHOOK");
            if (_hookMsgId == 0)
                Console.Error.WriteLine("Failed to register SHELLHOOK Windows message.");
        }

        uint _hookMsgId;
        bool _active = false;

        public void EnableDualMode(IEnumerable<WindowHandle> handles) {
            if (!HookMethods.RegisterShellHookWindow(Form.Handle)) {
                Console.WriteLine("Failed to register shell hook window.");
                return;
            }

            _active = true;
        }

        public void Disable() {
            if (!_active)
                return;

            if (!HookMethods.DeregisterShellHookWindow(Form.Handle))
                Console.WriteLine("Failed to deregister shell hook window.");

            _active = false;
        }

        public override void Process(Message msg) {
            if (msg.Msg == _hookMsgId) {
                int hookCode = msg.WParam.ToInt32();
                if (hookCode == HookMethods.HSHELL_WINDOWACTIVATED ||
                    hookCode == HookMethods.HSHELL_RUDEAPPACTIVATED) {
                    IntPtr activeHandle = msg.LParam;
                }
            }
        }

        protected override void Shutdown() {
            Disable();
        }
    }

}
