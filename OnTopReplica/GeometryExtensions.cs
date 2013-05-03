using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

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

        /// <summary>
        /// Expands a size value by a padding distance.
        /// </summary>
        public static Size Expand(this Size size, Padding padding) {
            return new Size(size.Width + padding.Horizontal, size.Height + padding.Vertical);
        }

        /// <summary>
        /// Expands a size value by a size distance.
        /// </summary>
        public static Size Expand(this Size size, Size expandSize) {
            return new Size(size.Width + expandSize.Width, size.Height + expandSize.Height);
        }

        /// <summary>
        /// Computes the difference between two size values.
        /// </summary>
        public static Size Difference(this Size a, Size b) {
            return new Size(a.Width - b.Width, a.Height - b.Height);
        }

        /// <summary>
        /// Ensures that the minimum size of a control respects a minimum
        /// client size area.
        /// </summary>
        /// <param name="ctrl">Control whose MinimumSize should be altered.</param>
        /// <param name="minimumClientSize">Minimum client size value to ensure.</param>
        public static void EnsureMinimumClientSize(this Control ctrl, Size minimumClientSize) {
            Size offset = ctrl.Size.Difference(ctrl.ClientSize);
            ctrl.MinimumSize = minimumClientSize.Expand(offset);
        }

        /// <summary>
        /// Attempts to fit a size structure to another fixed destination size, by maintaining
        /// the original aspect ratio.
        /// </summary>
        public static Size Fit(this Size sourceSize, Size destinationSize) {
            double sourceRatio = (double)sourceSize.Width / (double)sourceSize.Height;
            double clientRatio = (double)destinationSize.Width / (double)destinationSize.Height;

            Size ret;
            if (sourceRatio >= clientRatio) {
                ret = new Size(destinationSize.Width, (int)((double)destinationSize.Width / sourceRatio));
            }
            else {
                ret = new Size((int)((double)destinationSize.Height * sourceRatio), destinationSize.Height);
            }

            return ret;
        }

    }

}
