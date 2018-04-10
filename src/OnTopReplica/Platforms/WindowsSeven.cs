using System;
using System.Windows.Forms;
using OnTopReplica.Native;
using WindowsFormsAero.Dwm;

namespace OnTopReplica.Platforms {

    class WindowsSeven : WindowsVista {

        private double? PreviousOpacity { get; set; }

        public override void PreHandleFormInit() {
            //Set Application ID
            WindowsSevenMethods.SetCurrentProcessExplicitAppUserModelID("LorenzCunoKlopfenstein.OnTopReplica.MainForm");
        }

        public override void PostHandleFormInit(MainForm form) {
            DwmManager.SetWindowFlip3dPolicy(form, WindowsFormsAero.Flip3DPolicy.ExcludeAbove);
            DwmManager.SetExcludeFromPeek(form, true);
            DwmManager.SetDisallowPeek(form, true);
        }

        public override void HideForm(MainForm form) {
            PreviousOpacity = form.Opacity;
            form.Opacity = 0;
        }

        public override bool IsHidden(MainForm form) {
            return (form.Opacity == 0.0);
        }

        public override void RestoreForm(MainForm form) {
            if (form.Opacity == 0.0) {
                form.Opacity = PreviousOpacity.GetValueOrDefault(1.0);
                PreviousOpacity = null;
            }
            
            form.Show();
        }

    }

}
