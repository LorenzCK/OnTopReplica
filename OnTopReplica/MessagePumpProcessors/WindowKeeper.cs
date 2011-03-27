using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Native;

namespace OnTopReplica.MessagePumpProcessors {
    
    /// <summary>
    /// Listens for shell events and closes the thumbnail if a cloned window is destroyed.
    /// </summary>
    class WindowKeeper : BaseMessagePumpProcessor {

        public override bool Process(ref Message msg) {
            if (Form.CurrentThumbnailWindowHandle != null &&
                msg.Msg == HookMethods.WM_SHELLHOOKMESSAGE) {
                int hookCode = msg.WParam.ToInt32();

                if (hookCode == HookMethods.HSHELL_WINDOWDESTROYED) {
                    //Check whether the destroyed window is the one we were cloning
                    IntPtr destroyedHandle = msg.LParam;
                    if (destroyedHandle == Form.CurrentThumbnailWindowHandle.Handle) {
                        //Disable group switch mode, since a window of the group has been destroyed
                        Form.MessagePumpManager.Get<GroupSwitchManager>().Disable();

                        //Disable cloning
                        Form.UnsetThumbnail();
                    }
                }
            }

            return false;
        }

        protected override void Shutdown() {
            
        }
    }

}
