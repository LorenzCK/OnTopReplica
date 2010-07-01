using System;
using System.Windows.Forms;
using OnTopReplica.Native;
using VistaControls.Dwm;

namespace OnTopReplica.Platforms {
    class WindowsSeven : WindowsVista {

        public override void InitForm(MainForm form) {
            DwmManager.SetWindowFlip3dPolicy(form, Flip3DPolicy.ExcludeAbove);
            DwmManager.SetExludeFromPeek(form, true);
            DwmManager.SetDisallowPeek(form, true);

            //This hides the app from ALT+TAB, but when minimized the window is shrunk on the bottom, right above the task bar (ugly)
            /*Native.WindowMethods.SetWindowLong(form.Handle, WindowMethods.WindowLong.ExStyle,
                            (IntPtr)WindowMethods.WindowExStyles.ToolWindow);*/

            //This adds the task bar item, but hiding/showing again adds it back to alt+tab
            /*var list = (ITaskbarList)new CoTaskbarList();
            list.HrInit();
            list.AddTab(form.Handle);
            list.ActivateTab(form.Handle);            */
        }

        public override void InitApp() {
            //Set Application ID
            WindowsSevenMethods.SetCurrentProcessExplicitAppUserModelID("OnTopReplica");
        }

        public override void HideForm(MainForm form) {
            form.WindowState = FormWindowState.Minimized;
        }

    }
}
