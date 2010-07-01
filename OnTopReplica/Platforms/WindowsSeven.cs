using System.Windows.Forms;
using OnTopReplica.Native;
using VistaControls.Dwm;

namespace OnTopReplica.Platforms {
    class WindowsSeven : WindowsVista {

        public override void InitForm(MainForm form) {
            DwmManager.SetExludeFromPeek(form, true);
            DwmManager.SetDisallowPeek(form, true);
            
            base.InitForm(form);
        }

        public override void InitApp() {
            //Set Application ID
            WindowsSevenMethods.SetCurrentProcessExplicitAppUserModelID("OnTopReplica");
        }

        protected override void InitFormCore(MainForm form) {
            //do nothing
        }

        public override void HideForm(MainForm form) {
            form.WindowState = FormWindowState.Minimized;
        }

    }
}
