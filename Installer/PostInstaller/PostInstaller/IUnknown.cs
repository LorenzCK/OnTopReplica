using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PostInstaller {
    [ComImport]
    //[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000000-0000-0000-C000-000000000046")]
    public interface IUnknown {

        IntPtr QueryInterface(ref Guid riid);

        [PreserveSig]
        ulong AddRef();

        [PreserveSig]
        ulong Release();

    }
}
