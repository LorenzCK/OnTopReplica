using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnTopReplica.WindowSeekers {
    /// <summary>
    /// Interface for window seekers.
    /// </summary>
    interface IWindowSeeker {

        /// <summary>
        /// Get the list of matching windows, ordered by priority (optionally).
        /// </summary>
        IList<WindowHandle> Windows { get; }

        /// <summary>
        /// Refreshes the list of windows.
        /// </summary>
        void Refresh();

    }
}
