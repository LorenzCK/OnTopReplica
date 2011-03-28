using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VistaControls.Dwm;
using System.Diagnostics;
using OnTopReplica.Update;
using VistaControls.TaskDialog;

namespace OnTopReplica.SidePanels {
    partial class AboutPanel : SidePanel {

        UpdateManager _updater;

        public AboutPanel() {
            InitializeComponent();

            //Update dynamic strings
            thlabelVersion.Text = string.Format(Strings.AboutVersion, Application.ProductVersion);
            InternationalizeLinkLabel(linkAuthor, Strings.AboutAuthor, Strings.AboutAuthorContent);
            InternationalizeLinkLabel(linkCredits, Strings.AboutCreditsSources, Strings.AboutCreditsSourcesContent);

            //Updating
            _updater = new UpdateManager();
            _updater.UpdateCheckCompleted += new EventHandler<UpdateCheckCompletedEventArgs>(UpdateCheckCompleted);
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

        void ButtonUpdate_click(object sender, System.EventArgs e) {
            progressUpdate.Visible = true;

            _updater.CheckForUpdate();
        }

        void UpdateCheckCompleted(object sender, UpdateCheckCompletedEventArgs e) {
            this.Invoke(new Action(() => {
                var topForm = this.TopLevelControl as Form;

                if (e.Success) {
                    _updater.HandleUpdateCheck(topForm, e.Information, true);
                }
                else {
                    var dlg = new TaskDialog(Strings.ErrorUpdate, Strings.ErrorUpdate, Strings.ErrorUpdateContentGeneric) {
                        CommonIcon = TaskDialogIcon.Stop,
                        CommonButtons = TaskDialogButton.OK
                    };
                    dlg.Show(topForm);
                }

                progressUpdate.Visible = false;
            }));
        }

    }
}
