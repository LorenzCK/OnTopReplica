using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using VistaControls.Dwm;
using VistaControls.ThemeText;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace OnTopReplica {

	class ThumbnailPanel : Panel {

		//DWM Thumbnail stuff
		Thumbnail _thumbnail = null;
		bool _regionEnabled = false;
		Rectangle _regionCurrent;

		//Labels
		ClickThroughLabel _labelNoGlass;
		ThemedLabel _labelGlass;

		public ThumbnailPanel()
			: this(false) {
		}

		/// <summary>Constructs a new ThumbnailPanel with a given glass mode value.</summary>
		/// <param name="enableGlass">True if glass should be enabled.</param>
		public ThumbnailPanel(bool enableGlass) {
			InitFormComponents();

			GlassMode = enableGlass;

			UpdateRightClickLabels();
		}

		private void InitFormComponents() {
			//Themed Label
			_labelGlass = new ThemedLabel();
			_labelGlass.Dock = DockStyle.Fill;
			_labelGlass.ForeColor = SystemColors.ControlText;
			_labelGlass.Location = Point.Empty;
			_labelGlass.Size = ClientSize;
			_labelGlass.Name = "labelGlass";
			_labelGlass.Text = Strings.RightClick;
			_labelGlass.TextAlign = HorizontalAlignment.Center;
			_labelGlass.TextAlignVertical = VerticalAlignment.Center;
			this.Controls.Add(_labelGlass);

			//Standard label
			_labelNoGlass = new ClickThroughLabel();
			_labelNoGlass.Dock = DockStyle.Fill;
			_labelNoGlass.BackColor = Color.Transparent;
			_labelNoGlass.Location = Point.Empty;
			_labelNoGlass.Size = ClientSize;
			_labelNoGlass.Name = "labelNoGlass";
			_labelNoGlass.Text = Strings.RightClick;
			_labelNoGlass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.Controls.Add(_labelNoGlass);
		}

		#region Settings

		bool _glassMode = true;

		public bool GlassMode {
			get {
				return _glassMode;
			}
			set {
				_glassMode = value;

				//Set correct backcolor: black if glass is on
                BackColor = (value || _fullscreenMode) ? Color.Black : SystemColors.Control;

				UpdateRightClickLabels();
			}
		}

		bool _fullscreenMode = false;

		public bool FullscreenMode {
			get {
				return _fullscreenMode;
			}
			set {
				_fullscreenMode = value;

				//Set correct backcolor: black if fullscreen is on
				BackColor = (value || _glassMode) ? Color.Black : SystemColors.Control;

				UpdateRightClickLabels();
			}
		}

		public Rectangle ShownRegion {
			get {
				return _regionCurrent;
			}
			set {
				_regionEnabled = true;
				_regionCurrent = value;

				UpdateThubmnail();
			}
		}

		public bool ShowRegion {
			get {
				return _regionEnabled;
			}
			set {
				_regionEnabled = value;

				UpdateThubmnail();
			}
		}

		bool _drawMouseRegions = false;

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

				UpdateThubmnail();
			}
		}

		private byte ThumbnailOpacity {
			get {
                return (_drawMouseRegions) ? (byte)130 : (byte)255;
			}
		}

		bool _clickThrough = true;

		public bool ClickThrough {
			get {
				return _clickThrough;
			}
			set {
				_clickThrough = value;
			}
		}

		#endregion

		public void ResetShownRegion() {
			_regionEnabled = false;

			UpdateThubmnail();
		}

		public void SetThumbnailHandle(WindowHandle handle) {
			if (_thumbnail != null && !_thumbnail.IsInvalid)
				_thumbnail.Close();

			//Get form and register thumbnail on it
			Form owner = this.TopLevelControl as Form;
			if(owner == null)
				throw new Exception();

			//Reset region
			_regionEnabled = false;

			_thumbnail = DwmManager.Register(owner, handle.Handle);

			//Do empty thumbnail update to init DWM info (source size)
			_thumbnail.Update(ClientRectangle, (byte)255, true, true);

			//Correct update
			UpdateThubmnail();
		}

		public void UnsetThumbnail() {
			if (_thumbnail != null && !_thumbnail.IsInvalid)
				_thumbnail.Close();

			_thumbnail = null;

			UpdateRightClickLabels();
		}

		public bool IsShowingThumbnail {
			get {
				return (_thumbnail != null && !_thumbnail.IsInvalid);
			}
		}

		int padWidth = 0;
		int padHeight = 0;
		Size thumbnailSize;

		/// <summary>Updates the thumbnail options and the right-click labels.</summary>
		private void UpdateThubmnail() {
			if (_thumbnail != null && !_thumbnail.IsInvalid){
                try {
                    Size sourceSize = (_regionEnabled) ? _regionCurrent.Size : _thumbnail.SourceSize;
                    thumbnailSize = new Size(Size.Width, Size.Height * 2); //ComputeIdealSize(sourceSize, Size);

                    /*padWidth = (Size.Width - thumbnailSize.Width) / 2;
                    padHeight = (Size.Height - thumbnailSize.Height) / 2;

                    Rectangle target = new Rectangle(padWidth, padHeight, thumbnailSize.Width, thumbnailSize.Height);*/
                    var target = new Rectangle(0, 0, thumbnailSize.Width, thumbnailSize.Height);
                    Rectangle source = (_regionEnabled) ? _regionCurrent : new Rectangle(Point.Empty, _thumbnail.SourceSize);

                    //Console.WriteLine("Source " + sourceSize.ToString() + ", Target " + Size.ToString() + ", Fit " + thumbnailSize.ToString() + ", Padding " + padWidth + "," + padHeight);

                    _thumbnail.Update(target, source, ThumbnailOpacity, true, true);
                }
                catch {
                    //Any error updating the thumbnail forces to unset (handle may be not valid)
                    UnsetThumbnail();
                    return;
                }
			}

			UpdateRightClickLabels();
		}

		/// <summary>Computes ideal thumbnail size given an original size and a target to fit.</summary>
		/// <param name="sourceSize">Size of the original thumbnail.</param>
		/// <param name="clientSize">Size of the client area to fit.</param>
		private Size ComputeIdealSize(Size sourceSize, Size clientSize) {
			double sourceRatio = (double)sourceSize.Width / (double)sourceSize.Height;
			double clientRatio = (double)clientSize.Width / (double)clientSize.Height;

			Size ret;
			if(sourceRatio >= clientRatio)
				ret = new Size(clientSize.Width, (int)((double)clientSize.Width / sourceRatio));
			else
				ret = new Size((int)((double)clientSize.Height * sourceRatio), clientSize.Height);

			return ret;
		}

		/// <summary>Updates the right-click labels.</summary>
		/// <remarks>If a thumbnail is shown no label will be visible. If no thumbnail is active, the correct label will be visible.</remarks>
		private void UpdateRightClickLabels(){
			if (_thumbnail != null && !_thumbnail.IsInvalid /*&& !_drawMouseRegions*/) {
				//Thumbnail active and no region drawing
				_labelGlass.Visible = false;
				_labelNoGlass.Visible = false;
			}
			else {
				//Update text (removed, can't draw regions behind non-transparent ThemedLabel control)
				//_labelGlass.Text = _labelNoGlass.Text = (_drawMouseRegions) ? Strings.DrawRegions : Strings.RightClick;

				//Update visibility
				_labelGlass.Visible = _glassMode;
				_labelNoGlass.Visible = !_glassMode;
			}
		}

		#region Event handling

		protected override void OnResize(EventArgs eventargs) {
			UpdateThubmnail();

			base.OnResize(eventargs);
		}

		protected override void WndProc(ref Message m) {
			//Make transparent to hit-testing
			if (m.Msg == NativeMethods.WM_NCHITTEST && !DrawMouseRegions && ClickThrough) {
				m.Result = new IntPtr(NativeMethods.HTTRANSPARENT);
				return;
			}

			base.WndProc(ref m);
		}

		protected override void OnMouseClick(MouseEventArgs e) {
			if (!_clickThrough && e.Button == MouseButtons.Left) {
				if(_thumbnail != null)
					OnCloneClick(ScreenToThumbnail(e.Location), false);
			}

			base.OnMouseClick(e);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e) {
			if (!_clickThrough && e.Button == MouseButtons.Left) {
				if (_thumbnail != null)
					OnCloneClick(ScreenToThumbnail(e.Location), true);
			}

			base.OnMouseDoubleClick(e);
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			if (_drawMouseRegions && e.Button == MouseButtons.Left) {
				_drawingRegion = true;
				_regionStartPoint = _regionLastPoint = e.Location;

				this.Invalidate();
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			if (_drawMouseRegions && e.Button == MouseButtons.Left) {
				_drawingRegion = false;

				HandleRegionDrawn(_regionStartPoint, _regionLastPoint);

				this.Invalidate();
			}

			base.OnMouseUp(e);
		}

		protected override void OnMouseLeave(EventArgs e) {
			_drawingRegion = false;

			this.Invalidate();

			base.OnMouseLeave(e);
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			if (_drawingRegion && e.Button == MouseButtons.Left) {
				_regionLastPoint = e.Location;

				this.Invalidate();
			}
            else if(_drawMouseRegions && !_drawingRegion){
                _regionLastPoint = e.Location;

                this.Invalidate();
            }

			base.OnMouseMove(e);
		}

		Pen penRed = new Pen(Color.FromArgb(255, Color.Red), 1.0f);

		protected override void OnPaint(PaintEventArgs e) {
			if (_drawingRegion) {
				int left = Math.Min(_regionStartPoint.X, _regionLastPoint.X);
				int right = Math.Max(_regionStartPoint.X, _regionLastPoint.X);
				int top = Math.Min(_regionStartPoint.Y, _regionLastPoint.Y);
				int bottom = Math.Max(_regionStartPoint.Y, _regionLastPoint.Y);

				e.Graphics.DrawRectangle(penRed, left, top, right - left, bottom - top);
			}
            else if (_drawMouseRegions) {
                e.Graphics.DrawLine(penRed, new Point(0, _regionLastPoint.Y), new Point(ClientSize.Width, _regionLastPoint.Y));
                e.Graphics.DrawLine(penRed, new Point(_regionLastPoint.X, 0), new Point(_regionLastPoint.X, ClientSize.Height));
            }

			base.OnPaint(e);
		}

		#endregion

		bool _drawingRegion = false;
		Point _regionStartPoint;
		Point _regionLastPoint;

		public delegate void RegionDrawnHandler(object sender, Rectangle region);

		public event RegionDrawnHandler RegionDrawn;

		protected virtual void OnRegionDrawn(Rectangle region) {
			if (RegionDrawn != null)
				RegionDrawn(this, region);
		}

		protected Point ScreenToThumbnail(Point position) {
			//Compensate padding
			position.X -= padWidth;
			position.Y -= padHeight;

			PointF proportionalPosition = new PointF(
				(float)position.X / thumbnailSize.Width,
				(float)position.Y / thumbnailSize.Height
			);

			//Get real pixel region info
			Size source = (_regionEnabled) ? _regionCurrent.Size : _thumbnail.SourceSize;
			Point offset = (_regionEnabled) ? _regionCurrent.Location : Point.Empty;

			return new Point(
				(int)((proportionalPosition.X * source.Width) + offset.X),
				(int)((proportionalPosition.Y * source.Height) + offset.Y)
			);
		}

		protected void HandleRegionDrawn(Point start, Point end) {
			int left = Math.Min(start.X, end.X);
			int right = Math.Max(start.X, end.X);
			int top = Math.Min(start.Y, end.Y);
			int bottom = Math.Max(start.Y, end.Y);

			//Offset points of padding space around thumbnail
			left -= padWidth;
			right -= padWidth;
			top -= padHeight;
			bottom -= padHeight;

			//Get proportional region on thumbnail size
			RectangleF region = new RectangleF(
				(float)left / thumbnailSize.Width,
				(float)top / thumbnailSize.Height,
				(float)(right - left) / thumbnailSize.Width,
				(float)(bottom - top) / thumbnailSize.Height
			);

			//Compute real pixel-region
			Size source = (_regionEnabled) ? _regionCurrent.Size : _thumbnail.SourceSize;
			Point offset = (_regionEnabled) ? _regionCurrent.Location : Point.Empty;

			Rectangle regionPixel = new Rectangle(
				(int)(region.Left * source.Width) + offset.X,
				(int)(region.Top * source.Height) + offset.Y,
				(int)(region.Width * source.Width),
				(int)(region.Height * source.Height)
			);

			//Update region
			ShownRegion = regionPixel;

			//Report to hooked event handlers that the current region has changed
			OnRegionDrawn(regionPixel);
		}

		public Size ThumbnailOriginalSize {
			get {
				if (_thumbnail != null && !_thumbnail.IsInvalid)
					return (_regionEnabled) ? _regionCurrent.Size : _thumbnail.SourceSize;
				else
					throw new Exception(Strings.ErrorNoThumbnail);
			}
		}

		public event EventHandler<CloneClickEventArgs> CloneClick;

		protected virtual void OnCloneClick(Point location, bool doubleClick){
			if(CloneClick != null)
				CloneClick(this, new CloneClickEventArgs(location, doubleClick));
		}
	}

}
