using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;

namespace OnTopReplica.WindowSeekers {
    
    /// <summary>
    /// Base class for window seekers that can populate a list of window handles based on some criteria.
    /// </summary>
    abstract class BaseWindowSeeker {

        List<WindowHandle> _list = new List<WindowHandle>();

        /// <summary>
        /// Get the matching windows from the last refresh.
        /// </summary>
        public IEnumerable<WindowHandle> Windows {
            get {
                return _list;
            }
        }

        /// <summary>
        /// Forces a window list refresh.
        /// </summary>
        public virtual void Refresh() {
            _list.Clear();

            WindowManagerMethods.EnumWindows(
                new WindowManagerMethods.EnumWindowsProc(RefreshCallback),
                IntPtr.Zero);
        }

        private bool RefreshCallback(IntPtr hwnd, IntPtr lParam) {
            bool cont = true;

            //Skip owner
            if (hwnd == OwnerHandle)
                return true;

            if (SkipNotVisibleWindows && !WindowManagerMethods.IsWindowVisible(hwnd))
                return true;

            //Extract basic properties
            string title = WindowMethods.GetWindowText(hwnd);

            if (InspectWindow(hwnd, title, ref cont)) {
                //Window has been picked
                _list.Add(new WindowHandle(hwnd, title));
            }

            return cont;
        }

        /// <summary>
        /// Inspects a window and returns whether the window should be listed or not.
        /// </summary>
        /// <param name="hwnd">Handle of the window.</param>
        /// <param name="title">Title of the window (if any).</param>
        /// <param name="terminate">Indicates whether the inspection loop should terminate after this window.</param>
        /// <returns>True if the window should be listed.</returns>
        protected abstract bool InspectWindow(IntPtr hwnd, string title, ref bool terminate);

        /// <summary>
        /// Gets or sets the window handle of the owner.
        /// </summary>
        /// <remarks>
        /// Windows with this handle will be automatically skipped.
        /// </remarks>
        public IntPtr OwnerHandle { get; set; }

        /// <summary>
        /// Gets or sets whether not visible windows should be skipped.
        /// </summary>
        public bool SkipNotVisibleWindows { get; set; }

    }

}
