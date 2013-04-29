using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;

namespace OnTopReplica.WindowSeekers {
    /// <summary>
    /// Window seeker that attempts to mimic ALT+TAB behavior in filtering windows to show.
    /// </summary>
    class TaskWindowSeeker : BaseWindowSeeker {
        
        List<WindowHandle> _list = new List<WindowHandle>();

        public override IList<WindowHandle> Windows {
            get {
                return _list;
            }
        }

        public override void Refresh() {
            _list.Clear();

            base.Refresh();
        }

        protected override bool InspectWindow(WindowHandle handle) {
            //Code taken from: http://www.thescarms.com/VBasic/alttab.aspx

            //Reject empty titles
            if (string.IsNullOrEmpty(handle.Title))
                return true;

            //Accept windows that
            // - are visible
            // - do not have a parent
            // - have no owner and are not Tool windows OR
            // - have an owner and are App windows
            if ((long)WindowManagerMethods.GetParent(handle.Handle) == 0) {
                bool hasOwner = (long)WindowManagerMethods.GetWindow(handle.Handle, WindowManagerMethods.GetWindowMode.GW_OWNER) != 0;
                WindowMethods.WindowExStyles exStyle = (WindowMethods.WindowExStyles)WindowMethods.GetWindowLong(handle.Handle, WindowMethods.WindowLong.ExStyle);

                if (((exStyle & WindowMethods.WindowExStyles.ToolWindow) == 0 && !hasOwner) || //unowned non-tool window
                    ((exStyle & WindowMethods.WindowExStyles.AppWindow) == WindowMethods.WindowExStyles.AppWindow && hasOwner)) { //owned application window

                    _list.Add(handle);
                }
            }

            return true;
        }
    }
}
