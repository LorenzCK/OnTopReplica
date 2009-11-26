using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OnTopReplica.Properties;
using VistaControls.Dwm;
using VistaControls.TaskDialog;

namespace OnTopReplica
{
    public partial class MainForm : AspectRatioForm
    {
        //Visualization status
        byte _lastOpacity = 255;
		bool _clickForwarding = false;

		//GUI
		ThumbnailPanel _thumbnailPanel = null;
		RegionBox _regionBox = null;
		FullscreenForm _fullscreenForm;

		//Icon
		NotifyIcon taskIcon = null;

        //Window manager
        WindowManager _windowManager;
		WindowHandle _lastWindowHandle = null;

        //Override position and size on startup
        bool _startOverride = false;
        Point _startLocation;
        Size _startSize;

        public MainForm(Point location, Size size)
            : this() {
            _startOverride = true;
            _startLocation = location;
            _startSize = size;
        }

        public MainForm() {
            InitializeComponent();

			//Thumbnail panel
			_thumbnailPanel = new ThumbnailPanel(Settings.Default.UseGlass);
			_thumbnailPanel.RegionDrawn += new ThumbnailPanel.RegionDrawnHandler(Thumbnail_RegionDrawn);
			_thumbnailPanel.CloneClick += new EventHandler<CloneClickEventArgs>(Thumbnail_CloneClick);
			_thumbnailPanel.ClickThrough = !_clickForwarding;
			_thumbnailPanel.Location = Point.Empty;
			_thumbnailPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			_thumbnailPanel.Size = ClientSize;
			Controls.Add(_thumbnailPanel);

			//Region box
			_regionBox = new RegionBox();
			_regionBox.RequestClosing += new EventHandler(RegionBox_RequestClosing);
			_regionBox.RequestRegionReset += new EventHandler(RegionBox_RequestRegionReset);
			_regionBox.RegionSet += new RegionBox.RegionSetHandler(RegionBox_RegionChanged);
			_regionBox.Location = new Point(ClientSize.Width, 0);
			_regionBox.Size = new Size(_regionBox.Width, ClientSize.Height);
			_regionBox.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
			_regionBox.Enabled = _regionBox.Visible = false;
			Controls.Add(_regionBox);

			//Full screen form
			_fullscreenForm = new FullscreenForm();
			_fullscreenForm.CloseRequest += new EventHandler<CloseRequestEventArgs>(FullscreenForm_CloseRequest);

			//Set native renderer on context menues
			Asztal.Szótár.NativeToolStripRenderer.SetToolStripRenderer(new Control[] {
				menuContext, menuWindows, menuOpacity, menuIconContext, menuResize, menuLanguages
			});

            //Hook keyboard handler
            this.KeyUp += new KeyEventHandler(Common_Key);
            this.KeyPreview = true;
        }

        #region Child forms & controls events

        void FullscreenForm_CloseRequest(object sender, CloseRequestEventArgs e) {
			if (_isFullscreen) {
                var regionContainer = (e.LastRegion != null) ?
                    new StoredRegion { Rect = e.LastRegion.Value } : null;
                
                //Update handle to match the one selected in fullscreen mode
                ThumbnailSet(e.LastWindowHandle, regionContainer);

				ToggleFullscreen();
			}
		}

		void RegionBox_RegionChanged(object sender, Rectangle region) {
			_thumbnailPanel.ShownRegion = region;
		}

		void RegionBox_RequestRegionReset(object sender, EventArgs e) {
			_thumbnailPanel.ResetShownRegion();
		}

		void Thumbnail_RegionDrawn(object sender, Rectangle region) {
			_regionBox.SetRegion(region);
		}

		void Thumbnail_CloneClick(object sender, CloneClickEventArgs e) {
			if (_clickForwarding) {
				Win32Helper.InjectFakeMouseClick(_lastWindowHandle.Handle, e.ClientClickLocation, e.IsDoubleClick);
			}
		}

		void RegionBox_RequestClosing(object sender, EventArgs e) {
			RegionBoxShowing = false;
		}

		void Thumbnail_IdealSizeChange(object sender, Size e) {
			ClientSize = e;
        }

        #endregion

        #region Side "Region box" events

        const int cWindowBoundary = 10;

		bool _regionBoxShowing = false;
		bool _regionBoxDidMoveForm = false;
		Point _regionBoxPrevFormLocation;

