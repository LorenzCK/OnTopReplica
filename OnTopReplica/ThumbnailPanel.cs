using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using VistaControls.Dwm;
using VistaControls.ThemeText;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using OnTopReplica.Native;

namespace OnTopReplica {

	class ThumbnailPanel : Panel {

		//DWM Thumbnail stuff
		Thumbnail _thumbnail = null;
		Rectangle _regionCurrent;

		//Labels
		ThemedLabel _labelGlass;

		public ThumbnailPanel() {
			InitFormComponents();
		}

		private void InitFormComponents() {
            BackColor = Color.Black;

			//Themed Label
            _labelGlass = new ThemedLabel {
                Dock = DockStyle.Fill,
                ForeColor = SystemColors.ControlText,
                Location = Point.Empty,
                Size = ClientSize,
                Name = "labelGlass",
                Text = Strings.RightClick,
                TextAlign = HorizontalAlignment.Center,
                TextAlignVertical = VerticalAlignment.Center
            };
			this.Controls.Add(_labelGlass);
		}

		#region Properties and settings

        /// <summary>
        /// Gets or sets the region that is currently shown on the thumbnail. When set, also enabled region constrain.
        /// </summary>
		public Rectangle SelectedRegion {
			get {
				return _regionCurrent;
			}
			set {
				_regionCurrent = value;
                ConstrainToRegion = true;
			}
		}

        bool _regionEnabled = false;

        /// <summary>
        /// Gets or sets whether the thumbnail is constrained to a region or not.
        /// </summary>
		public bool ConstrainToRegion {
			get {
				return _regionEnabled;
			}
			set {
				_regionEnabled = value;
				UpdateThubmnail();
			}
		}

		bool _drawMouseRegions = false;

        /// <summary>
        /// Gets or sets whether the control is is "region drawing" mode and reports them via events.
        /// </summary>
		public bool DrawMouseRegions {
			get {
				return _drawMouseRegions;
			}
			set {
				//Set mode and reset region
				_drawMouseRegions = value;
				_drawingRegion = false;

				//Cursor change
				Cursor = (value) ? Cursors.Cross : Cursors.Default;

                //Refresh gui
				UpdateThubmnail();
                _labelGlass.Visible = !value;
                this.Invalidate();
			}
		}

        /// <summary>
        /// Gets the target opacity of the thumbnail, depending on the control's state.
        /// </summary>
		protected byte ThumbnailOpacity {
			get {
                return (_drawMouseRegions) ? (byte)130 : (byte)255;
			}
		}

        /// <summary>
        /// Gets or sets whether the control should report clicks made on the cloned thumbnail.
        /// </summary>
        public bool ReportThumbnailClicks {
            get;
            set;
        }

        /// <summary>
        /// Gets the thumbnail's original size.
        /// </summary>
        public Size ThumbnailOriginalSize {
            get {
                if (_thumbnail != null && !_thumbnail.IsInvalid) {
                    if (_regionEnabled)
                        return _regionCurrent.Size;

                    return _thumbnail.SourceSize;
                }
                else
                    throw new Exception(Strings.ErrorNoThumbnail);
            }
        }

		#endregion

        #region GUI event handling

        protected override void OnResize(EventArgs eventargs) {
            base.OnResize(eventargs);
            UpdateThubmnail();
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            //Check whether this is a hit-test on "client" surface
            if (m.Msg == WM.NCHITTEST && m.Result.ToInt32() == HT.CLIENT) {
                //Check whether clicks must be reported
                if(!DrawMouseRegions && !ReportThumbnailClicks /*&& !InputMethods.IsKeyPressed(VirtualKeyState.VK_SHIFT)*/){
                    m.Result = new IntPtr(HT.TRANSPARENT);
                }
            }
        }

        #endregion

        #region Thumbnail interface

        /// <summary>
        /// Creates a new thumbnail of a certain window.
        /// </summary>
        /// <param name="handle">Handle of the window to clone.</param>
		public void SetThumbnailHandle(WindowHandle handle) {
			if (_thumbnail != null && !_thumbnail.IsInvalid)
				_thumbnail.Close();

			//Get form and register thumbnail on it
			Form owner = this.TopLevelControl as Form;
			if(owner == null)
				throw new Exception("Internal error: ThumbnailPanel.TopLevelControl is not a Form.");

            _labelGlass.Visible = false;

			_thumbnail = DwmManager.Register(owner, handle.Handle);
            ConstrainToRegion = false; //this also invokes a thumbnail update
		}

