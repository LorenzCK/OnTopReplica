using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace OnTopReplica.Native {
    
    /// <summary>
    /// Native methods for filesystem interop.
    /// </summary>
    static class FilesystemMethods {

        /// <summary>
        /// Gets the path to the current user's download path.
        /// </summary>
        /// <remarks>
        /// Code taken from http://stackoverflow.com/questions/3795023/downloads-folder-not-special-enough
        /// </remarks>
        public static string DownloadsPath {
            get {
                string path = null;

                //Requires Vista or superior
                if (Environment.OSVersion.Version.Major >= 6) {
                    IntPtr pathPtr;
                    Guid folderId = FolderDownloads;
                    int hr = SHGetKnownFolderPath(ref folderId, 0, IntPtr.Zero, out pathPtr);
                    if (hr == 0) {
                        path = Marshal.PtrToStringUni(pathPtr);
                        Marshal.FreeCoTaskMem(pathPtr);
                        return path;
                    }
                }

                //Fallback code
                path = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
                path = Path.Combine(path, "Downloads");
                return path;
            }
        }

        
        static readonly Guid FolderDownloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetKnownFolderPath(ref Guid id, int flags, IntPtr token, out IntPtr path);

    }

}
