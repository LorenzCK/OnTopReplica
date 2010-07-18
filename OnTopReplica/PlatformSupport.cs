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
            else if (os.Version.Major > 6) {
                //Ensures forward compatibility
                return new WindowsSeven();
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
        /// Initializes the application. Called once in the app lifetime.
        /// </summary>
        public virtual void InitApp() {
        }

        /// <summary>
        /// Gets the main OnTopReplica form.
        /// </summary>
        protected MainForm Form { get; private set; }

        /// <summary>
        /// Initializes a form. Called once in the form lifetime.
        /// </summary>
        /// <param name="form">Form to initialize.</param>
        public virtual void InitForm(MainForm form) {
            Form = form;
        }

        /// <summary>
        /// Prepares the app for shutdown. Called once before the program terminates.
        /// </summary>
        public virtual void ShutdownApp() {
        }

        /// <summary>
        /// Hides the main form in a way that it can be restored later by the user.
        /// </summary>
        /// <param name="form">Form to hide.</param>
        public virtual void HideForm(MainForm form) {
            form.Hide();
        }

        /// <summary>
        /// Gets whether the form is currently hidden or not.
        /// </summary>
        public virtual bool IsHidden(MainForm form) {
            return form.Visible;
        }

        /// <summary>
        /// Restores the main form to its default state after is has been hidden.
        /// Can be called whether the form is hidden or not.
        /// </summary>
        /// <param name="form">Form to restore.</param>
        public virtual void RestoreForm(MainForm form) {
            form.Show();
        }

        /// <summary>
        /// Called when the form changes its state, without calling into <see cref="RestoreForm"/> or <see cref="HideForm"/>.
        /// Enables inheritors to update the form's state on each state change.
        /// </summary>
        public virtual void OnFormStateChange(MainForm form) {
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
