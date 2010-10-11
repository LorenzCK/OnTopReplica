using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;

namespace OnTopReplica.WindowSeekers {
    /// <summary>
    /// Seeks a single window by approximately matching its title.
    /// </summary>
    /// <remarks>
    /// Title search is case-insensitive and matches only the beginning of the windows' titles.
    /// </remarks>
    class ByTitleWindowSeeker : BaseWindowSeeker {

        public ByTitleWindowSeeker(string titleSeekString) {
            if (titleSeekString == null)
                throw new ArgumentNullException();

            TitleMatch = titleSeekString.Trim().ToLower();
        }

        public string TitleMatch { get; private set; }

        protected override bool InspectWindow(IntPtr hwnd, string title, ref bool terminate) {
            //Skip empty titles
            if (string.IsNullOrEmpty(title))
                return false;

            //Skip non-top-level windows (skips most internal controls)
            bool hasParent = (long)WindowManagerMethods.GetParent(hwnd) != 0;
            bool hasOwner = (long)WindowManagerMethods.GetWindow(hwnd, WindowManagerMethods.GetWindowMode.GW_OWNER) != 0;
            if (hasParent || hasOwner)
                return false;

            var modTitle = title.Trim().ToLower();
            if (modTitle.StartsWith(TitleMatch)) {
                terminate = true; //only one needed
                return true;
            }

            return false;
        }

    }
}
