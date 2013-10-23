using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OnTopReplica {
    static class Log {

        const string LogFileName = "lastrun.log.txt";

        private readonly static StreamWriter Writer;

        static Log() {
            try {
                var filepath = Path.Combine(AppPaths.PrivateRoamingFolderPath, LogFileName);
                Writer = new StreamWriter(new FileStream(filepath, FileMode.Create));
                Writer.AutoFlush = true;
            }
            catch (Exception) {
                Writer = null;

#if DEBUG
                throw;
#endif
            }
        }

        public static void Write(string message) {
            WriteLine(message);
        }

        public static void Write(string format, object arg0) {
            WriteLine(string.Format(format, arg0));
        }

        public static void Write(string format, object arg0, object arg1) {
            WriteLine(string.Format(format, arg0, arg1));
        }

        public static void Write(string format, params object[] args) {
            WriteLine(string.Format(format, args));
        }

        public static void WriteDetails(string caption, string format, params object[] args) {
            WriteLines(caption, string.Format(format, args));
        }

        public static void WriteException(string message, Exception exception) {
            if (exception != null) {
                WriteLines(message, exception.ToString());
            }
            else {
                WriteLines(message, "(no last exception)");
            }
        }

        private static void WriteLine(string message) {
            var s = string.Format("{0,-8:HH:mm:ss} {1}", DateTime.Now, message);
            Writer.WriteLine(s);
        }

        private static void WriteLines(params string[] messages) {
            if (messages.Length > 0)
                WriteLine(messages[0]);
            if (messages.Length > 1) {
                for (int i = 1; i < messages.Length; ++i) {
                    Writer.WriteLine("         {0}", messages[i]);
                }
            }
        }

    }
}
