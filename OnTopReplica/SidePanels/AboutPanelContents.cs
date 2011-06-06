using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Update;
using System.Diagnostics;
using WindowsFormsAero.TaskDialog;
using System.IO;

namespace OnTopReplica.SidePanels {
    public partial class AboutPanelContents : UserControl {

        EventHandler<UpdateCheckCompletedEventArgs> _updateHandler;

        public AboutPanelContents() {
            InitializeComponent();

            LocalizePanel();
        }

        private void LocalizePanel() {
            lblSlogan.Text = Strings.AboutSlogan;
            linkAuthor.Internationalize(Strings.AboutAuthor, Strings.AboutAuthorContent);
            labeledDivider1.Text = Strings.AboutDividerUpdates;
            lblUpdateDisclaimer.Text = Strings.AboutUpdatesDisclaimer;
            buttonUpdate.Text = Strings.AboutUpdatesCheckNow;
            labeledDivider2.Text = Strings.AboutDividerCredits;
            linkCredits.Internationalize(Strings.AboutCreditsSources, Strings.AboutCreditsSourcesContent);
            labelTranslators.Text = string.Format(Strings.AboutTranslators, Strings.AboutTranslatorsContent);
            labeledDivider3.Text = Strings.AboutDividerLicense;
            linkLicense.Internationalize(Strings.AboutLicense, Strings.AboutLicenseContent);
            labeledDivider4.Text = Strings.AboutDividerContribute;
            linkContribute.Internationalize(Strings.AboutContribute, Strings.AboutContributeContent);
        }

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);

            if (!DesignMode) {
                //Updating
                _updateHandler = new EventHandler<UpdateCheckCompletedEventArgs>(UpdateCheckCompleted);
                Program.Update.UpdateCheckCompleted += _updateHandler;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e) {
            base.OnHandleDestroyed(e);

            if (!DesignMode) {
                Program.Update.UpdateCheckCompleted -= _updateHandler;
            }
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

            Program.Update.CheckForUpdate();
        }

        void UpdateCheckCompleted(object sender, UpdateCheckCompletedEventArgs e) {
            this.Invoke(new Action(() => {
                if (!e.Success || e.Information == null) {
                    //TODO
                    MessageBox.Show("Failed to download update info.");
                }
                else if (!e.Information.IsNewVersion) {
                    Program.Update.DisplayInfo();
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
