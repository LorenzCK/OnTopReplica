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

            //Skip non top-level windows
            if (!WindowManagerMethods.IsTopLevel(hwnd))
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
