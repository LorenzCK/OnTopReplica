using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VistaControls.Dwm;

namespace OnTopReplica.SidePanels {
    partial class AboutPanel : SidePanel {

        public AboutPanel() {
            InitializeComponent();

            labelVersion.Text = string.Format(Strings.AboutVersion, Application.ProductVersion);
        }

        public override string Title {
            get {
                return Strings.AboutTitle;
            }
        }

        public override Margins? GlassMargins {
            get {
                return new Margins(0, 0, 0, labelVersion.Height);
            }
        }

    }
}
