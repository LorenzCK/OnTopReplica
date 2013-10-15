using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    public static class CommonControls {

        [DllImport("comctl32.dll", EntryPoint = "InitCommonControlsEx", CallingConvention = CallingConvention.StdCall)]
        static extern bool InitCommonControlsEx(ref INITCOMMONCONTROLSEX iccex);

        const int ICC_STANDARD_CLASSES = 0x00004000;
        const int ICC_WIN95_CLASSES = 0x000000FF;

        public static bool InitStandard() {
            INITCOMMONCONTROLSEX ex = new INITCOMMONCONTROLSEX();
            ex.dwSize = 8;
            ex.dwICC = ICC_STANDARD_CLASSES | ICC_WIN95_CLASSES;

            return InitCommonControlsEx(ref ex);
        }

    }

    struct INITCOMMONCONTROLSEX {
        public int dwSize;
        public int dwICC;
    }

}
