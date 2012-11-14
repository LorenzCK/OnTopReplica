using System;
using System.Drawing;
using System.Windows.Forms;
using OnTopReplica.Properties;

namespace OnTopReplica.SidePanels {

	partial class RegionPanel : SidePanel {

		public RegionPanel() {
			InitializeComponent();

            Localize();

			//Copy settings into combo box
			if (Settings.Default.SavedRegions != null) {
				foreach (object o in Settings.Default.SavedRegions) {
					comboRegions.Items.Add(o);
				}
			}

            _regionDrawnHandler = new ThumbnailPanel.RegionDrawnHandler(ThumbnailPanel_RegionDrawn);
		}

        /// <summary>
        /// Localizes the dialog's labels.
        /// </summary>
        private void Localize() {
            this.SuspendLayout();

            groupRegions.Text = Strings.RegionsTitle;
            comboRegions.CueBannerText = Strings.RegionsStoredRegions;
            labelCurrentRegion.Text = Strings.RegionsCurrentRegion;
            buttonReset.Text = Strings.RegionsResetButton;
            buttonDone.Text = Strings.RegionsDoneButton;
            UpdateRegionLabels();

            toolTip.SetToolTip(buttonSave, Strings.RegionsSaveButton);
            toolTip.SetToolTip(buttonDelete, Strings.RegionsDeleteButton);

            this.ResumeLayout();
        }

        /// <summary>
        /// Updates the labels for the region value selectors and the relative mode checkbox.
        /// </summary>
        private void UpdateRegionControls(ThumbnailRegion region) {
            checkRelative.Checked = region.Relative;
            
            if (region.Relative) {
                Padding p = region.BoundsAsPadding;
                numX.Value = p.Left;
                numY.Value = p.Top;
                numW.Value = p.Right;
                numH.Value = p.Bottom;
            }
            else {
                Rectangle r = region.Bounds;
                numX.Value = r.X;
                numY.Value = r.Y;
                numW.Value = r.Width;
                numH.Value = r.Height;
            }

            UpdateRegionLabels();
        }

        /// <summary>
        /// Updates the labels of region selectors based on the dialog's state.
        /// </summary>
        private void UpdateRegionLabels() {
            if (checkRelative.Checked) {
                labelX.Text = Strings.RegionsLeft;
                labelY.Text = Strings.RegionsTop;
                labelWidth.Text = Strings.RegionsRight;
                labelHeight.Text = Strings.RegionsBottom;
            }
            else {
                labelX.Text = Strings.RegionsX;
                labelY.Text = Strings.RegionsY;
                labelWidth.Text = Strings.RegionsWidth;
                labelHeight.Text = Strings.RegionsHeight;
            }
        }

        public override string Title {
            get {
                return Strings.MenuRegion;
            }
        }

        ThumbnailPanel.RegionDrawnHandler _regionDrawnHandler;

        public override void OnFirstShown(MainForm form) {
            base.OnFirstShown(form);

            //Init shown region if current thumbnail is clipped to region
            if (form.SelectedThumbnailRegion != null) {
                SetRegion(form.SelectedThumbnailRegion);
            }

            //Enable region drawing
            form.ThumbnailPanel.DrawMouseRegions = true;
            form.ThumbnailPanel.RegionDrawn += _regionDrawnHandler;
        }

        public override void OnClosing(MainForm form) {
            base.OnClosing(form);
            
            //Reset region drawing
            form.ThumbnailPanel.DrawMouseRegions = false;
            form.ThumbnailPanel.RegionDrawn -= _regionDrawnHandler;
        }

        void ThumbnailPanel_RegionDrawn(object sender, ThumbnailRegion region) {
            SetRegion(region);
        }

        #region Interface

        /// <summary>
        /// Sets the current selected region to a specific instance of a stored region.
        /// </summary>
        /// <param name="region">A stored region instance or null to reset.</param>
        public void SetRegion(StoredRegion region) {
            if (region == null) {
                Reset();
                return;
            }

            SetRegion(region.Region);

            //Select right combobox
            if (comboRegions.Items.Contains(region)) {
                comboRegions.SelectedItem = region;
            }
        }

        /// <summary>
        /// Sets the current selected region to a specific region rectangle.
        /// </summary>
        /// <param name="region">The region boundaries.</param>
		public void SetRegion(ThumbnailRegion region) {
			try {
				_ignoreValueChanges = true;

                UpdateRegionControls(region);

				numX.Enabled = numY.Enabled = numW.Enabled = numH.Enabled = true;
			}
			finally {
				_ignoreValueChanges = false;
			}

			OnRegionSet(region);
		}

