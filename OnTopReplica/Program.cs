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

        public static UpdateManager Update { get; private set; }

        static MainForm _mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            try {
                AppPaths.SetupPaths();
            }
            catch (Exception ex) {
                MessageBox.Show(string.Format("Unable to setup application folders: {0}", ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Log.Write("Launching OnTopReplica v.{0}", Application.ProductVersion);

            //Hook fatal abort handler
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //Initialize and check for platform support
            Platform = PlatformSupport.Create();
            if (!Platform.CheckCompatibility())
                return;
            Platform.PreHandleFormInit();

            Log.Write("Platform support initialized");

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
                Application.Idle += _handlerIdleUpdater;

                //Enter GUI loop
                Application.Run(_mainForm);

                //Re-enable chrome to store correct position (position is stored always WITH chrome: when restoring fails, the position stays ok)
                Settings.Default.RestoreLastShowChrome = _mainForm.IsChromeVisible;
                if (!_mainForm.IsChromeVisible)
                    _mainForm.IsChromeVisible = true;

                //Persist settings
                Log.Write("Last position before shutdown: {0}, size: {1}", _mainForm.Location, _mainForm.Size);
                Settings.Default.RestoreLastPosition = _mainForm.Location;
                Settings.Default.RestoreLastSize = _mainForm.ClientSize;
                Settings.Default.Save();

                //Store last thumbnail, if any
                if (_mainForm.ThumbnailPanel.IsShowingThumbnail && _mainForm.CurrentThumbnailWindowHandle != null) {
                    Settings.Default.RestoreLastWindowTitle = _mainForm.CurrentThumbnailWindowHandle.Title;
                    Settings.Default.RestoreLastWindowHwnd = _mainForm.CurrentThumbnailWindowHandle.Handle.ToInt64();
                    Settings.Default.RestoreLastWindowClass = _mainForm.CurrentThumbnailWindowHandle.Class;
                }
                else {
                    Settings.Default.RestoreLastWindowTitle = string.Empty;
                    Settings.Default.RestoreLastWindowHwnd = 0;
                    Settings.Default.RestoreLastWindowClass = string.Empty;
                }
            }
        }

        private static EventHandler _handlerIdleUpdater = new EventHandler(Application_Idle);

        /// <summary>
        /// Callback detecting application idle time.
        /// </summary>
        static void Application_Idle(object sender, EventArgs e) {
            Application.Idle -= _handlerIdleUpdater;

            Update = new UpdateManager(_mainForm);
            Update.UpdateCheckCompleted += new EventHandler<UpdateCheckCompletedEventArgs>(UpdateManager_CheckCompleted);
            Update.CheckForUpdate();
        }

        /// <summary>
        /// Callback that handles update checking.
        /// </summary>
        static void UpdateManager_CheckCompleted(object sender, UpdateCheckCompletedEventArgs e) {
            if (e.Success && e.Information != null) {
                Log.Write("Updated check successful (latest version is {0})", e.Information.LatestVersion);

                if (e.Information.IsNewVersion) {
                    Update.ConfirmAndInstall();
                }
            }
            else {
                Log.WriteException("Unable to check for updates", e.Error);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Log.WriteException("Unhandled exception", e.ExceptionObject as Exception);

            string path = AppPaths.GenerateCrashDumpPath();

            using (var s = new FileStream(path, FileMode.Create)) {
                using (var sw = new StreamWriter(s)) {
                    sw.WriteLine("OnTopReplica Dump file");
                    sw.WriteLine("This file has been created because OnTopReplica crashed.");
                    sw.WriteLine("Please send it to lck@klopfenstein.net to help fix the bug that caused the crash.");
                    sw.WriteLine();
                    sw.WriteLine("Last exception:");
                    sw.WriteLine(e.ExceptionObject.ToString());
                    sw.WriteLine();
                    sw.WriteLine("OnTopReplica v.{0}", Application.ProductVersion);
                    sw.WriteLine("OS: {0}", Environment.OSVersion.ToString());
                    sw.WriteLine(".NET: {0}", Environment.Version.ToString());
                    sw.WriteLine("DWM: {0}", WindowsFormsAero.OsSupport.IsCompositionEnabled);
                    sw.WriteLine("Launch command: {0}", Environment.CommandLine);
                    sw.WriteLine("UTC time: {0} {1}", DateTime.UtcNow.ToShortDateString(), DateTime.UtcNow.ToShortTimeString());
                }
            }

            Log.Write("Crash dump written to {0}", path);
        }

    }
}
