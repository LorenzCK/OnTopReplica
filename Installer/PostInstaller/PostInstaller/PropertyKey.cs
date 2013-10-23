using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PostInstaller {
    [StructLayout(LayoutKind.Sequential)]
    public struct PropertyKey {

        public PropertyKey(Guid formatId, uint propertyId) {
            fmtid = formatId;
            pid = propertyId;
        }

        public Guid fmtid;
        public uint pid;

    }
}
