using System;
using System.Windows.Forms;
using OnTopReplica.Native;
using VistaControls.Dwm;

namespace OnTopReplica.Platforms {

    class WindowsSeven : WindowsVista {

        public override void PreHandleFormInit() {
            //Set Application ID
            WindowsSevenMethods.SetCurrentProcessExplicitAppUserModelID("LorenzCunoKlopfenstein.OnTopReplica.MainForm");
        }

        public override void PostHandleFormInit(MainForm form) {
            DwmManager.SetWindowFlip3dPolicy(form, Flip3DPolicy.ExcludeAbove);
            DwmManager.SetExludeFromPeek(form, true);
            DwmManager.SetDisallowPeek(form, true);

            SetWindowStyle(form);
        }

        public override void HideForm(MainForm form) {
            form.Opacity = 0;
        }

        public override bool IsHidden(MainForm form) {
            return (form.Opacity == 0.0);
        }

        public override void RestoreForm(MainForm form) {
            if (form.Opacity == 0.0)
                form.Opacity = 1.0;
            
            form.Show();
            
            SetWindowStyle(form);
        }

        public override void OnFormStateChange(MainForm form) {
            SetWindowStyle(form);
        }

        private void SetWindowStyle(MainForm form) {
            if (!form.IsFullscreen) {
                //This hides the app from ALT+TAB
                //Note that when minimized, it will be shown as an (ugly) minimized tool window
                //thus we do not minimize, but set to transparent when hiding
                long exStyle = WindowMethods.GetWindowLong(form.Handle, WindowMethods.WindowLong.ExStyle).ToInt64();

                exStyle |= (long)(WindowMethods.WindowExStyles.ToolWindow);
                exStyle &= ~(long)(WindowMethods.WindowExStyles.AppWindow);

                WindowMethods.SetWindowLong(form.Handle, WindowMethods.WindowLong.ExStyle, new IntPtr(exStyle));

                //WindowMethods.SetWindowLong(form.Handle, WindowMethods.WindowLong.HwndParent, WindowManagerMethods.GetDesktopWindow());
            }
        }


    }

}
