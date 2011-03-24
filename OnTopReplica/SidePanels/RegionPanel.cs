using System;
using System.Drawing;
using System.Windows.Forms;
using OnTopReplica.Properties;

namespace OnTopReplica.SidePanels {
	partial class RegionPanel : SidePanel {

		public RegionPanel() {
			InitializeComponent();

			//Copy settings into combo box
			if (Settings.Default.SavedRegions != null) {
				foreach (object o in Settings.Default.SavedRegions) {
					comboRegions.Items.Add(o);
				}
			}

            _regionDrawnHandler = new ThumbnailPanel.RegionDrawnHandler(ThumbnailPanel_RegionDrawn);
		}

        public override string Title {
            get {
                return Strings.MenuRegion;
            }
        }

        ThumbnailPanel.RegionDrawnHandler _regionDrawnHandler;

        public override void OnFirstShown(MainForm form) {
            base.OnFirstShown(form);

            //Init shown region if needed
            if (form.SelectedThumbnailRegion.HasValue)
                SetRegion(form.SelectedThumbnailRegion.Value);

            form.ThumbnailPanel.DrawMouseRegions = true;
            form.ThumbnailPanel.RegionDrawn += _regionDrawnHandler;
        }

        public override void OnClosing(MainForm form) {
            base.OnClosing(form);

            form.ThumbnailPanel.DrawMouseRegions = false;
            form.ThumbnailPanel.RegionDrawn -= _regionDrawnHandler;
        }

        void ThumbnailPanel_RegionDrawn(object sender, Rectangle region) {
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

            SetRegion(region.Bounds);

            //Select right combobox
            if (comboRegions.Items.Contains(region)) {
                comboRegions.SelectedItem = region;
            }
        }

        /// <summary>
        /// Sets the current selected region to a specific region rectangle.
        /// </summary>
        /// <param name="region">The region boundaries.</param>
		public void SetRegion(Rectangle region) {
			try {
				_ignoreValueChanges = true;

				numX.Enabled = numY.Enabled = numW.Enabled = numH.Enabled = true;

				numX.Value = region.Left;
				numY.Value = region.Top;
				numW.Value = region.Width;
				numH.Value = region.Height;
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

				buttonSave.Enabled = false;

				comboRegions.SelectedIndex = -1;
			}
			finally {
				_ignoreValueChanges = false;
			}
		}

        #endregion

        /// <summary>
        /// Adds a new stored region.
        /// </summary>
        /// <param name="rectangle">Region bounds.</param>
        /// <param name="regionName">Name of the region.</param>
        private void AddRegion(Rectangle rectangle, string regionName) {
            var region = new StoredRegion(rectangle, regionName);

            int index = comboRegions.Items.Add(region);
            comboRegions.SelectedIndex = index;

            if (Settings.Default.SavedRegions == null)
                Settings.Default.SavedRegions = new StoredRegionArray();
            Settings.Default.SavedRegions.Add(region);
        }

        /// <summary>
        /// Internal event raised when a change occurs in the selected region.
        /// </summary>
        /// <param name="regionBounds">Region bounds.</param>
        protected virtual void OnRegionSet(Rectangle regionBounds) {
            //Forward region to thumbnail
            ParentForm.SelectedThumbnailRegion = regionBounds;

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
				AddRegion(
                    new Rectangle((int)numX.Value, (int)numY.Value, (int)numW.Value, (int)numH.Value),
                    textRegionName.Text
                );
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

            OnRegionSet(new Rectangle((int)numX.Value, (int)numY.Value, (int)numW.Value, (int)numH.Value));
        }

        private void RegionCombo_index_change(object sender, EventArgs e) {
            buttonDelete.Enabled = (comboRegions.SelectedIndex >= 0);

            if (comboRegions.SelectedIndex >= 0) {
                //Load region
                var region = comboRegions.SelectedItem as StoredRegion;

                if (region == null)
                    return;

                SetRegion(region.Bounds);
            }
        }

        #endregion

	}
}
