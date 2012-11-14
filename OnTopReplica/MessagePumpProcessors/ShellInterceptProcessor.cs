using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Native;

namespace OnTopReplica.MessagePumpProcessors {
    
#if DEBUG

    /// <summary>
    /// Basic shell message interceptor to use for debugging.
    /// </summary>
    class ShellInterceptProcessor : BaseMessagePumpProcessor {
        
        public override bool Process(ref Message msg) {
            if (msg.Msg == HookMethods.WM_SHELLHOOKMESSAGE) {
                int hookCode = msg.WParam.ToInt32();

                System.Diagnostics.Trace.WriteLine(string.Format("Hook msg #{0}: {1}", hookCode, msg.LParam));
            }

            return false;
        }

        protected override void Shutdown() {
            
        }

    }

#endif

}