		public bool RegionBoxShowing {
			get {
				return _regionBoxShowing;
			}
			set {
				if (_regionBoxShowing != value) {
					_regionBoxShowing = value;

					//Show box
					_regionBox.Visible = value;
					_regionBox.Enabled = value;

					//Disable dragging
					HandleMouseMove = !value;

					//Enable region drawing on thumbnail
					_thumbnailPanel.DrawMouseRegions = value;

					//Resize and move to fit region panel
					ClientSize = new Size {
						Width = ClientSize.Width + ((value) ? _regionBox.Width : -_regionBox.Width),
						Height = Math.Max(ClientSize.Height, _regionBox.ClientSize.Height)
					};
					_thumbnailPanel.Size = new Size {
						Width = (value) ? (ClientSize.Width - _regionBox.Width) : ClientSize.Width,
						Height = ClientSize.Height
					};
					_regionBox.Location = new Point {
						X = (value) ? (ClientSize.Width - _regionBox.Width) : ClientSize.Width,
						Y = 0
					};

                    //Disable aspect ratio keeping
                    KeepAspectRatio = !value;

					//Set new glass margins
					this.GlassMargins = (value) ?
						new Margins(ClientSize.Width - _regionBox.Width, 0, 0, 0) :
						new Margins(-1);

					//Check form boundaries and move form if necessary
					if (value) {
						var screenCurr = Screen.FromControl(this);
						int pRight = Location.X + Size.Width + cWindowBoundary;
						if (pRight >= screenCurr.Bounds.Width) {
							_regionBoxPrevFormLocation = Location;
							_regionBoxDidMoveForm = true;

							Location = new Point(screenCurr.WorkingArea.Width - Size.Width - cWindowBoundary, Location.Y);
						}
						else
							_regionBoxDidMoveForm = false;
					}
					else {
						if (_regionBoxDidMoveForm) {
							Location = _regionBoxPrevFormLocation;
							_regionBoxDidMoveForm = false;
						}

						//Resize automatically on region box closing
						/*if (Settings.Default.AutoFitOnResize)
							FitToThumbnail();*/
					}
				}
			}
		}

		#endregion

		#region Event override

		protected override void OnClosing(CancelEventArgs e) {
			//Destroy NotifyIcon
			if (taskIcon != null) {
				taskIcon.Visible = false;
				taskIcon.Dispose();
                taskIcon = null;
			}

			//Store settings
			if (Settings.Default.StoreWindowPosition) {
				Settings.Default.WindowPositionStored = true;
				Settings.Default.LastLocation = Location;
				Settings.Default.LastSize = ClientSize;
			}
			else
				Settings.Default.WindowPositionStored = false;

            base.OnClosing(e);
        }

        protected override void OnResizeBegin(EventArgs e) {
            base.OnResizeBegin(e);

            //Update aspect ratio if needed
            if (_thumbnailPanel.IsShowingThumbnail) {
                SetAspectRatio(_thumbnailPanel.ThumbnailOriginalSize);
            }
        }

