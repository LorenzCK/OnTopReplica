using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;

namespace OnTopReplica.WindowSeekers {
    /// <summary>
    /// Seeks a single window by matching its window class.
    /// </summary>
    /// <remarks>
    /// Class matching is case-sensitive and prefers perfect matches, also accepting
    /// partial matches (when the class matches the beginning of the target class name).
    /// </remarks>
    class ByClassWindowSeeker : PointBasedWindowSeeker {

        public ByClassWindowSeeker(string className) {
            if (className == null)
                throw new ArgumentNullException();

            ClassName = className;
        }

        public string ClassName { get; private set; }

        protected override int EvaluatePoints(WindowHandle handle) {
            if(string.IsNullOrEmpty(handle.Class))
                return -1;

            int points = 0;

            //Partial match
            if (handle.Class.StartsWith(ClassName, StringComparison.InvariantCulture))
                points += 10;

            if (handle.Class.Equals(ClassName, StringComparison.InvariantCulture))
                points += 10;

            return points;
        }
    }
}
