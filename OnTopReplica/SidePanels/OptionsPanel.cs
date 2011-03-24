using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica.SidePanels {
    partial class OptionsPanel : SidePanel {
        public OptionsPanel() {
            InitializeComponent();
        }

        private void Close_click(object sender, EventArgs e) {
            OnRequestClosing();
        }

        public override string Title {
            get {
                return Strings.MenuSettings;
            }
        }
    }
}