		protected override void OnShown(EventArgs e) {
            base.OnShown(e);

			//Do some checks in order to verify the presence of desktop composition
			if (!VistaControls.OsSupport.IsVistaOrBetter) {
				MessageBox.Show(Strings.ErrorNoDwm, Strings.ErrorNoDwmTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

				this.Close();

				return;
			}

			if (!VistaControls.OsSupport.IsCompositionEnabled) {
				VistaControls.TaskDialog.TaskDialog dlg = new VistaControls.TaskDialog.TaskDialog(Strings.ErrorDwmOff, Strings.ErrorGenericTitle, Strings.ErrorDwmOffContent);
				dlg.ExpandedControlText = Strings.ErrorDetailsAero;
				dlg.ExpandedInformation = Strings.ErrorDetailsAeroInfo;
				dlg.CommonButtons = TaskDialogButton.Close;
				dlg.CommonIcon = VistaControls.TaskDialog.TaskDialogIcon.Stop;
				dlg.Show();

				this.Close();

				return;
			}

			//Get a window manager
			_windowManager = new WindowManager();

			//Install NotifyIcon
			taskIcon = new NotifyIcon();
			taskIcon.Text = Strings.ApplicationName;
			taskIcon.Icon = Properties.Resources.main_icon;
			taskIcon.Visible = true;
			taskIcon.ContextMenuStrip = menuIconContext;
			taskIcon.DoubleClick += new EventHandler(Icon_doubleclick);

			//Reload position settings
            if (_startOverride) {
                Location = _startLocation;
                Size = _startSize;
            }
			else if (Settings.Default.WindowPositionStored) {
				Location = Settings.Default.LastLocation;
				ClientSize = Settings.Default.LastSize;
			}

			//Glassify window
			this.GlassMargins = new VistaControls.Dwm.Margins(-1);
			SetGlass(Settings.Default.UseGlass);
		}

		#endregion

		#region Task Icon events

		void Icon_doubleclick(object sender, EventArgs e) {
			if (_isFullscreen)
				ToggleFullscreen();

			//Ensure main form is shown
			this.Show();
			this.Activate();

			this.TopMost = true;
		}

		private void IconContextOpen_click(object sender, EventArgs e) {
			Icon_doubleclick(sender, e);
		}

		private void IconContextReset_click(object sender, EventArgs e) {
			var dlg = new TaskDialog(Strings.AskReset, Strings.AskResetTitle, Strings.AskResetContent);
			dlg.UseCommandLinks = true;
			dlg.CustomButtons = new CustomButton[] {
				new CustomButton(Result.OK, Strings.AskResetButtonOk),
				new CustomButton(Result.Cancel, Strings.ButtonCancel)
			};
			dlg.CommonIcon = TaskDialogIcon.Information;

			if (dlg.Show().CommonButton == Result.OK) {
				//Reset display status
				Icon_doubleclick(sender, e);

				//Reset form settings
				ThumbnailUnset();
				RegionBoxShowing = false;

				Point nuLoc = Screen.PrimaryScreen.WorkingArea.Location;
				nuLoc.Offset(40, 40);

				Location = nuLoc;
				Size = MinimumSize;

				Show();
			}
		}

		private void IconContextExit_click(object sender, EventArgs e) {
			this.Close();
		}

		#endregion

        #region Menu Event Handling

		private void Menu_opening(object sender, CancelEventArgs e) {
			//Cancel if currently "fullscreen" mode
			if (_isFullscreen) {
				e.Cancel = true;
				return;
			}

			//Close region box if opened
			if (RegionBoxShowing) {
				RegionBoxShowing = false;
				e.Cancel = true;
				return;
			}

			selectRegionToolStripMenuItem.Enabled = _thumbnailPanel.IsShowingThumbnail;
			switchToWindowToolStripMenuItem.Enabled = _thumbnailPanel.IsShowingThumbnail;
			resizeToolStripMenuItem.Enabled = _thumbnailPanel.IsShowingThumbnail;
			chromeToolStripMenuItem.Checked = (FormBorderStyle == FormBorderStyle.SizableToolWindow);

			forwardClicksToolStripMenuItem.Checked = _clickForwarding;
		}

        private void Menu_Close_click(object sender, EventArgs e) {
            this.Close();
        }

        private void Menu_About_click(object sender, EventArgs e) {
			this.Hide();

			var box = new AboutForm();
			box.Location = RecenterLocation(this, box);
            box.ShowDialog();

			Location = RecenterLocation(box, this);
			this.Show();

			box.Dispose();
        }

		private void Menu_Language_click(object sender, EventArgs e) {
			ToolStripItem tsi = (ToolStripItem)sender;

			string langCode = tsi.Tag as string;

			if (Program.ForceGlobalLanguageChange(langCode))
				this.Close();
			else
				MessageBox.Show("Error");
		}

		private Point RecenterLocation(Control original, Control final) {
			int origX = original.Location.X + original.Size.Width / 2;
			int origY = original.Location.Y + original.Size.Height / 2;

			int finX = origX - final.Size.Width / 2;
			int finY = origY - final.Size.Height / 2;

			//Check boundaries
			var screen = Screen.FromControl(final);
			if (finX < screen.WorkingArea.X)
				finX = screen.WorkingArea.X;
			if (finX + final.Size.Width > screen.WorkingArea.Width)
				finX = screen.WorkingArea.Width - final.Size.Width;
			if (finY < screen.WorkingArea.Y)
				finY = screen.WorkingArea.Y;
			if (finY + final.Size.Height > screen.WorkingArea.Height)
				finY = screen.WorkingArea.Height - final.Size.Height;

			return new Point(finX, finY);
		}

        void Menu_Windows_itemclick(object sender, EventArgs e) {
            //Insure the menu is closed
            menuContext.Close();

            //Get clicked item and window index from tag
            ToolStripItem tsi = (ToolStripItem)sender;

			//Handle special "none" window
			if (tsi.Tag == null) {
				ThumbnailUnset();
				return;
			}

            var selectionData = (WindowListHelper.WindowSelectionData)tsi.Tag;

            if (_windowManager != null) {
                ThumbnailSet(selectionData.Handle, selectionData.Region);
            }
        }

		private void Menu_Switch_click(object sender, EventArgs e) {
			if (_lastWindowHandle == null)
				return;

			NativeMethods.SetForegroundWindow(_lastWindowHandle.Handle);

			this.Hide();
		}

		private void Menu_Forward_click(object sender, EventArgs e) {
			if (Settings.Default.FirstTimeClickForwarding && !_clickForwarding) {
				TaskDialog dlg = new TaskDialog(Strings.InfoClickForwarding,
					Strings.InfoClickForwardingTitle, Strings.InfoClickForwardingContent);
				dlg.CommonButtons = TaskDialogButton.Yes | TaskDialogButton.No;

				Results result = dlg.Show(this);

				if (result.CommonButton == Result.No)
					return;

				Settings.Default.FirstTimeClickForwarding = false;
			}

			_clickForwarding = !_clickForwarding;
			_thumbnailPanel.ClickThrough = !_clickForwarding;
		}

        private void Menu_Opacity_opening(object sender, CancelEventArgs e) {
            ToolStripMenuItem[] items = {
				toolStripMenuItem1,
				toolStripMenuItem2,
				toolStripMenuItem3,
				toolStripMenuItem4
			};

			foreach (ToolStripMenuItem i in items) {
				if ((int)i.Tag == _lastOpacity)
					i.Checked = true;
				else
					i.Checked = false;
			}

			//Glass state
			toolStripMenuItem5.Checked = Settings.Default.UseGlass;
        }

        private void Menu_Opacity_click(object sender, EventArgs e) {
            //Get clicked menu item
            ToolStripMenuItem tsi = sender as ToolStripMenuItem;

            if (tsi != null) {
                //Get opacity from the tag
                int op = (int)tsi.Tag;

                //Store new opacity
                _lastOpacity = (byte)op;

                //Set the window's opacity
                this.Opacity = (double)op / 255.0;
            }
        }

		private void Menu_Opacity_Glass_click(object sender, EventArgs e) {
			SetGlass(!Settings.Default.UseGlass);
		}

		private void Menu_Region_click(object sender, EventArgs e) {
			RegionBoxShowing = true;
		}
		
		private void Menu_Resize_opening(object sender, CancelEventArgs e) {
			if (!_thumbnailPanel.IsShowingThumbnail)
				e.Cancel = true;

			//autofitOnResizeToolStripMenuItem.Checked = Settings.Default.AutoFitOnResize;
			recallLastPositionAndSizeToolStripMenuItem.Checked = Settings.Default.StoreWindowPosition;
			clickThroughToolStripMenuItem.Checked = Settings.Default.ClickThrough;
		}

		private void Menu_Resize_Double(object sender, EventArgs e) {
			FitToThumbnail(2.0);
		}

		private void Menu_Resize_FitToWindow(object sender, EventArgs e) {
			FitToThumbnail(1.0);
		}

		private void Menu_Resize_Half(object sender, EventArgs e) {
			FitToThumbnail(0.5);
		}

		private void Menu_Resize_Quarter(object sender, EventArgs e) {
			FitToThumbnail(0.25);
		}

		private void Menu_Resize_Fullscreen(object sender, EventArgs e) {
			ToggleFullscreen();
		}

		private void Menu_Resize_ClickThrough(object sender, EventArgs e) {
			Settings.Default.ClickThrough = !Settings.Default.ClickThrough;
		}

		/*private void Menu_Resize_Autofit_click(object sender, EventArgs e) {
			Settings.Default.AutoFitOnResize = !Settings.Default.AutoFitOnResize;
		}*/

		private void Menu_Position_Recall_click(object sender, EventArgs e) {
			Settings.Default.StoreWindowPosition = !Settings.Default.StoreWindowPosition;
		}

		private void Menu_Position_TopLeft(object sender, EventArgs e) {
			var screen = Screen.FromControl(this);

			Location = new Point(
                screen.WorkingArea.Left - ChromeBorderHorizontal,
                screen.WorkingArea.Top - ChromeBorderVertical
			);
		}

		private void Menu_Position_TopRight(object sender, EventArgs e) {
			var screen = Screen.FromControl(this);

			Location = new Point(
                screen.WorkingArea.Width - Size.Width + ChromeBorderHorizontal,
                screen.WorkingArea.Top - ChromeBorderVertical
			);
		}

		private void Menu_Position_BottomLeft(object sender, EventArgs e) {
			var screen = Screen.FromControl(this);

			Location = new Point(
                screen.WorkingArea.Left - ChromeBorderHorizontal,
                screen.WorkingArea.Height - Size.Height + ChromeBorderVertical
			);
		}

		private void Menu_Position_BottomRight(object sender, EventArgs e) {
			var screen = Screen.FromControl(this);

			Location = new Point(
				screen.WorkingArea.Width - Size.Width + ChromeBorderHorizontal,
                screen.WorkingArea.Height - Size.Height + ChromeBorderVertical
			);
		}

        private int ChromeBorderVertical {
            get {
                if (FormBorderStyle == FormBorderStyle.SizableToolWindow)
                    return SystemInformation.FrameBorderSize.Height;
                else
                    return 0;
            }
        }

        private int ChromeBorderHorizontal {
            get {
                if (FormBorderStyle == FormBorderStyle.SizableToolWindow)
                    return SystemInformation.FrameBorderSize.Width;
                else
                    return 0;
            }
        }

        private void Menu_Reduce_click(object sender, EventArgs e) {
            //Hide form
            this.Hide();
        }

		private void Menu_Windows_opening(object sender, EventArgs e) {
			//Refresh window list
			_windowManager.Refresh(WindowManager.EnumerationMode.TaskWindows);

			WindowListHelper.PopulateMenu(this, _windowManager, menuWindows, _lastWindowHandle, new EventHandler(Menu_Windows_itemclick));
		}

		private void Menu_Chrome_click(object sender, EventArgs e) {
            if (FormBorderStyle == FormBorderStyle.SizableToolWindow) {
                FormBorderStyle = FormBorderStyle.None;
                Location = new Point {
                    X = Location.X + SystemInformation.FrameBorderSize.Width,
                    Y = Location.Y + SystemInformation.FrameBorderSize.Height
                };
            }
            else {
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
                Location = new Point {
                    X = Location.X - SystemInformation.FrameBorderSize.Width,
                    Y = Location.Y - SystemInformation.FrameBorderSize.Height
                };
            }

			Invalidate();
		}

        #endregion

		#region Event handling

		private void Form_doubleclick(object sender, EventArgs e) {
			if(_thumbnailPanel.IsShowingThumbnail)
				ToggleFullscreen();
		}

        void Common_Key(object sender, KeyEventArgs e) {
            if (e.Modifiers == Keys.Alt) {
                if (e.KeyCode == Keys.Enter) {
                    e.Handled = true;
                    ToggleFullscreen();
                }

                else if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) {
                    FitToThumbnail(0.25);
                }

                else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) {
                    FitToThumbnail(0.5);
                }

                else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0) {
                    FitToThumbnail(1.0);
                }