        /// <summary>
        /// Resets the selected region and disables the num spinners.
        /// </summary>
		public void Reset() {
			try {
				_ignoreValueChanges = true;

				numX.Value = numY.Value = numW.Value = numH.Value = 0;
				numX.Enabled = numY.Enabled = numW.Enabled = numH.Enabled = false;
                checkRelative.Checked = false;
                UpdateRegionLabels();

				buttonSave.Enabled = false;

				comboRegions.SelectedIndex = -1;
			}
			finally {
				_ignoreValueChanges = false;
			}
		}

        #endregion

        /// <summary>
        /// Constructs a ThumbnailRegion from the dialog's current state.
        /// </summary>
        protected ThumbnailRegion ConstructCurrentRegion() {
            Rectangle bounds = new Rectangle {
                X = (int)numX.Value,
                Y = (int)numY.Value,
                Width = (int)numW.Value,
                Height = (int)numH.Value
            };

            ThumbnailRegion newRegion = new ThumbnailRegion(bounds, checkRelative.Checked);

            return newRegion;
        }

        /// <summary>
        /// Adds a new stored region.
        /// </summary>
        /// <param name="rectangle">Region bounds.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <param name="isRelative">Whether the region is relative to the border.</param>
        private void StoreCurrentRegion(string regionName) {
            StoredRegion storedRegion = new StoredRegion(this.ConstructCurrentRegion(), regionName);

            int index = comboRegions.Items.Add(storedRegion);
            comboRegions.SelectedIndex = index;

            if (Settings.Default.SavedRegions == null)
                Settings.Default.SavedRegions = new StoredRegionArray();
            Settings.Default.SavedRegions.Add(storedRegion);
        }

        /// <summary>
        /// Internal event raised when a change occurs in the selected region.
        /// </summary>
        /// <param name="regionBounds">Region bounds.</param>
        protected virtual void OnRegionSet(ThumbnailRegion region) {
            //Forward region to thumbnail
            ParentForm.SelectedThumbnailRegion = region;

            //Have region, allowed to save
            buttonSave.Enabled = true;
        }

        #region GUI event handlers

        private void Close_click(object sender, EventArgs e) {
			OnRequestClosing();
		}
        
		private void Reset_click(object sender, EventArgs e) {
			Reset();
            ParentForm.SelectedThumbnailRegion = null;
		}

		private void Delete_click(object sender, EventArgs e) {
            if (comboRegions.SelectedIndex < 0)
                return;

            var origIndex = comboRegions.SelectedIndex;
			Settings.Default.SavedRegions.RemoveAt(origIndex);
			comboRegions.Items.RemoveAt(origIndex);

            if (comboRegions.Items.Count > 0)
                comboRegions.SelectedIndex = 0;
		}

		private void Save_click(object sender, EventArgs e) {
			//Display textbox instead of button
            buttonSave.Enabled = buttonDelete.Enabled = false;
			textRegionName.Visible = true;
			textRegionName.Focus();
		}

		private void Save_confirm(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(textRegionName.Text)) {
                StoreCurrentRegion(textRegionName.Text);
			}

			//Hide textbox and show button again
			textRegionName.Visible = false;
			textRegionName.Text = string.Empty;

            buttonSave.Enabled = buttonDelete.Enabled = true;
		}

		private void Save_lost(object sender, EventArgs e) {
			//Reset textbox
			textRegionName.Visible = false;
            textRegionName.Text = string.Empty;

            buttonSave.Enabled = buttonDelete.Enabled = true;
			buttonSave.Focus();
		}

        // Used to signal to the value change handler that all events should be temporarily ignored.
        bool _ignoreValueChanges = false;

        private void RegionValueSpinner_value_change(object sender, EventArgs e) {
            if (_ignoreValueChanges)
                return;

            OnRegionSet(ConstructCurrentRegion());
        }

        private void RegionCombo_index_change(object sender, EventArgs e) {
            buttonDelete.Enabled = (comboRegions.SelectedIndex >= 0);

            if (comboRegions.SelectedIndex >= 0) {
                //Load region
                var region = comboRegions.SelectedItem as StoredRegion;

                if (region == null)
                    return;

                SetRegion(region.Region);
            }
        }

        private void CheckRelative_checked(object sender, EventArgs e) {
            if (_ignoreValueChanges)
                return;

            //Get current region and switch mode
            var region = ConstructCurrentRegion();
            region.Relative = !region.Relative; //this must be reversed because the GUI has already switched state when calling ConstructCurrentRegion()
            if (checkRelative.Checked)
                region.SwitchToRelative(ParentForm.ThumbnailPanel.ThumbnailOriginalSize);
            else
                region.SwitchToAbsolute(ParentForm.ThumbnailPanel.ThumbnailOriginalSize);

            //Update GUI
            SetRegion(region);
        }

        #endregion

	}

}
