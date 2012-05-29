using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;

namespace OnTopReplica.WindowSeekers {
    class TaskWindowSeeker : BaseWindowSeeker {
        
        protected override bool InspectWindow(IntPtr hwnd, string title, ref bool terminate) {
            //Code taken from: http://www.thescarms.com/VBasic/alttab.aspx

            //Reject empty titles
            if (string.IsNullOrEmpty(title))
                return false;

            //Accept windows that
            // - are visible
            // - do not have a parent
            // - have no owner and are not Tool windows OR
            // - have an owner and are App windows
            if ((long)WindowManagerMethods.GetParent(hwnd) == 0) {
                bool hasOwner = (long)WindowManagerMethods.GetWindow(hwnd, WindowManagerMethods.GetWindowMode.GW_OWNER) != 0;
                WindowMethods.WindowExStyles exStyle = (WindowMethods.WindowExStyles)WindowMethods.GetWindowLong(hwnd, WindowMethods.WindowLong.ExStyle);

                if (((exStyle & WindowMethods.WindowExStyles.ToolWindow) == 0 && !hasOwner) || //unowned non-tool window
                    ((exStyle & WindowMethods.WindowExStyles.AppWindow) == WindowMethods.WindowExStyles.AppWindow && hasOwner)) { //owned application window
                    return true;
                }
            }

            return false;
        }

    }
}
