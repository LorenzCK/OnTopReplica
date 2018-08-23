using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Platforms;
using System.Windows.Forms;

namespace OnTopReplica.Platforms {

    abstract class PlatformSupport {

        /// <summary>
        /// Creates a concrete PlatformSupport instance based on the OS the app is running on.
        /// </summary>
        public static PlatformSupport Create() {
            var os = Environment.OSVersion;
            var platform = CreateFromOperatingSystem(os);

            Log.Write("{0} detected, using support class {1}",
                os.VersionString, platform.GetType().FullName);

            return platform;
        }

        private static PlatformSupport CreateFromOperatingSystem(OperatingSystem os) {
            if (os.Platform != PlatformID.Win32NT)
                return new Other();

            if(os.Version.Major == 10) {
                return new WindowsTen();
            }
            else if (os.Version.Major == 6) {
                if (os.Version.Minor >= 2)
                    return new WindowsEight();
                else if (os.Version.Minor == 1)
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
        /// <returns>Returns false if OnTopReplica cannot run and should terminate right away.</returns>
        public abstract bool CheckCompatibility();

        /// <summary>
        /// Initialization step to run before the form is constructed.
        /// </summary>
        public virtual void PreHandleFormInit() {
        }

        /// <summary>
        /// Initialization step to run just after the form is constructed and WinForms
        /// components have initialized.
        /// </summary>
        public virtual void PostFormInit(MainForm form) {
        }

        /// <summary>
        /// Initializes a form after its handle has been created.
        /// </summary>
        public virtual void PostHandleFormInit(MainForm form) {
        }

        /// <summary>
        /// Called before closing a form. Called once during a form's lifetime.
        /// </summary>
        public virtual void CloseForm(MainForm form) {
        }

        /// <summary>
        /// Hides the main form in a way that it can be restored later by the user.
        /// </summary>
        /// <param name="form">Form to hide.</param>
        public virtual void HideForm(MainForm form) {
        }

        /// <summary>
        /// Gets whether the form is currently hidden or not.
        /// </summary>
        public virtual bool IsHidden(MainForm form) {
            return false;
        }

        /// <summary>
        /// Restores the main form to its default state after is has been hidden.
        /// Can be called whether the form is hidden or not.
        /// </summary>
        /// <param name="form">Form to restore.</param>
        public virtual void RestoreForm(MainForm form) {
        }

        /// <summary>
        /// Called when the form changes its state, without calling into <see cref="RestoreForm"/> or <see cref="HideForm"/>.
        /// Enables inheritors to update the form's state on each state change.
        /// </summary>
        public virtual void OnFormStateChange(MainForm form) {
        }

    }
}
