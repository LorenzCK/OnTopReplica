using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica.Native {
    /// <summary>
    /// Native Windows Message codes.
    /// </summary>
    static class WM {
        public const int GETICON = 0x7f;
        public const int SIZING = 0x214;
        public const int NCHITTEST = 0x84;
        public const int NCPAINT = 0x0085;
        public const int LBUTTONDOWN = 0x0201;
        public const int LBUTTONUP = 0x0202;
        public const int LBUTTONDBLCLK = 0x0203;
        public const int RBUTTONDOWN = 0x0204;
        public const int RBUTTONUP = 0x0205;
        public const int RBUTTONDBLCLK = 0x0206;
        public const int NCLBUTTONUP = 0x00A2;
        public const int NCLBUTTONDOWN = 0x00A1;
        public const int NCLBUTTONDBLCLK = 0x00A3;
        public const int NCRBUTTONUP = 0x00A5;
        public const int NCMOUSELEAVE = 0x02A2;
        public const int SYSCOMMAND = 0x0112;
        public const int GETTEXT = 0x000D;
        public const int GETTEXTLENGTH = 0x000E;
    }
}
