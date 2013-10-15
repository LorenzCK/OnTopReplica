using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;

namespace OnTopReplica.WindowSeekers {
    
    /// <summary>
    /// Base class for window seekers that can populate a list of window handles based on some criteria and with basic filtering.
    /// </summary>
    abstract class BaseWindowSeeker : IWindowSeeker {

        #region IWindowSeeker

        /// <summary>
        /// Get the matching windows from the last refresh.
        /// </summary>
        public abstract IList<WindowHandle> Windows {
            get;
        }

        /// <summary>
        /// Forces a window list refresh.
        /// </summary>
        public virtual void Refresh() {
            WindowManagerMethods.EnumWindows(RefreshCallback, IntPtr.Zero);
        }

        #endregion

        private bool RefreshCallback(IntPtr hwnd, IntPtr lParam) {
            //Skip owner
            if (hwnd == OwnerHandle)
                return true;

            if (SkipNotVisibleWindows && !WindowManagerMethods.IsWindowVisible(hwnd))
                return true;

            //Extract basic properties
            string title = WindowMethods.GetWindowText(hwnd);
            var handle = new WindowHandle(hwnd, title);

            return InspectWindow(handle);
        }

        /// <summary>
        /// Inspects a window and return whether inspection should continue.
        /// </summary>
        /// <param name="handle">Handle of the window.</param>
        /// <returns>True if inspection should continue. False stops current refresh operation.</returns>
        protected abstract bool InspectWindow(WindowHandle handle);

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
