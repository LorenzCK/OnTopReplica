using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Update;
using System.Diagnostics;
using VistaControls.TaskDialog;
using System.IO;

namespace OnTopReplica.SidePanels {
    public partial class AboutPanelContents : UserControl {

        UpdateManager _updater;

        public AboutPanelContents() {
            InitializeComponent();

            //Localized strings
            lblSlogan.Text = Strings.AboutSlogan;
            InternationalizeLinkLabel(linkAuthor, Strings.AboutAuthor, Strings.AboutAuthorContent);
            labeledDivider1.Text = Strings.AboutDividerUpdates;
            lblUpdateDisclaimer.Text = Strings.AboutUpdatesDisclaimer;
            buttonUpdate.Text = Strings.AboutUpdatesCheckNow;
            labeledDivider2.Text = Strings.AboutDividerCredits;
            InternationalizeLinkLabel(linkCredits, Strings.AboutCreditsSources, Strings.AboutCreditsSourcesContent);
            labelTranslators.Text = string.Format(Strings.AboutTranslators, Strings.AboutTranslatorsContent);
            labeledDivider3.Text = Strings.AboutDividerLicense;
            InternationalizeLinkLabel(linkLicense, Strings.AboutLicense, Strings.AboutLicenseContent);
            labeledDivider4.Text = Strings.AboutDividerContribute;
            InternationalizeLinkLabel(linkContribute, Strings.AboutContribute, Strings.AboutContributeContent);

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

        private void LinkHomepage_clicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://ontopreplica.codeplex.com");
        }

        private void LinkAuthor_clicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://lorenz.klopfenstein.net");
        }

        private void LinkCredits_click(object sender, LinkLabelLinkClickedEventArgs e) {
            var exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            var filePath = Path.Combine(exeDir, "CREDITS.txt");

            Process.Start(filePath);
        }

        void UpdateButton_click(object sender, System.EventArgs e) {
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

        private void LinkLicense_click(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://opensource.org/licenses/ms-rl.html");
        }

        private void LinkContribute_clicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://ontopreplica.codeplex.com/SourceControl/list/changesets");
        }
    }
}
