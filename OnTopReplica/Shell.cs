using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OnTopReplica {

    static class Shell {

        /// <summary>
        /// Executes a filename via Windows shell.
        /// </summary>
        /// <param name="filename">Filename to execute.</param>
        public static void Execute(string filename){
            if (filename == null)
                throw new ArgumentNullException();

            Process.Start(new ProcessStartInfo {
                FileName = filename,
                UseShellExecute = true
            });
        }

    }

}
