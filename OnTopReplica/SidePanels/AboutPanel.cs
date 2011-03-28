using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VistaControls.Dwm;
using System.Diagnostics;

namespace OnTopReplica.SidePanels {
    partial class AboutPanel : SidePanel {
        public AboutPanel() {
            InitializeComponent();

            //Update dynamic strings
            thlabelVersion.Text = string.Format(Strings.AboutVersion, Application.ProductVersion);
            InternationalizeLinkLabel(linkAuthor, Strings.AboutAuthor, Strings.AboutAuthorContent);
            InternationalizeLinkLabel(linkCredits, Strings.AboutCreditsSources, Strings.AboutCreditsSourcesContent);
        }

        private void InternationalizeLinkLabel(LinkLabel label, string text, string linkText) {
            int linkIndex = text.IndexOf('%');
            if (linkIndex == -1) {
                //Shouldn't happen, but try to fail with meaningful text
                label.Text = text;
                return;
            }

            label.Text = text.Substring(0, linkIndex) + linkText + text.Substring(linkIndex + 1);
            label.LinkArea = new LinkArea(linkIndex, linkText.Length);
        }

        public override string Title {
            get {
                return Strings.AboutTitle;
            }
        }

        public override Margins? GlassMargins {
            get {
                return new Margins(0, 0, 0, thlabelVersion.Height);
            }
        }

        #region Link clicks

        private void LinkAuthor_click(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://lorenz.klopfenstein.net");
        }

        private void LinkWebsite_click(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://ontopreplica.codeplex.com");
        }

        private void LinkCredits_click(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("CREDITS.txt");
        }

        #endregion

    }
}
