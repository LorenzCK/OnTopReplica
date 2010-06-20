using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OnTopReplica.Properties;
using System.Threading;
using System.Globalization;
using System.Drawing;
using System.IO;
using VistaControls.TaskDialog;

namespace OnTopReplica
{
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            //Hook abort handler
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Check for DWM
            if (!CanStart)
                return;

			//Update settings if needed
			if (Settings.Default.MustUpdate) {
				Settings.Default.Upgrade();
				Settings.Default.MustUpdate = false;
			}

            bool reloadSettings = false;
            Point reloadLocation = new Point();
            Size reloadSize = new Size();

			do {
				//Update language settings
				Thread.CurrentThread.CurrentUICulture = _languageChangeCode;
				Settings.Default.Language = _languageChangeCode;
				_languageChangeCode = null;

                Form form;
                if (reloadSettings)
                    form = new MainForm(reloadLocation, reloadSize);
                else
                    form = new MainForm();

				Application.Run(form);

                reloadSettings = true;
                reloadLocation = form.Location;
                reloadSize = form.Size;
			}
			while(_languageChangeCode != null);

			//Persist settings
			Settings.Default.Save();
        }

        /// <summary>
        /// Checks whether OnTopReplica can start or not.
        /// </summary>
        private static bool CanStart {
            get {
                //Do some checks in order to verify the presence of desktop composition
                if (!VistaControls.OsSupport.IsVistaOrBetter) {
                    MessageBox.Show(Strings.ErrorNoDwm, Strings.ErrorNoDwmTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!VistaControls.OsSupport.IsCompositionEnabled) {
                    MessageBox.Show(Strings.ErrorDwmOffContent, Strings.ErrorDwmOff, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    /*var dlg = new TaskDialog(Strings.ErrorDwmOff, Strings.ErrorGenericTitle, Strings.ErrorDwmOffContent) {
                        ExpandedControlText = Strings.ErrorDetailsAero,
                        ExpandedInformation = Strings.ErrorDetailsAeroInfo,
                        CommonButtons = TaskDialogButton.Close,
                        CommonIcon = VistaControls.TaskDialog.TaskDialogIcon.Stop
                    };
                    dlg.Show();*/

                    return false;
                }

                return true;
            }
        }

		static CultureInfo _languageChangeCode = Settings.Default.Language;

		/// <summary>
		/// Forces a global language change. As soon as the main form is closed, the change is performed
		/// and the form is reopened using the new language.
		/// </summary>
		public static bool ForceGlobalLanguageChange(string languageCode){
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
                    sw.WriteLine("OS: {0}", Environment.OSVersion.ToString());
                    sw.WriteLine(".NET: {0}", Environment.Version.ToString());
                    sw.WriteLine("Aero DWM: {0}", VistaControls.OsSupport.IsCompositionEnabled);
                }
            }
        }

    }
}
