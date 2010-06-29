using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Platforms;
using System.Windows.Forms;

namespace OnTopReplica {
    abstract class PlatformSupport : IDisposable {

        public static PlatformSupport Create() {
            var os = Environment.OSVersion;
            
            if (os.Platform != PlatformID.Win32NT)
                return new Other();

            if (os.Version.Major == 6) {
                if (os.Version.Minor >= 1)
                    return new WindowsSeven();
                else
                    return new WindowsVista();
            }
            else {
                //Generic NT
                return new WindowsXp();
            }
        }

        /// <summary>
        /// Checks whether OnTopReplica is compatible with the platform.
        /// </summary>
        /// <returns>Returns false if OnTopReplica cannot run.</returns>
        public abstract bool CheckCompatibility();

        /// <summary>
        /// Initialized the application. Called once in the app lifetime.
        /// </summary>
        public virtual void InitApp() {
        }

        protected MainForm Form { get; private set; }

        /// <summary>
        /// Initializes a form.
        /// </summary>
        /// <param name="form">Form to initialize.</param>
        public virtual void InitForm(MainForm form) {
            Form = form;
        }

        public virtual void ShutdownApp() {
        }

        #region IDisposable Members

        bool _isDisposed = false;

        public void Dispose() {
            if (_isDisposed)
                return;

            this.ShutdownApp();
            _isDisposed = true;
        }

        #endregion
    }
}
