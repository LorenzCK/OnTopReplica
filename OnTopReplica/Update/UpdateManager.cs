using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using WindowsFormsAero.TaskDialog;

namespace OnTopReplica.Update {

    /// <summary>
    /// Handles update checking and information display.
    /// </summary>
    class UpdateManager {

        /// <summary>
        /// Constructs a new update manager with an attached form.
        /// </summary>
        /// <param name="attachedForm">Form through which all GUI calls are made. Closing this form should terminate the application.</param>
        public UpdateManager(Form attachedForm) {
            if (attachedForm == null)
                throw new ArgumentNullException();

            AttachedForm = attachedForm;
        }

        /// <summary>
        /// Gets or sets the attached form (through which all GUI calls are made).
        /// </summary>
        protected Form AttachedForm { get; private set; }

        #region Checking

        /// <summary>
        /// Gets the latest update information available.
        /// </summary>
        public UpdateInformation LastInformation { get; private set; }

        HttpWebRequest _checkRequest;

        /// <summary>
        /// Checks for update asynchronously, updating update information.
        /// When check is completed, raises update events.
        /// </summary>
        public void CheckForUpdate() {
            if (_checkRequest != null) {
                _checkRequest.Abort();
            }

            //Build web request
            _checkRequest = (HttpWebRequest)HttpWebRequest.Create(AppStrings.UpdateFeed);
            _checkRequest.AllowAutoRedirect = true;
            _checkRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            _checkRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

            _checkRequest.BeginGetResponse(CheckForUpdateCallback, null);
        }

        /// <summary>
        /// Asynchronous callback that handles the update check request.
        /// </summary>
        private void CheckForUpdateCallback(IAsyncResult result) {
            if (_checkRequest == null)
                return;

            try {
                var response = _checkRequest.EndGetResponse(result);
                LastInformation = ParseUpdateCheckResponse(response.GetResponseStream());

                OnUpdateCheckSuccess(LastInformation);
            }
            catch (Exception ex) {
                OnUpdateCheckError(ex);
            }

            _checkRequest = null;
        }

        private Regex _versionExtractor = new Regex(@"^Released: Release (?<version>([0-9]\.){0,3}[0-9]?)", RegexOptions.Compiled | RegexOptions.Singleline);

        private UpdateInformation ParseUpdateCheckResponse(Stream stream) {
            var xdoc = XDocument.Load(stream);

            var releases = from item in xdoc.Descendants("item")
                           let title = item.Element("title").Value
                           let match = _versionExtractor.Match(title)
                           where match.Success
                           let versionNumber = new Version(match.Groups["version"].Value)
                           orderby versionNumber descending
                           select new { Version = versionNumber, Link = item.Element("link").Value, Date = item.Element("pubDate").Value };

            var lastRelease = releases.FirstOrDefault();

            return new UpdateInformation(lastRelease.Version, lastRelease.Link, lastRelease.Date);
        }

        #endregion

        #region Eventing

        public event EventHandler<UpdateCheckCompletedEventArgs> UpdateCheckCompleted;

        protected virtual void OnUpdateCheckError(Exception ex) {
            var evt = UpdateCheckCompleted;
            if (evt != null) {
                evt(this, new UpdateCheckCompletedEventArgs {
                    Success = false,
                    Error = ex
                });
            }
        }

        protected virtual void OnUpdateCheckSuccess(UpdateInformation information) {
            var evt = UpdateCheckCompleted;
            if (evt != null) {
                evt(this, new UpdateCheckCompletedEventArgs {
                    Success = true,
                    Information = information
                });
            }
        }

        #endregion

        #region Updating

        /// <summary>
        /// Asks confirmation for an update and installs the update (if available).
        /// </summary>
        public void ConfirmAndInstall() {
            if (LastInformation == null || !LastInformation.IsNewVersionAvailable)
                return;

            AttachedForm.SafeInvoke(new Action(ConfirmAndInstallCore));
        }

        /// <summary>
        /// Core delegate that asks for update confirmation and installs. Must be called from GUI thread.
        /// </summary>
        private void ConfirmAndInstallCore() {
            var updateDialog = new TaskDialog {
                Title = Strings.UpdateTitle,
                Instruction = string.Format(Strings.UpdateAvailableInstruction, LastInformation.LatestVersion),
                Content = Strings.UpdateAvailableContent,
                CustomButtons = new CustomButton[] {
                    new CustomButton(Result.OK, string.Format(Strings.UpdateAvailableCommandOk, LastInformation.LatestVersion)),
                    new CustomButton(Result.Cancel, Strings.UpdateAvailableCommandCancel)
                },
                UseCommandLinks = true,
                CommonIcon = TaskDialogIcon.Information,
                ExpandedInformation = string.Format(Strings.UpdateAvailableExpanded, LastInformation.CurrentVersion, LastInformation.LatestVersion),
            };
            if (updateDialog.Show(AttachedForm).CommonButton == Result.OK) {
                Shell.Execute(LastInformation.DownloadPage);
            }
        }

        /// <summary>
        /// Displays some information about the current installation and available updates.
        /// </summary>
        public void DisplayInfo() {
            AttachedForm.SafeInvoke(new Action(DisplayInfoCore));
        }

        /// <summary>
        /// Displays info. Called from GUI thread.
        /// </summary>
        private void DisplayInfoCore() {
            //No updates, but need to inform the user
            var dlg = new TaskDialog {
                Title = Strings.UpdateTitle,
                Instruction = Strings.UpdateInfoInstruction,
                Content = Strings.UpdateInfoContent,
                EnableHyperlinks = true,
                CommonButtons = TaskDialogButton.Close,
                AllowDialogCancellation = true,
                CommonIcon = TaskDialogIcon.Information,
                Footer = string.Format(Strings.UpdateInfoFooter, LastInformation.LatestVersionRelease.ToLongDateString())
            };
            dlg.HyperlinkClick += delegate(object sender, HyperlinkEventArgs args) {
                Shell.Execute(AppStrings.ApplicationWebsite);
            };

            dlg.Show(AttachedForm);
        }

        #endregion

    }

}
