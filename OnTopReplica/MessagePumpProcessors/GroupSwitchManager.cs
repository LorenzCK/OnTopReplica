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
    
    class GroupSwitchManager : BaseMessagePumpProcessor {

        public GroupSwitchManager() {
            _hookMsgId = HookMethods.RegisterWindowMessage("SHELLHOOK");
            if (_hookMsgId == 0)
                Console.Error.WriteLine("Failed to register SHELLHOOK Windows message.");
        }

        uint _hookMsgId;
        bool _active = false;
        IList<WindowHandle> _handles;

        public void EnableGroupMode(IList<WindowHandle> handles) {
            //Enable new hook
            if (!_active) {
                if (!HookMethods.RegisterShellHookWindow(Form.Handle)) {
                    Console.WriteLine("Failed to register shell hook window.");
                    return;
                }
            }

            //Okey dokey, will now track handles
            _handles = handles;
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
            if (_active && msg.Msg == _hookMsgId) {
                int hookCode = msg.WParam.ToInt32();
                if (hookCode == HookMethods.HSHELL_WINDOWACTIVATED ||
                    hookCode == HookMethods.HSHELL_RUDEAPPACTIVATED) {
         
                    IntPtr activeHandle = msg.LParam;
                    Console.WriteLine("New foreground: {0}", activeHandle);
                    HandleForegroundWindowChange(activeHandle);
                }
            }
        }

        private void HandleForegroundWindowChange(IntPtr activeWindow) {
            int iActive = -1;
            for (int i = 0; i < _handles.Count; ++i) {
                if (_handles[i].Handle == activeWindow)
                    iActive = i;
            }

            if (iActive < 0) {
                //new foreground window is not tracked
                Console.WriteLine("Active window is not tracked.");
                return;
            }

            //Get new handle to clone
            int iNewToClone = (iActive + 1) % _handles.Count;

            Console.WriteLine("Tracked as {0}. Switching to {1}.", iActive, iNewToClone);

            Form.SetThumbnail(_handles[iNewToClone], null);
        }

        protected override void Shutdown() {
            Disable();
        }

        /// <summary>
        /// Gets whether the group switch manager ia active.
        /// </summary>
        public bool IsActive {
            get {
                return _active;
            }
        }

    }

}