        /// <summary>
        /// Disposes current thumbnail and enters stand-by mode.
        /// </summary>
		public void UnsetThumbnail() {
			if (_thumbnail != null && !_thumbnail.IsInvalid)
				_thumbnail.Close();

			_thumbnail = null;
            _labelGlass.Visible = true;
		}

        /// <summary>
        /// Gets whether the control is currently displaying a thumbnail.
        /// </summary>
		public bool IsShowingThumbnail {
			get {
				return (_thumbnail != null && !_thumbnail.IsInvalid);
			}
		}

		int _padWidth = 0;
		int _padHeight = 0;
		Size _thumbnailSize;

		/// <summary>
        /// Updates the thumbnail options and the right-click label.
        /// </summary>
		private void UpdateThubmnail() {
			if (_thumbnail != null && !_thumbnail.IsInvalid){
                try {
                    Size sourceSize = ThumbnailOriginalSize;
                    _thumbnailSize = ComputeIdealSize(sourceSize, Size);

                    _padWidth = (Size.Width - _thumbnailSize.Width) / 2;
                    _padHeight = (Size.Height - _thumbnailSize.Height) / 2;

                    var target = new Rectangle(_padWidth, _padHeight, _thumbnailSize.Width, _thumbnailSize.Height);
                    Rectangle source = (_regionEnabled) ? _regionCurrent : new Rectangle(Point.Empty, _thumbnail.SourceSize);

                    _thumbnail.Update(target, source, ThumbnailOpacity, true, true);
                }
                catch {
                    //Any error updating the thumbnail forces to unset (handle may not be valid anymore)
                    UnsetThumbnail();
                    return;
                }
			}
		}

        /// <summary>
        /// Computes ideal thumbnail size given an original size and a target to fit.
        /// </summary>
        /// <param name="sourceSize">Size of the original thumbnail.</param>
        /// <param name="clientSize">Size of the client area to fit.</param>
        private Size ComputeIdealSize(Size sourceSize, Size clientSize) {
            double sourceRatio = (double)sourceSize.Width / (double)sourceSize.Height;
            double clientRatio = (double)clientSize.Width / (double)clientSize.Height;
            
            Size ret;
            if (sourceRatio >= clientRatio) {
                ret = new Size(clientSize.Width, (int)((double)clientSize.Width / sourceRatio));
            }
            else {
                ret = new Size((int)((double)clientSize.Height * sourceRatio), clientSize.Height);
            }

            return ret;
        }

        #endregion

		#region Region drawing

        //Set if currently drawing a window (first click/drag was initiated)
        bool _drawingRegion = false;
        //Set if drawing was suspended because the mouse left the control
        bool _drawingSuspended = false;
        Point _regionStartPoint;
        Point _regionLastPoint;

        public delegate void RegionDrawnHandler(object sender, Rectangle region);

        public event RegionDrawnHandler RegionDrawn;

        protected virtual void OnRegionDrawn(Rectangle region) {
            //Fix region if necessary (bug report by Gunter, via comment)
            if (region.Width < 1) region.Width = 1;
            if (region.Height < 1) region.Height = 1;

            var evt = RegionDrawn;
            if (evt != null)
                evt(this, region);
        }

        /// <summary>
        /// Raises a RegionDrawn event, given a starting and an ending point of the drawn region.
        /// </summary>
        protected void RaiseRegionDrawn(Point start, Point end) {
            if (_thumbnailSize.Width < 1 || _thumbnailSize.Height < 1) //causes DivBy0
                return;

            //Compute bounds
            int left = Math.Min(start.X, end.X);
            int right = Math.Max(start.X, end.X);
            int top = Math.Min(start.Y, end.Y);
            int bottom = Math.Max(start.Y, end.Y);

            //Clip to boundaries
            left = Math.Max(0, left);
            right = Math.Min(_thumbnailSize.Width, right);
            top = Math.Max(0, top);
            bottom = Math.Min(_thumbnailSize.Height, bottom);

            //Compute region rectangle in thumbnail coordinates
            var startPoint = ClientToThumbnail(new Point(left, top));
            var endPoint = ClientToThumbnail(new Point(right, bottom));
            var final = new Rectangle(
                startPoint,
                new Size(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y)
            );

            //Update region
            SelectedRegion = final;
            OnRegionDrawn(final);
        }

