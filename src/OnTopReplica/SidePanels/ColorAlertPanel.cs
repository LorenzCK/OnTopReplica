using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using OnTopReplica.Properties;
using OnTopReplica.MessagePumpProcessors;

namespace OnTopReplica.SidePanels {
    partial class ColorAlertPanel : SidePanel {

        public ColorAlertPanel() {
            InitializeComponent();
            LocalizePanel();
        }

        private void LocalizePanel() {
            groupColor.Text = Strings.ColorAlert_Title;
            labelInterval.Text = Strings.ColorAlert_Interval;
            checkEnabled.Text = Strings.ColorAlert_Enable;
            labelVolume.Text = Strings.ColorAlert_Volume;
            labelSoundFile.Text = Strings.ColorAlert_SoundFile;
            btnClose.Text = Strings.MenuClose;

            tooltipInfo.SetToolTip(labelInterval, Strings.ColorAlert_IntervalTooltip);
        }

        ColorDetectionProcessor _processor;
        private bool _loading = false; // suppress CheckColor_CheckedChanged during panel init
        private const string SoundsRelativePath = "Sounds";
        private const string ColorConfigFileName = "ColorAlertConfig.json";

        private string GetColorConfigPath() {
            return Path.Combine(AppPaths.PrivateRoamingFolderPath, ColorConfigFileName);
        }

        public override void OnFirstShown(MainForm form) {
            base.OnFirstShown(form);
            Log.Write("ColorAlertPanel shown size {0}", this.Size);

            try {
                _processor = form.MessagePumpManager.Get<ColorDetectionProcessor>();
            }
            catch {
                _processor = null;
            }

            _loading = true;
            try {
                if (_processor != null) {
                    checkEnabled.Checked = _processor.Enabled;
                    numInterval.Value = _processor.SampleInterval;
                }

                // Priority: 1) processor current session state  2) config file  3) Settings  4) default Red
                HashSet<ColorCategory> categories = null;

                // Use processor's current categories if it has been configured this session
                // (detected by: processor is non-null and has categories different from factory default,
                //  OR processor is currently enabled — meaning user has actively used it)
                bool processorHasSessionState = _processor != null &&
                    (_processor.Enabled || _processor.EnabledCategories.Count != 1 ||
                    !_processor.EnabledCategories.Contains(ColorCategory.Red));

                if (processorHasSessionState) {
                    categories = new HashSet<ColorCategory>(_processor.EnabledCategories);
                    Log.Write("Loaded ColorAlert categories from processor: {0}", CategoriesToString(categories));
                }

                if (categories == null || categories.Count == 0) {
                    categories = LoadCategoriesFromFile();
                }
                if (categories == null || categories.Count == 0) {
                    categories = ParseCategoriesFromString(Settings.Default.ColorAlertTargetColors);
                }
                if ((categories == null || categories.Count == 0) && _processor != null) {
                    categories = new HashSet<ColorCategory>(_processor.EnabledCategories);
                }
                if (categories == null || categories.Count == 0) {
                    categories = new HashSet<ColorCategory> { ColorCategory.Red };
                }

                // Set checkboxes (events suppressed by _loading flag)
                checkRed.Checked = categories.Contains(ColorCategory.Red);
                checkOrange.Checked = categories.Contains(ColorCategory.Orange);
                checkGray.Checked = categories.Contains(ColorCategory.Gray);

                Log.Write("Loaded ColorAlert categories: {0}", CategoriesToString(categories));

                // Sync to processor
                if (_processor != null) {
                    _processor.EnabledCategories = new HashSet<ColorCategory>(categories);
                }
            }
            finally {
                _loading = false;
            }

            // Load volume and sound settings
            trackBarVolume.Value = (int)(Settings.Default.ColorAlertVolume * 100);
            PopulateSoundList();
            if (!string.IsNullOrEmpty(Settings.Default.ColorAlertSoundFile)) {
                var f = Path.GetFileName(Settings.Default.ColorAlertSoundFile);
                if (comboSound.Items.Contains(f))
                    comboSound.SelectedItem = f;
            }
        }

        public override string Title {
            get {
                return "Color Alert";
            }
        }

