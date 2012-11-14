using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OnTopReplica {

    /// <summary>
    /// Describes a resolution independent screen position.
    /// </summary>
    enum ScreenPosition {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Center
    }

    /// <summary>
    /// Extension methods for ScreenPositions.
    /// </summary>
    static class ScreenPositionExtensions {

        /// <summary>
        /// Gets the coordinates point matching an independent screen position value.
        /// </summary>
        /// <param name="position">Screen position value.</param>
        /// <returns>Pixel point in screen coordinates.</returns>
        public static Point ResolveScreenPosition(this Screen screen, ScreenPosition position) {
            var rect = screen.WorkingArea;

            return ResolveScreenPositionToRectangle(rect, position);
        }

        /// <summary>
        /// Gets the coordinates matching an independent screen position value.
        /// </summary>
        /// <param name="ctrl">Target control for which the coordinates should be resolved.</param>
        /// <param name="position">Screen position value.</param>
        /// <returns>Pixel point in screen coordinates.</returns>
        public static Point ResolveScreenPositionEdge(this Control ctrl, ScreenPosition position) {
            var ctrlRegion = ctrl.RectangleToScreen(ctrl.ClientRectangle);

            return ResolveScreenPositionToRectangle(ctrlRegion, position);
        }

        private static Point ResolveScreenPositionToRectangle(Rectangle rect, ScreenPosition position) {
            switch (position) {
                case ScreenPosition.TopLeft:
                    return new Point(rect.X, rect.Y);
                case ScreenPosition.TopRight:
                    return new Point(rect.X + rect.Width, rect.Y);
                case ScreenPosition.BottomLeft:
                    return new Point(rect.X, rect.Y + rect.Height);
                case ScreenPosition.BottomRight:
                    return new Point(rect.X + rect.Width, rect.Y + rect.Height);
                case ScreenPosition.Center:
                    return new Point(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
                default:
                    throw new ArgumentException("Invalid ScreenPosition value.");
            }
        }

        /// <summary>
        /// Sets the form's screen position in independent coordinates.
        /// </summary>
        /// <remarks>
        /// Position is set relative to the form's current screen.
        /// </remarks>
        public static void SetScreenPosition(this MainForm form, ScreenPosition position) {
            var screen = Screen.FromControl(form);

            var start = form.ResolveScreenPositionEdge(position);
            var end = screen.ResolveScreenPosition(position);

            var move = end.Difference(start);

            System.Diagnostics.Trace.WriteLine(string.Format("From {0} to {1} => {2}.", start, end, move));

            var original = form.Location;
            form.Location = new Point(original.X + move.X, original.Y + move.Y);
        }

    }

}
