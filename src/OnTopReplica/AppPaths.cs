using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OnTopReplica {
    public static class AppPaths {

        const string AppDataFolder = "OnTopReplica";

        public static void SetupPaths() {
            var roamingAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var roamingAppDataApplicationPath = Path.Combine(roamingAppData, AppDataFolder);

            if (!Directory.Exists(roamingAppDataApplicationPath)) {
                Directory.CreateDirectory(roamingAppDataApplicationPath);
            }
            PrivateRoamingFolderPath = roamingAppDataApplicationPath;
        }

        public static string PrivateRoamingFolderPath { get; private set; }

        public static string GenerateCrashDumpPath() {
            var now = DateTime.Now;

            string dump = string.Format("OnTopReplica-dump-{0}{1}{2}-{3}{4}.txt",
                now.Year, now.Month, now.Day,
                now.Hour, now.Minute);

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), dump);
        }
    }
}
