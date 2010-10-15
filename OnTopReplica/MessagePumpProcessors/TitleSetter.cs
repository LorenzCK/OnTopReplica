using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Native;
using System.Runtime.InteropServices;

namespace OnTopReplica.MessagePumpProcessors {
    class TitleSetter : BaseMessagePumpProcessor {
        
        const string Title = "OnTopReplica";

        public override bool Process(ref Message msg) {
            switch (msg.Msg) {
                case WM.GETTEXT: {
                    Console.WriteLine("GetText");
                        int maxLen = msg.WParam.ToInt32();
                        byte[] strBytes = Encoding.UTF8.GetBytes(Title);
                        byte[] termBytes = new byte[strBytes.Length + 1];
                        strBytes.CopyTo(termBytes, 0);
                        termBytes[strBytes.Length] = 0;

                        Marshal.Copy(termBytes, 0, msg.LParam, Math.Min(maxLen, Title.Length + 1));
                    }
                    goto case WM.GETTEXTLENGTH;

                case WM.GETTEXTLENGTH:
                    Console.WriteLine("GetTextLength");
                    msg.Result = (IntPtr)Title.Length;
                    return true;
            }

            return false;
        }

        protected override void Shutdown() {
            
        }

    }
}
