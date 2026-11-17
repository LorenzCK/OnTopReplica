using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Threading;
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
        /// When check is completed, raises update events (on a background thread).
        /// </summary>
        public void CheckForUpdate() {
            //HttpWebRequest.BeginGetResponse resolves the host name and detects proxy settings
            //synchronously on the calling thread: when called from the GUI thread, a slow or failing
            //DNS lookup (which can take 10+ seconds) freezes the whole UI. Start it from a worker thread.
            ThreadPool.QueueUserWorkItem(delegate {
                BeginCheckForUpdate();
            });
        }

        private void BeginCheckForUpdate() {
            try {
                //Build web request
                var request = (HttpWebRequest)WebRequest.Create(AppStrings.UpdateFeed);
                request.AllowAutoRedirect = true;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

                //Supersede any check still in progress
                var previousRequest = Interlocked.Exchange(ref _checkRequest, request);
                if (previousRequest != null) {
                    previousRequest.Abort();
                }

                request.BeginGetResponse(CheckForUpdateCallback, request);
            }
            catch (Exception ex) {
                OnUpdateCheckError(ex);
            }
        }

        /// <summary>
        /// Asynchronous callback that handles the update check request.
        /// </summary>
        private void CheckForUpdateCallback(IAsyncResult result) {
            var request = (HttpWebRequest)result.AsyncState;

            //Ignore results of checks that have been superseded by a newer one
            if (Interlocked.CompareExchange(ref _checkRequest, null, request) != request)
                return;

            try {
                using (var response = request.EndGetResponse(result)) {
                    LastInformation = ParseUpdateCheckResponse(response.GetResponseStream());
                }

                OnUpdateCheckSuccess(LastInformation);
            }
            catch (Exception ex) {
                OnUpdateCheckError(ex);
            }
        }

        static readonly XNamespace AtomNamespace = "http://www.w3.org/2005/Atom";

        //Extracts the version number from a release page URL, which ends with the release's git tag
        //(for instance ".../releases/tag/v3.5.1" or ".../releases/tag/v3.5.1-fix")
        private Regex _versionExtractor = new Regex(@"/releases/tag/v?(?<version>[0-9]+(\.[0-9]+){1,3})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Parses the Atom feed of the project's GitHub releases and returns information about the latest one.
        /// </summary>
        private UpdateInformation ParseUpdateCheckResponse(Stream stream) {
            var xdoc = XDocument.Load(stream);

            var releases = from entry in xdoc.Descendants(AtomNamespace + "entry")
                           let link = (from l in entry.Elements(AtomNamespace + "link")
                                       where ((string)l.Attribute("rel") ?? "alternate") == "alternate"
                                       select (string)l.Attribute("href")).FirstOrDefault()
                           where link != null
                           let match = _versionExtractor.Match(link)
                           where match.Success
                           let versionNumber = NormalizeVersion(new Version(match.Groups["version"].Value))
                           orderby versionNumber descending
                           select new UpdateInformation(versionNumber, link, (DateTime?)entry.Element(AtomNamespace + "updated"));

            var lastRelease = releases.FirstOrDefault();
            if (lastRelease == null)
                throw new InvalidDataException("The update feed does not list any release with a recognizable version number.");

            return lastRelease;
        }

        /// <summary>
        /// Fills in missing version components with zeros (i.e. "3.5.1" becomes "3.5.1.0"),
        /// so that versions compare correctly with the 4-component assembly version.
        /// </summary>
        private static Version NormalizeVersion(Version v) {
            return new Version(v.Major, v.Minor, Math.Max(v.Build, 0), Math.Max(v.Revision, 0));
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
                    new CustomButton(CommonButtonResult.OK, string.Format(Strings.UpdateAvailableCommandOk, LastInformation.LatestVersion)),
                    new CustomButton(CommonButtonResult.Cancel, Strings.UpdateAvailableCommandCancel)
                },
                UseCommandLinks = true,
                CommonIcon = CommonIcon.Information,
                ExpandedInformation = string.Format(Strings.UpdateAvailableExpanded, LastInformation.CurrentVersion, LastInformation.LatestVersion),
            };
            if (updateDialog.Show(AttachedForm).CommonButton == CommonButtonResult.OK) {
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
                CommonButtons = CommonButton.Close,
                AllowDialogCancellation = true,
                CommonIcon = CommonIcon.Information,
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
