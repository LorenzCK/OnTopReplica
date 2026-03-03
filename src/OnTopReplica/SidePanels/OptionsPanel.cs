using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Properties;
using System.Globalization;

namespace OnTopReplica.SidePanels {
    partial class OptionsPanel : SidePanel {

        public OptionsPanel() {
            InitializeComponent();

            LocalizePanel();
        }

        private void LocalizePanel() {
            groupLanguage.Text = Strings.SettingsLanguageTitle;
            lblLanguage.Text = Strings.SettingsRestartRequired;

            groupHotkeys.Text = Strings.SettingsHotKeyTitle;
            lblHotKeyShowHide.Text = Strings.SettingsHotKeyShowHide;
            lblHotKeyClone.Text = Strings.SettingsHotKeyClone;
            label1.Text = Strings.SettingsHotKeyDescription;

            btnClose.Text = Strings.MenuClose;
        }

        public override void OnFirstShown(MainForm form) {
            base.OnFirstShown(form);

            PopulateLanguageComboBox();

            //Stop hotkey handling and load current shortcuts
            form.MessagePumpManager.Get<OnTopReplica.MessagePumpProcessors.HotKeyManager>().Enabled = false;
            txtHotKeyShowHide.Text = Settings.Default.HotKeyShowHide;
            txtHotKeyClone.Text = Settings.Default.HotKeyCloneCurrent;
        }

        private void Close_click(object sender, EventArgs e) {
            OnRequestClosing();
        }

        public override string Title {
            get {
                return Strings.SettingsTitle;
            }
        }

        public override void OnClosing(MainForm form) {
            base.OnClosing(form);

            //Update hotkey settings and update processor
            Settings.Default.HotKeyShowHide = txtHotKeyShowHide.Text;
            Settings.Default.HotKeyCloneCurrent = txtHotKeyClone.Text;
            var manager = form.MessagePumpManager.Get<OnTopReplica.MessagePumpProcessors.HotKeyManager>();
            manager.RefreshHotkeys();
            manager.Enabled = true;
        }

        #region Language

        class CultureWrapper {
            public CultureWrapper(string name, CultureInfo culture, Image img) {
                Culture = culture;
                Image = img;
                Name = name;
            }
            public CultureInfo Culture { get; set; }
            public Image Image { get; set; }
            public string Name { get; set; }
        }

        // only Chinese languages are kept per requirements
        CultureWrapper[] _languageList = {
            new CultureWrapper("简体中文", new CultureInfo("zh-CN"), Resources.flag_china),
            new CultureWrapper("繁體中文", new CultureInfo("zh-TW"), Resources.flag_taiwan),
        };

        private void PopulateLanguageComboBox() {
            comboLanguage.Items.Clear();

            var imageList = new ImageList() {
                ImageSize = new Size(16, 16),
                ColorDepth = ColorDepth.Depth32Bit
            };
            comboLanguage.IconList = imageList;

            int selectedIndex = -1;
            foreach (var langPair in _languageList) {
                var item = new ImageComboBoxItem(langPair.Name, imageList.Images.Count) {
                    Tag = langPair.Culture
                };
                imageList.Images.Add(langPair.Image);
                comboLanguage.Items.Add(item);

                if (langPair.Culture.Equals(CultureInfo.CurrentUICulture)) {
                    selectedIndex = comboLanguage.Items.Count - 1;
                }
            }

            //Handle case when there is not explicitly set culture (default to first one, i.e. english)
            if (CultureInfo.CurrentUICulture.Equals(CultureInfo.InvariantCulture))
                selectedIndex = 0;

            comboLanguage.SelectedIndex = selectedIndex;
        }

        private void LanguageBox_IndexChange(object sender, EventArgs e) {
            var item = comboLanguage.SelectedItem as ImageComboBoxItem;
            if (item == null)
                return;

            Settings.Default.Language = item.Tag as CultureInfo;
        }

        #endregion

    }

}
