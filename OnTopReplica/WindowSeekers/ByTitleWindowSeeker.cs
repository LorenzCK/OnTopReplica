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

            TitleMatch = titleSeekString.Trim().ToLowerInvariant();
        }

        public string TitleMatch { get; private set; }

        protected override int EvaluatePoints(WindowHandle handle) {
            //Skip empty titles
            if (string.IsNullOrEmpty(handle.Title))
                return -1;

            //Skip non top-level windows
            if (!WindowManagerMethods.IsTopLevel(handle.Handle))
                return -1;

            string handleTitle = handle.Title.ToLowerInvariant();
            int points = 0;

            //Give points for partial match
            if (handleTitle.Equals(TitleMatch)) {
                points += 20;
            }
            else if (handleTitle.StartsWith(TitleMatch)) {
                points += 15;
            }
            else if (handleTitle.Contains(TitleMatch)) {
                points += 10;
            }

            return points;
        }
    }
}
