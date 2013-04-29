using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace OnTopReplica {

    /// <summary>
    /// Represents a thumbnail region.
    /// </summary>
    /// <remarks>
    /// A ThumbnailRegion can work in absolute or in relative mode.
    /// In absolute mode, the region is expressed in absolute pixel values, as expressed by the value of the
    /// <see cref="Bounds"/> property.
    /// In relative mode, the region is expressed in padding pixels from the borders of the source. Internally this
    /// is still represented by the <see cref="Bounds"/> property. Properties of the Rectangle value are mapped as follows:
    /// Rectangle.X = Padding.Left
    /// Rectangle.Y = Padding.Top
    /// Rectangle.Width = Padding.Right
    /// Rectangle.Height = Padding.Bottom
    /// </remarks>
    public class ThumbnailRegion {

        /// <summary>
        /// Creates a ThumbnailRegion from a padding value relative to the thumbnail borders.
        /// </summary>
        public ThumbnailRegion(Padding padding) {
            _bounds = new Rectangle {
                X = padding.Left,
                Y = padding.Top,
                Width = padding.Right,
                Height = padding.Bottom
            };
            Relative = true;
        }

        /// <summary>
        /// Creates a ThumbnailRegion from a bounds rectangle (in absolute terms).
        /// </summary>
        public ThumbnailRegion(Rectangle rectangle) {
            _bounds = rectangle;
            Relative = false;
        }

        /// <summary>
        /// Creates a ThumbnailRegion from a rectangle, either expressing values in relative or in absolute terms.
        /// </summary>
        public ThumbnailRegion(Rectangle paddingOrBounds, bool relative) {
            _bounds = paddingOrBounds;
            Relative = relative;
        }

        private Rectangle _bounds;

        /// <summary>
        /// Gets or sets the bounds of the thumbnail region.
        /// </summary>
        public Rectangle Bounds {
            get {
#if DEBUG
                if (Relative)
                    throw new InvalidOperationException("Not allowed to use ThumbnailRegion Bounds as Rectangle value (in relative mode).");
#endif

                return _bounds;
            }
            set {
                _bounds = value;
                Relative = false;
            }
        }

        /// <summary>
        /// Gets or sets whether the bounds are expressed relative to the thumbnail borders.
        /// </summary>
        public bool Relative {
            get;
            set;
        }

        /// <summary>
        /// Sets the relative bounds of the region. Switches to relative mode.
        /// </summary>
        /// <param name="padding">Padding in relative terms from the borders.</param>
        public void SetRelativeBounds(Padding padding) {
            Bounds = new Rectangle {
                X = padding.Left,
                Y = padding.Top,
                Width = padding.Right,
                Height = padding.Bottom
            };
            Relative = true;
        }

        /// <summary>
        /// Gets the bounds of the thumbnail region as relative padding from the thumbnail borders.
        /// </summary>
        /// <remarks>Makes sense only in relative mode.</remarks>
        public Padding BoundsAsPadding {
            get {
#if DEBUG
                if (!Relative)
                    throw new InvalidOperationException("Not allowed to use ThumbnailRegion Bounds as Padding value (not in relative mode).");
#endif

                return new Padding {
                    Left = _bounds.X,
                    Top = _bounds.Y,
                    Right = _bounds.Width,
                    Bottom = _bounds.Height
                };
            }
        }

        /// <summary>
        /// Gets the offset of the region.
        /// </summary>
        /// <remarks>
        /// The offset is expressed as a point of displacement from the up-right corner (0,0) of the original source.
        /// </remarks>
        public Point Offset {
            get {
                //This is equal in both absolute and relative mode
                return _bounds.Location;
            }
        }

        const int MinimumRegionSize = 8;

        /// <summary>
        /// Computes the effective region representing the bounds inside a source thumbnail of a certain size.
        /// </summary>
        /// <param name="sourceSize">Size of the full thumbnail source.</param>
        /// <returns>Bounds inside the thumbnail.</returns>
        protected Rectangle ComputeRegion(Size sourceSize) {
            Rectangle ret;

            //Compute
            if (Relative) {
                ret = new Rectangle {
                    X = _bounds.X,
                    Y = _bounds.Y,
                    Width = sourceSize.Width - _bounds.X - _bounds.Width,
                    Height = sourceSize.Height - _bounds.Y - _bounds.Height
                };
            }
            else {
                ret = _bounds;
            }

            //Constrain to bounds
            if (ret.X + ret.Width > sourceSize.Width)
                ret.Width = sourceSize.Width - ret.X;
            if (ret.Y + ret.Height > sourceSize.Height)
                ret.Height = sourceSize.Height - ret.Y;

            return ret;
        }

        /// <summary>
        /// Computes a rectangle representing the bounds of the region inside a source thumbnail of a certain size.
        /// </summary>
        /// <param name="sourceSize">Size of the full thumbnail source.</param>
        /// <returns>Bounds inside the thumbnail.</returns>
        public Rectangle ComputeRegionRectangle(Size sourceSize) {
            return ComputeRegion(sourceSize);
        }

        /// <summary>
        /// Computes a value representing the size of the region inside a source thumbnail of a certain size.
        /// </summary>
        /// <param name="sourceSize">Size of the full thumbnail source.</param>
        /// <returns>Size of the bounds inside the thumbnail.</returns>
        public Size ComputeRegionSize(Size sourceSize) {
            return ComputeRegion(sourceSize).Size;
        }

        /// <summary>
        /// Switches the region to relative mode, according to a source thumbnail of a given size.
        /// </summary>
        /// <param name="sourceSize">Size of the full thumbnail source.</param>
        public void SwitchToRelative(Size sourceSize) {
            if (Relative)
                return;

            var relativeBounds = new Padding {
                Left = _bounds.X,
                Top = _bounds.Y,
                Right = sourceSize.Width - (_bounds.X + _bounds.Width),
                Bottom = sourceSize.Height - (_bounds.Y + _bounds.Height)
            };

            this.SetRelativeBounds(relativeBounds);
        }

        /// <summary>
        /// Switches the region to absolute mode, according to a source thumbnail of a given size.
        /// </summary>
        /// <param name="sourceSize">Size of the full thumbnail source.</param>
        public void SwitchToAbsolute(Size sourceSize) {
            if (!Relative)
                return;

            var absoluteBounds = new Rectangle {
                X = _bounds.X,
                Y = _bounds.Y,
                Width = (sourceSize.Width - _bounds.Width) - _bounds.X,
                Height = (sourceSize.Height - _bounds.Height) - _bounds.Y
            };

            Bounds = absoluteBounds;
        }

        public override string ToString() {
            return string.Format("({0}, {1})", _bounds, (Relative) ? "relative" : "absolute");
        }

    }

}
