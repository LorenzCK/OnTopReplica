using System.Windows.Forms;
using OnTopReplica.Native;
using WindowsFormsAero.Dwm;

namespace OnTopReplica.Platforms {

    class WindowsTen : PlatformSupport {

        public override bool CheckCompatibility() {
            return true;
        }

        public override void PreHandleFormInit() {
            WindowsSevenMethods.SetCurrentProcessExplicitAppUserModelID(Program.ApplicationId);
        }

        public override void PostFormInit(MainForm form) {
            base.PostFormInit(form);

            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.ControlBox = true;
            form.HideCaption = false;
        }

        public override void PostHandleFormInit(MainForm form) {
            DwmManager.SetExcludeFromPeek(form, true);
            DwmManager.SetDisallowPeek(form, true);
        }

    }

}
