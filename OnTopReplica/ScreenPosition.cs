using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OnTopReplica {
    /// <summary>
    /// Describes a resolution independent position.
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
        /// Sets the form's screen position in independent coordinates.
        /// </summary>
        /// <remarks>
        /// Position is set relative to the form's current screen.
        /// </remarks>
        public static void SetScreenPosition(this MainForm form, ScreenPosition position) {
            var screen = Screen.FromControl(form);
            var wa = screen.WorkingArea;

            Point p = new Point();
            switch (position) {
                case ScreenPosition.TopLeft:
                    p = new Point(
                        wa.Left - form.ChromeBorderHorizontal,
                        wa.Top - form.ChromeBorderVertical
                    );
                    break;

                case ScreenPosition.TopRight:
                    p = new Point(
                        wa.Right - form.Width + form.ChromeBorderHorizontal,
                        wa.Top - form.ChromeBorderVertical
                    );
                    break;

                case ScreenPosition.BottomLeft:
                    p = new Point(
                        wa.Left - form.ChromeBorderHorizontal,
                        wa.Bottom - form.Height + form.ChromeBorderVertical
                    );
                    break;

                case ScreenPosition.BottomRight:
                    p = new Point(
                        wa.Right - form.Width + form.ChromeBorderHorizontal,
                        wa.Bottom - form.Height + form.ChromeBorderVertical
                    );
                    break;

                case ScreenPosition.Center:
                    p = new Point(
                        wa.X + (wa.Width / 2) - (form.Width / 2) - (form.ChromeBorderHorizontal / 2),
                        wa.Y + (wa.Height / 2) - (form.Height / 2) - (form.ChromeBorderVertical / 2)
                    );
                    break;
            }

            form.Location = p;
        }

    }

}
