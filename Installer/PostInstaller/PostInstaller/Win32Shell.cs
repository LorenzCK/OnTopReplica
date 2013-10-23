using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PostInstaller {
    public static class Win32Shell {
        
        public readonly static Guid IShellItemId = new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe");

        public readonly static Guid IShellLinkId = new Guid("000214F9-0000-0000-C000-000000000046");

        public readonly static Guid IUnknownId = new Guid("00000000-0000-0000-C000-000000000046");

        public readonly static Guid IPropertyStoreId = new Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void SHCreateItemFromParsingName(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pszPath,
            [In] IntPtr pbc,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid iIdIShellItem,
            [Out][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] out IShellLink iShellItem
        );

    }
}
