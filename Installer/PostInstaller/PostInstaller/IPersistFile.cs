using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PostInstaller {

    [ComImportAttribute]
    [GuidAttribute("0000010B-0000-0000-C000-000000000046")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPersistFile {

        // can't get this to go if I extend IPersist, 
        // so put it here:
        [PreserveSig]
        int GetClassID(out Guid pClassID);

        //[helpstring("Checks for changes since
        // last file write")]
        [PreserveSig]
        int IsDirty();

        //[helpstring("Opens the specified file and
        // initializes the object from its contents")]
        int Load(
           [MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
           ulong dwMode);

        //[helpstring("Saves the object into 
        // the specified file")]
        void Save(
           [MarshalAs(UnmanagedType.LPWStr)] 
            string pszFileName,
           [MarshalAs(UnmanagedType.Bool)] 
            bool fRemember);

        //[helpstring("Notifies the object that save
        // is completed")]
        void SaveCompleted(
           [MarshalAs(UnmanagedType.LPWStr)]
            string pszFileName);

        //[helpstring("Gets the current name of the 
        // file associated with the object")]
        void GetCurFile(
           [MarshalAs(UnmanagedType.LPWStr)]
            out string ppszFileName);

    }
}
