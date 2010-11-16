using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using OnTopReplica.Properties;
using OnTopReplica.StartupOptions;
using OnTopReplica.Update;

namespace OnTopReplica {
    
    static class Program {

        public static PlatformSupport Platform { get; private set; }

        static UpdateManager _updateManager;

        static MainForm _mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            //Hook fatal abort handler
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //Initialize and check for platform support
            Platform = PlatformSupport.Create();
            if (!Platform.CheckCompatibility())
                return;

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
            
            //Load language
            Thread.CurrentThread.CurrentUICulture = Settings.Default.Language;

            //Show form
            using (_mainForm = new MainForm(options)) {
                Application.Run(_mainForm);

                //Persist settings
                Settings.Default.RestoreLastPosition = _mainForm.Location;
                Settings.Default.RestoreLastSize = _mainForm.ClientSize;
                Settings.Default.Save();
            }
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
