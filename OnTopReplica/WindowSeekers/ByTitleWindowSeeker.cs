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
    class ByTitleWindowSeeker : PointBasedWindowSeeker {

        public ByTitleWindowSeeker(string titleSeekString) {
            if (titleSeekString == null)
                throw new ArgumentNullException();

            TitleMatch = titleSeekString.Trim();
        }

        public string TitleMatch { get; private set; }

        protected override int EvaluatePoints(WindowHandle handle) {
            //Skip empty titles
            if (string.IsNullOrEmpty(handle.Title))
                return -1;

            //Skip non top-level windows
            if (!WindowManagerMethods.IsTopLevel(handle.Handle))
                return -1;

            int points = 0;

            //Give points for partial match
            if (handle.Title.StartsWith(TitleMatch, StringComparison.InvariantCultureIgnoreCase))
                points += 10;

            //Give points for exact match
            if (handle.Title.Equals(TitleMatch, StringComparison.InvariantCultureIgnoreCase))
                points += 10;

            return points;
        }
    }
}
