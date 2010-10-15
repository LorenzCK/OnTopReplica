using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OnTopReplica.Properties;
using System.Threading;
using System.Globalization;
using System.Drawing;
using System.IO;
using VistaControls.TaskDialog;
using OnTopReplica.Update;
using System.Reflection;
using OnTopReplica.StartupOptions;

namespace OnTopReplica {
    
    static class Program {

        public static PlatformSupport Platform { get; private set; }

        static CultureInfo _languageChangeCode = Settings.Default.Language;

        static UpdateManager _updateManager;

        static MainForm _mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            //Hook abort handler
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //Initialize and check for platform support
            Platform = PlatformSupport.Create();
            if (!Platform.CheckCompatibility())
                return;
            Platform.InitApp();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Update settings if needed
            if (Settings.Default.MustUpdate) {
                Settings.Default.Upgrade();
                Settings.Default.MustUpdate = false;
            }

            //Load startup options
            var options = StartupOptions.Factory.CreateOptions(args);
            string optionsMessage = options.DebugMessage;
            if (!string.IsNullOrEmpty(optionsMessage)) { //show dialog if debug message present or if parsing failed
                var dlg = new CommandLineReportForm(options.Status, optionsMessage);
                dlg.ShowDialog();
            }
            if (options.Status == CliStatus.Information || options.Status == CliStatus.Error)
                return;
            
            bool mustReloadForm = false;
            Point reloadLocation = new Point();
            Size reloadSize = new Size();

            do {
                //Update language settings
                Thread.CurrentThread.CurrentUICulture = _languageChangeCode;
                Settings.Default.Language = _languageChangeCode;
                _languageChangeCode = null;

                _mainForm = new MainForm(options);
                if (mustReloadForm) {
                    _mainForm.Location = reloadLocation;
                    _mainForm.Size = reloadSize;
                }

                Application.Run(_mainForm);

                //Enable reloading on next loop
                mustReloadForm = true;
                reloadLocation = _mainForm.Location;
                reloadSize = _mainForm.Size;
            }
            while (_languageChangeCode != null);

            //Persist settings
            Settings.Default.RestoreLastPosition = reloadLocation;
            Settings.Default.RestoreLastSize = reloadSize;
            Settings.Default.Save();
        }

        /// <summary>
        /// Forces a global language change. As soon as the main form is closed, the change is performed
        /// and the form is reopened using the new language.
        /// </summary>
        public static bool ForceGlobalLanguageChange(string languageCode) {
            if (string.IsNullOrEmpty(languageCode))
                return false;

            try {
                _languageChangeCode = new CultureInfo(languageCode);
            }
            catch {
                return false;
            }

            return true;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            string dump = string.Format("OnTopReplica-dump-{0}{1}{2}{3}{4}.txt",
                DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day,
                DateTime.UtcNow.Hour, DateTime.UtcNow.Minute);
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), dump);

            using (var s = new FileStream(path, FileMode.Create)) {
                using (var sw = new StreamWriter(s)) {
                    sw.WriteLine("OnTopReplica Dump file");
                    sw.WriteLine("This file has been created because OnTopReplica crashed.");
                    sw.WriteLine("Please send it to lck@klopfenstein.net to help fix the bug that caused the crash.");
                    sw.WriteLine();
                    sw.WriteLine("Last exception:");
                    sw.WriteLine(e.ExceptionObject.ToString());
                    sw.WriteLine();
                    sw.WriteLine("OnTopReplica v. {0}", Assembly.GetEntryAssembly().GetName().Version);
                    sw.WriteLine("OS: {0}", Environment.OSVersion.ToString());
                    sw.WriteLine(".NET: {0}", Environment.Version.ToString());
                    sw.WriteLine("Aero DWM: {0}", VistaControls.OsSupport.IsCompositionEnabled);
                    sw.WriteLine("Launch command: {0}", Environment.CommandLine);
                }
            }
        }

    }
}
