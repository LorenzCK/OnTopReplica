using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica {

	public partial class FullscreenForm : Form {

		public FullscreenForm() {
			InitializeComponent();

			_thumbnail.GlassMode = true;

			//Set native renderer on context menu
			Asztal.Szótár.NativeToolStripRenderer.SetToolStripRenderer(new Control[] {
				menuContext, menuWindows
			});

            _cursorTimer = new Timer();
            _cursorTimer.Interval = 1000;
            _cursorTimer.Tick += new EventHandler(_cursorTimer_Tick);
		}

        Timer _cursorTimer;

		WindowHandle _lastHandle;
		WindowManager _manager = new WindowManager(WindowManager.EnumerationMode.TaskWindows);

		public void DisplayFullscreen(Screen screen, WindowHandle window) {
			_lastHandle = window;

			//Init thumbnail
			_thumbnail.SetThumbnailHandle(window);

			//Form setup
			this.Location = screen.WorkingArea.Location;
			this.Size = screen.WorkingArea.Size;
		}

		public void CloseFullscreen() {
			this.Visible = false;

			_thumbnail.UnsetThumbnail();
		}

		public Rectangle ShownRegion {
			get {
				return _thumbnail.ShownRegion;
			}
			set {
				_thumbnail.ShownRegion = value;
			}
		}

		public bool ShowRegion {
			get {
				return _thumbnail.ShowRegion;
			}
			set {
				_thumbnail.ShowRegion = value;
			}
		}

		public event EventHandler<CloseRequestEventArgs> CloseRequest;

		protected virtual void OnCloseRequest() {
			if (CloseRequest != null)
				CloseRequest(this, new CloseRequestEventArgs(_lastHandle));
		}

        protected override void OnActivated(EventArgs e) {
            _cursorTimer.Start();

            base.OnActivated(e);
        }

        protected override void OnDeactivate(EventArgs e) {
            Cursor.Show();
            _cursorTimer.Stop();

            base.OnDeactivate(e);
        }

        Point? _lastPos = null;

        protected override void OnMouseMove(MouseEventArgs e) {
            if (_lastPos.HasValue) {
                int distance = 0;
                distance += Math.Abs(_lastPos.Value.X - e.X);
                distance += Math.Abs(_lastPos.Value.Y - e.Y);

                if (distance > 8) {
                    Cursor.Show();
                    _cursorTimer.Start();
                }
            }

            _lastPos = e.Location;

            base.OnMouseMove(e);
        }

        void _cursorTimer_Tick(object sender, EventArgs e) {
            Cursor.Hide();
            _cursorTimer.Stop();
        }

		protected override void OnDoubleClick(EventArgs e) {
			OnCloseRequest();

			base.OnDoubleClick(e);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				e.Handled = true;
				OnCloseRequest();
			}
            else if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Alt) {
                e.Handled = true;
                OnCloseRequest();
            }

			base.OnKeyUp(e);
		}

		bool _clickThrough = false;

		public bool ClickThrough {
			get {
				return _clickThrough;
			}
			set {
				_clickThrough = value;

				this.TransparencyKey = (value) ? Color.Black : Color.White;
				this.Invalidate();
			}
		}

		const int WM_NCHITTEST = 0x0084;
		const int HTTRANSPARENT = -1;

		protected override void WndProc(ref Message m) {
			if (_clickThrough && m.Msg == WM_NCHITTEST) {
				m.Result = new IntPtr(HTTRANSPARENT);
				return;
			}

			base.WndProc(ref m);
		}

		private void Menu_Quit_click(object sender, EventArgs e) {
			OnCloseRequest();
		}

		private void Menu_Windows_opening(object sender, EventArgs e) {
			_manager.Refresh(WindowManager.EnumerationMode.TaskWindows);

			WindowListHelper.PopulateMenu(_manager, menuWindows, _lastHandle, new EventHandler(Menu_Window_click));
		}

		void Menu_Window_click(object sender, EventArgs e) {
			//Get clicked item and window index from tag
			ToolStripItem tsi = (ToolStripItem)sender;

			//Handle -none- selection
			if (tsi.Tag == null) {
				OnCloseRequest();
				return;
			}

			int index = (int)tsi.Tag;

			if (index >= _manager.Windows.Count)
				return;

			var handle = _manager.Windows[index];

			_thumbnail.SetThumbnailHandle(handle);
			_lastHandle = handle;
		}

	}

}