        public override void OnClosing(MainForm form) {
            base.OnClosing(form);

            var categories = GetEnabledCategories();

            if (_processor != null) {
                _processor.EnabledCategories = new HashSet<ColorCategory>(categories);
                _processor.SampleInterval = (int)numInterval.Value;
                _processor.Enabled = checkEnabled.Checked;
                _processor.AlarmVolume = trackBarVolume.Value / 100f;
                if (comboSound.SelectedItem != null) {
                    string file = comboSound.SelectedItem.ToString();
                    _processor.AlarmSoundFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), SoundsRelativePath, file);
                }
            }

            // Persist settings
            Settings.Default.ColorAlertVolume = trackBarVolume.Value / 100f;
            if (comboSound.SelectedItem != null) {
                string file = comboSound.SelectedItem.ToString();
                Settings.Default.ColorAlertSoundFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), SoundsRelativePath, file);
            }
            Settings.Default.ColorAlertTargetColors = CategoriesToString(categories);

            SaveCategoriesToFile(categories);
            Settings.Default.Save();
        }

        /// <summary>
        /// Gets the set of enabled color categories from the UI checkboxes.
        /// </summary>
        private HashSet<ColorCategory> GetEnabledCategories() {
            var cats = new HashSet<ColorCategory>();
            if (checkRed.Checked) cats.Add(ColorCategory.Red);
            if (checkOrange.Checked) cats.Add(ColorCategory.Orange);
            if (checkGray.Checked) cats.Add(ColorCategory.Gray);
            return cats;
        }

        /// <summary>
        /// Serializes categories to a comma-separated string.
        /// </summary>
        private string CategoriesToString(HashSet<ColorCategory> categories) {
            if (categories == null || categories.Count == 0) return string.Empty;
            var parts = new List<string>();
            foreach (var c in categories) parts.Add(c.ToString());
            return string.Join(",", parts);
        }

        /// <summary>
        /// Parses a comma-separated category string.
        /// </summary>
        private HashSet<ColorCategory> ParseCategoriesFromString(string text) {
            var result = new HashSet<ColorCategory>();
            if (string.IsNullOrWhiteSpace(text)) return result;

            var parts = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts) {
                string s = part.Trim();
                ColorCategory cat;
                if (Enum.TryParse(s, true, out cat) && cat != ColorCategory.None) {
                    result.Add(cat);
                }
            }
            return result;
        }

        private void SaveCategoriesToFile(HashSet<ColorCategory> categories) {
            try {
                string path = GetColorConfigPath();
                string data = CategoriesToString(categories);
                File.WriteAllText(path, data, Encoding.UTF8);
                Log.Write("Saved color config to {0}: {1}", path, data);
            }
            catch (Exception ex) {
                Log.Write("Error saving color config: {0}", ex.Message);
            }
        }

        private HashSet<ColorCategory> LoadCategoriesFromFile() {
            try {
                string path = GetColorConfigPath();
                if (!File.Exists(path)) return null;
                string data = File.ReadAllText(path, Encoding.UTF8);

                // Try new format (comma-separated category names)
                var result = ParseCategoriesFromString(data);
                if (result.Count > 0) {
                    Log.Write("Loaded color config from {0}: {1}", path, data);
                    return result;
                }

                // Legacy format: JSON array of hex colors → map to categories
                if (data.TrimStart().StartsWith("[")) {
                    result = MapLegacyColorsToCategories(data);
                    if (result.Count > 0) {
                        Log.Write("Loaded legacy color config from {0}, mapped to: {1}", path, CategoriesToString(result));
                        return result;
                    }
                }
            }
            catch (Exception ex) {
                Log.Write("Error loading color config: {0}", ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Maps legacy hex color values to predefined categories for backwards compatibility.
        /// </summary>
        private HashSet<ColorCategory> MapLegacyColorsToCategories(string json) {
            var result = new HashSet<ColorCategory>();
            try {
                string trimmed = json.Trim().Trim('[', ']');
                var parts = trimmed.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts) {
                    string s = p.Trim().Trim('"').Trim();
                    if (s.StartsWith("#")) s = s.Substring(1);
                    if (s.Length == 6) {
                        int rgb = int.Parse(s, System.Globalization.NumberStyles.HexNumber);
                        int r = (rgb >> 16) & 0xFF;
                        int g = (rgb >> 8) & 0xFF;
                        int b = rgb & 0xFF;

                        // Simple hue-based classification
                        float max = Math.Max(r, Math.Max(g, b)) / 255f;
                        float min = Math.Min(r, Math.Min(g, b)) / 255f;
                        float sat = max > 0 ? (max - min) / max * 100 : 0;

                        if (sat < 15) {
                            result.Add(ColorCategory.Gray);
                        } else if (r > g && r > b) {
                            // Red or orange
                            if (g > b && g > r * 0.3f) {
                                result.Add(ColorCategory.Orange);
                            } else {
                                result.Add(ColorCategory.Red);
                            }
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        private void CheckEnabled_CheckedChanged(object sender, EventArgs e) {
            if (_loading) return; // suppress during panel initialization
            if (_processor != null) {
                _processor.Enabled = checkEnabled.Checked;
                if (checkEnabled.Checked) {
                    SyncSettingsToProcessor();
                }
            }
        }

        private void CheckColor_CheckedChanged(object sender, EventArgs e) {
            if (_loading) return; // suppress during panel initialization
            var categories = GetEnabledCategories();
            if (_processor != null) {
                _processor.EnabledCategories = new HashSet<ColorCategory>(categories);
                Log.Write("Color categories changed: {0}", CategoriesToString(categories));
            }
            // Persist immediately so reopening the panel restores the correct state
            SaveCategoriesToFile(categories);
        }

        /// <summary>
        /// Syncs all current panel settings to the color detection processor.
        /// </summary>
        private void SyncSettingsToProcessor() {
            if (_processor == null) return;
            _processor.EnabledCategories = GetEnabledCategories();
            _processor.SampleInterval = (int)numInterval.Value;
            _processor.AlarmVolume = trackBarVolume.Value / 100f;
            if (comboSound.SelectedItem != null) {
                string file = comboSound.SelectedItem.ToString();
                _processor.AlarmSoundFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), SoundsRelativePath, file);
            }
            Log.Write("SyncSettingsToProcessor: categories={0}", CategoriesToString(_processor.EnabledCategories));
        }

        private void TrackBarVolume_Scroll(object sender, EventArgs e) {
            if (_processor != null) {
                _processor.AlarmVolume = trackBarVolume.Value / 100f;
                Log.Write("Alarm volume set to {0}", _processor.AlarmVolume);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e) {
            OnRequestClosing();
        }

        private void NumInterval_ValueChanged(object sender, EventArgs e) {
            if (_processor != null) {
                _processor.SampleInterval = (int)numInterval.Value;
            }
        }

        private void ComboSound_SelectedIndexChanged(object sender, EventArgs e) {
            if (_processor != null && comboSound.SelectedItem != null) {
                string file = comboSound.SelectedItem.ToString();
                string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), SoundsRelativePath, file);
                _processor.AlarmSoundFile = path;
            }
        }

        private void PopulateSoundList() {
            comboSound.Items.Clear();
            string dir = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), SoundsRelativePath);
            if (Directory.Exists(dir)) {
                foreach (var ext in new[] { "*.wav", "*.mp3" }) {
                    foreach (var f in Directory.GetFiles(dir, ext)) {
                        comboSound.Items.Add(Path.GetFileName(f));
                    }
                }
            }
            Log.Write("PopulateSoundList found {0} files", comboSound.Items.Count);
        }

        private void BtnTestAlarm_Click(object sender, EventArgs e) {
            if (comboSound.SelectedItem != null) {
                string file = comboSound.SelectedItem.ToString();
                string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), SoundsRelativePath, file);
                float volume = trackBarVolume.Value / 100f;
                try {
                    var player = new System.Windows.Media.MediaPlayer();
                    player.Open(new Uri(path));
                    player.Volume = Math.Max(0, Math.Min(1, volume));
                    player.Play();
                    Log.Write("Test alarm: playing {0} at volume {1}", file, volume);
                }
                catch (Exception ex) {
                    Log.Write("Test alarm error: {0}", ex.Message);
                }
            }
            else {
                Log.Write("Test alarm: no sound file selected");
            }
        }
    }
}