                else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4) {
                    FitToThumbnail(2.0);
                }
            }
        }

		#endregion

		#region Fullscreen

		bool _isFullscreen = false;
		
		private void ToggleFullscreen() {
			if (_isFullscreen) {
				_fullscreenForm.Visible = false;

				this.Visible = true;
			}
			else {
				if (_lastWindowHandle == null) {
					//Should not happen... if it does, do nothing
					return;
				}

				CheckFirstTimeClickThrough();

				_fullscreenForm.DisplayFullscreen(Screen.FromControl(this), _lastWindowHandle);
				_fullscreenForm.ShownRegion = _thumbnailPanel.ShownRegion;
				_fullscreenForm.ShowRegion = _thumbnailPanel.ShowRegion;
				_fullscreenForm.Opacity = this.Opacity;

				//Enable click through if it is enabled and opacity is less than 255 (opaque)
				_fullscreenForm.ClickThrough = (Settings.Default.ClickThrough && _lastOpacity < 255);

				_fullscreenForm.Visible = true;

				this.Visible = false;
			}

			_isFullscreen = !_isFullscreen;
		}

		/// <summary>Check if the user uses click-through for the first time and asks confirmation.</summary>
		private void CheckFirstTimeClickThrough() {
			if (Settings.Default.FirstTimeClickThrough && _lastOpacity < 255) {
				//Alert the user about click through
				TaskDialog dlg = new TaskDialog(Strings.InfoClickThrough, Strings.InfoClickThroughTitle, Strings.InfoClickThroughInformation);
				dlg.CommonIcon = TaskDialogIcon.Information;
				dlg.ExpandedControlText = Strings.ErrorDetailButton;
				dlg.ExpandedInformation = Strings.InfoClickThroughDetails;
				dlg.UseCommandLinks = true;
				dlg.CustomButtons = new CustomButton[] {
					new CustomButton(Result.Yes, Strings.InfoClickThroughOk),
					new CustomButton(Result.No, Strings.InfoClickThroughNo)
				};

				Settings.Default.ClickThrough = (dlg.Show(this).CommonButton == Result.Yes);
			}

			Settings.Default.FirstTimeClickThrough = false;
		}

		#endregion

		#region Thumbnail operation

        private void ThumbnailSet(WindowHandle handle, StoredRegion region) {
            try {
				_lastWindowHandle = handle;

				_thumbnailPanel.SetThumbnailHandle(handle);
            }
            catch (Exception ex) {
                ThumbnailError(ex, false, Strings.ErrorUnableToCreateThumbnail);
            }

            //Update region to show
            if (region == null)
                _regionBox.Reset();
            else
                _regionBox.SetRegion(region);

            //Set aspect ratio (this will resize the form)
            SetAspectRatio(_thumbnailPanel.ThumbnailOriginalSize);

			//GUI
			selectRegionToolStripMenuItem.Enabled = true;
			resizeToolStripMenuItem.Enabled = true;
        }

        private void ThumbnailUnset(){
            //Unset handle
			_lastWindowHandle = null;
			_thumbnailPanel.UnsetThumbnail();

            //Reset regions
			_regionBox.Reset();

            //Disable aspect ratio
            KeepAspectRatio = false;
        }

        private void ThumbnailError(Exception ex, bool suppress, string title){
            if (!suppress) {
                ShowErrorDialog(title, Strings.ErrorGenericThumbnailHandleError, ex.Message);
            }

            ThumbnailUnset();
        }

        
		/// <summary>Automatically sizes the window in order to accomodate the thumbnail p times.</summary>
		/// <param name="p">Scale of the thumbnail to consider.</param>
		private void FitToThumbnail(double p) {
			try {
				Size originalSize = _thumbnailPanel.ThumbnailOriginalSize;

				Size fittedSize = new Size((int)(originalSize.Width * p), (int)(originalSize.Height * p));

				this.ClientSize = fittedSize;
			}
			catch (Exception ex) {
				ThumbnailError(ex, false, Strings.ErrorUnableToFit);
			}
		}

        /*
		/// <summary>Automatically fits the window to the current thumbnail.</summary>
		/// <remarks>Only adjusts height and corrects the window's position.</remarks>
		private void FitToThumbnail() {
			if (_thumbnailPanel.IsShowingThumbnail) {
				var source = _thumbnailPanel.ThumbnailOriginalSize;
				double ratio = (double)source.Width / (double)source.Height;

				int width = (RegionBoxShowing) ? (ClientSize.Width - _regionBox.Width) : ClientSize.Width;
					//(RegionBoxShowing) ? ClientSize.Width - cRegionWithPadding : ClientSize.Width;
				int newHeight = (int)(width / ratio);
				int diffHeight = ClientSize.Height - newHeight;
				ClientSize = new Size(ClientSize.Width, newHeight);
				Location = new Point(Location.X, Location.Y + diffHeight / 2);
			}
		}
         * */

        #endregion

		#region GUI stuff

		/// <summary>Do whatever needed to set the "glass" effect to the desired state.</summary>
		protected void SetGlass(bool b) {
			this.GlassEnabled = b;
			
			_thumbnailPanel.GlassMode = b;
			_regionBox.GlassMode = b;

			this.Invalidate();

			//Store
			Settings.Default.UseGlass = b;
		}

		private void ShowErrorDialog(string mainInstruction, string explanation, string errorMessage) {
			TaskDialog dlg = new TaskDialog(mainInstruction, Strings.ErrorGenericTitle, explanation);
			dlg.CommonIcon = TaskDialogIcon.Stop;
			dlg.IsExpanded = false;
			if (!string.IsNullOrEmpty(errorMessage)) {
				dlg.ExpandedInformation = Strings.ErrorGenericInfoText + errorMessage;
				dlg.ExpandedControlText = Strings.ErrorGenericInfoButton;
			}
			dlg.Show(this.Handle);
		}

		#endregion

	}
}