		protected override void OnMouseDown(MouseEventArgs e) {
			if (DrawMouseRegions && e.Button == MouseButtons.Left) {
                //Start new region drawing
				_drawingRegion = true;
                _drawingSuspended = false;
				_regionStartPoint = _regionLastPoint = e.Location;

				this.Invalidate();
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			if (DrawMouseRegions && e.Button == MouseButtons.Left) {
                //Region completed
				_drawingRegion = false;
                _drawingSuspended = false;
				RaiseRegionDrawn(_regionStartPoint, _regionLastPoint);

				this.Invalidate();
			}

			base.OnMouseUp(e);
		}

		protected override void OnMouseLeave(EventArgs e) {
            _drawingSuspended = true;

			this.Invalidate();

			base.OnMouseLeave(e);
		}

        protected override void OnMouseEnter(EventArgs e) {
            _drawingSuspended = false;

            this.Invalidate();

            base.OnMouseEnter(e);
        }

		protected override void OnMouseMove(MouseEventArgs e) {
			if (_drawingRegion && e.Button == MouseButtons.Left) {
                //Continue drawing
				_regionLastPoint = e.Location;

				this.Invalidate();
			}
            else if(DrawMouseRegions && !_drawingRegion){
                //Keep track of region start point
                _regionLastPoint = e.Location;

                this.Invalidate();
            }

			base.OnMouseMove(e);
		}

		Pen penRed = new Pen(Color.FromArgb(255, Color.Red), 1.0f);

		protected override void OnPaint(PaintEventArgs e) {
			if (_drawingRegion) {
                //Is currently drawing, show rectangle
				int left = Math.Min(_regionStartPoint.X, _regionLastPoint.X);
				int right = Math.Max(_regionStartPoint.X, _regionLastPoint.X);
				int top = Math.Min(_regionStartPoint.Y, _regionLastPoint.Y);
				int bottom = Math.Max(_regionStartPoint.Y, _regionLastPoint.Y);

				e.Graphics.DrawRectangle(penRed, left, top, right - left, bottom - top);
			}
            else if (DrawMouseRegions && ! _drawingSuspended) {
                //Show cursor coordinates
                e.Graphics.DrawLine(penRed, new Point(0, _regionLastPoint.Y), new Point(ClientSize.Width, _regionLastPoint.Y));
                e.Graphics.DrawLine(penRed, new Point(_regionLastPoint.X, 0), new Point(_regionLastPoint.X, ClientSize.Height));
            }

			base.OnPaint(e);
		}

		#endregion

        #region Thumbnail clone click

        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);

            if (_thumbnail == null)
                return;

            //Raise clicking event to allow click forwarding
            if (ReportThumbnailClicks) {
                OnCloneClick(ClientToThumbnail(e.Location), e.Button, false);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            base.OnMouseDoubleClick(e);

            if (_thumbnail == null)
                return;

            //Raise double clicking event to allow click forwarding
            if (ReportThumbnailClicks) {
                OnCloneClick(ClientToThumbnail(e.Location), e.Button, true);
            }
        }

        /// <summary>
        /// Is raised when the thumbnail clone is clicked.
        /// </summary>
		public event EventHandler<CloneClickEventArgs> CloneClick;

		protected virtual void OnCloneClick(Point location, MouseButtons buttons, bool doubleClick){
            var evt = CloneClick;
			if(evt != null)
				evt(this, new CloneClickEventArgs(location, buttons, doubleClick));
        }

        #endregion

        /// <summary>
        /// Convert a point in client coordinates to a point expressed in terms of a cloned thumbnail window.
        /// </summary>
        /// <param name="position">Point in client coordinates.</param>
        protected Point ClientToThumbnail(Point position) {
            //Compensate padding
            position.X -= _padWidth;
            position.Y -= _padHeight;

            PointF proportionalPosition = new PointF(
                (float)position.X / _thumbnailSize.Width,
                (float)position.Y / _thumbnailSize.Height
            );

            //Get real pixel region info
            Size source = (_regionEnabled) ? _regionCurrent.Size : _thumbnail.SourceSize;
            Point offset = (_regionEnabled) ? _regionCurrent.Location : Point.Empty;

            return new Point(
                (int)((proportionalPosition.X * source.Width) + offset.X),
                (int)((proportionalPosition.Y * source.Height) + offset.Y)
            );
        }

    }

}
