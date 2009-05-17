using System;
using System.Drawing;
using System.Windows.Forms;
using OnTopReplica.Properties;

namespace OnTopReplica {
	public partial class RegionBox : UserControl {

		public RegionBox() {
			InitializeComponent();

			//Copy settings into combo box
			if (Settings.Default.SavedRegions != null) {
				foreach (object o in Settings.Default.SavedRegions) {
					comboBox1.Items.Add(o);
				}
			}
		}

		void Default_SettingsLoaded(object sender, System.Configuration.SettingsLoadedEventArgs e) {
			
		}

		bool _glassMode = true;

		public bool GlassMode {
			get {
				return _glassMode;
			}
			set {
				BackColor = (value) ? Color.Black : SystemColors.Control;

				_glassMode = value;
			}
		}

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

		public void Reset() {
			try {
				_ignoreValueChanges = true;

				numX.Value = numY.Value = numW.Value = numH.Value = 0;

				numX.Enabled = numY.Enabled = numW.Enabled = numH.Enabled = false;

				buttonSave.Enabled = false;

				comboBox1.SelectedIndex = -1;
			}
			finally {
				_ignoreValueChanges = false;
			}
		}

		public event EventHandler RequestClosing;

		protected virtual void OnRequestClosing() {
			if (RequestClosing != null)
				RequestClosing(this, EventArgs.Empty);
		}

		private void CloseClick(object sender, EventArgs e) {
			OnRequestClosing();
		}

		public event EventHandler RequestRegionReset;

		protected virtual void OnRequestRegionReset() {
			if (RequestRegionReset != null)
				RequestRegionReset(this, EventArgs.Empty);
		}

		private void ResetClick(object sender, EventArgs e) {
			Reset();

			OnRequestRegionReset();
		}

		private void DeleteClick(object sender, EventArgs e) {
			Settings.Default.SavedRegions.RemoveAt(comboBox1.SelectedIndex);
			comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
		}

		private void SaveClick(object sender, EventArgs e) {
			//Display textbox instead of button
			buttonSave.Visible = false;
			textRegionName.Visible = true;
			textRegionName.Focus();
		}

		private void Save_confirm(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty(textRegionName.Text)) {
				AddRegion(new Rectangle((int)numX.Value, (int)numY.Value, (int)numW.Value, (int)numH.Value), textRegionName.Text);
			}

			//Hide textbox and show button again
			textRegionName.Visible = false;
			textRegionName.Text = string.Empty;
			
			buttonSave.Visible = true;
			buttonSave.Enabled = false;
		}

		private void Save_lost(object sender, EventArgs e) {
			//Reset textbox
			textRegionName.Visible = false;

			buttonSave.Visible = true;
			buttonSave.Focus();
		}

		private void AddRegion(Rectangle rectangle, string p) {
			var region = new StoredRegion(rectangle, p);

			int index = comboBox1.Items.Add(region);
			comboBox1.SelectedIndex = index;

			if (Settings.Default.SavedRegions == null)
				Settings.Default.SavedRegions = new StoredRegionArray();
			Settings.Default.SavedRegions.Add(region);
		}

		public delegate void RegionSetHandler(object sender, Rectangle region);

		public event RegionSetHandler RegionSet;

		protected virtual void OnRegionSet(Rectangle region){
			if (RegionSet != null)
				RegionSet(this, region);

			buttonSave.Enabled = true;
		}

		/// <summary>Used to signal to the value change handler that all events should be temporarily ignored.</summary>
		/// <remarks>Must be used if the RegionBox is updating the values of the NumericUpDown controls internally and the handler
		/// should not raise any event.</remarks>
		bool _ignoreValueChanges = false;

		private void RegionValueChanged(object sender, EventArgs e) {
			if (_ignoreValueChanges)
				return;

			OnRegionSet(new Rectangle((int)numX.Value, (int)numY.Value, (int)numW.Value, (int)numH.Value));
		}

		private void RegionCombo_index(object sender, EventArgs e) {
			buttonDelete.Enabled = (comboBox1.SelectedIndex >= 0);

			if (comboBox1.SelectedIndex >= 0) {
				//Load region
				var region = comboBox1.SelectedItem as StoredRegion;
				
				if (region == null)
					return;

				SetRegion(region.Rect);
			}
		}

	}
}
