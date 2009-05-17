using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OnTopReplica.Properties;
using System.Threading;
using System.Globalization;

namespace OnTopReplica
{
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
			//Update settings if needed
			if (Settings.Default.MustUpdate) {
				Settings.Default.Upgrade();
				Settings.Default.MustUpdate = false;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			do {
				//Update language settings
				Thread.CurrentThread.CurrentUICulture = _languageChangeCode;
				Settings.Default.Language = _languageChangeCode;
				_languageChangeCode = null;

				Application.Run(new MainForm());
			}
			while(_languageChangeCode != null);

			//Persist settings
			Settings.Default.Save();
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
    }
}
