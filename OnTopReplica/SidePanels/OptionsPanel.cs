using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Properties;
using System.Globalization;

namespace OnTopReplica.SidePanels {
    partial class OptionsPanel : SidePanel {

        public OptionsPanel() {
            InitializeComponent();

            PopulateLanguageComboBox();
        }

        private void Close_click(object sender, EventArgs e) {
            OnRequestClosing();
        }

        public override string Title {
            get {
                return Strings.MenuSettings;
            }
        }

        #region Language

        Pair<CultureInfo, Image>[] _languageList = {
            new Pair<CultureInfo, Image>(new CultureInfo("en-US"), Resources.flag_usa),
            new Pair<CultureInfo, Image>(new CultureInfo("it-IT"), Resources.flag_ita),
            new Pair<CultureInfo, Image>(new CultureInfo("cs-CZ"), Resources.flag_czech),
            new Pair<CultureInfo, Image>(new CultureInfo("da-DK"), Resources.flag_danish),
            new Pair<CultureInfo, Image>(new CultureInfo("es-ES"), Resources.flag_spanish),
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
                var item = new ImageComboBoxItem(langPair.Item1.NativeName, imageList.Images.Count) {
                    Tag = langPair.Item1
                };
                imageList.Images.Add(langPair.Item2);
                comboLanguage.Items.Add(item);

                if (langPair.Item1.Equals(CultureInfo.CurrentUICulture) ||
                    (CultureInfo.CurrentUICulture.Equals(CultureInfo.InvariantCulture) && langPair.Item1.TwoLetterISOLanguageName.Equals("en"))) {
                    selectedIndex = comboLanguage.Items.Count - 1;
                }
            }

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
