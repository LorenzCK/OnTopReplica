using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OnTopReplica {
    
    /// <summary>
    /// Common geometry extension methods.
    /// </summary>
    static class GeometryExtensions {

        /// <summary>
        /// Returns the difference (offset vector) between two points.
        /// </summary>
        public static Point Difference(this Point a, Point b) {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

    }

}
