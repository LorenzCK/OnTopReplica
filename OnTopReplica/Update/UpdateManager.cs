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

        const string UpdateFeedUrl = "https://ontopreplica.codeplex.com/project/feeds/rss?ProjectRSSFeed=codeplex%3a%2f%2frelease%2fontopreplica";

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
            _checkRequest = (HttpWebRequest)HttpWebRequest.Create(UpdateFeedUrl);
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
                           let versionNumber = match.Groups["version"].Value
                           orderby versionNumber descending
                           select new { versionNumber, item.Element("link").Value };

            return new UpdateInformation();
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

        HttpWebRequest _downloadRequest;
        TaskDialog _updateDialog;
        bool _updateDownloaded = false;

        /// <summary>
        /// Asks confirmation for an update and installs the update.
        /// </summary>
        public void ConfirmAndInstall() {
            if (LastInformation == null || !LastInformation.IsNewVersion)
                return;

            AttachedForm.SafeInvoke(new Action(ConfirmAndInstallCore));
        }

        /// <summary>
        /// Core delegate that asks for update confirmation and installs. Must be called from GUI thread.
        /// </summary>
        private void ConfirmAndInstallCore() {
            _updateDialog = new TaskDialog {
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
            _updateDialog.ButtonClick += delegate(object sender, ClickEventArgs args) {
                if (args.ButtonID == (int)Result.OK) {
                    args.PreventClosing = true;

                    if (_updateDownloaded) {
                        //Terminate application
                        AttachedForm.Close();

                        //Launch updater
                        Process.Start(UpdateInstallerPath);
                    }   
                    else {
                        var downDlg = new TaskDialog {
                            Title = Strings.UpdateTitle,
                            Instruction = Strings.UpdateDownloadingInstruction,
                            ShowProgressBar = true,
                            ProgressBarMinRange = 0,
                            ProgressBarMaxRange = 100,
                            ProgressBarPosition = 0,
                            CommonButtons = TaskDialogButton.Cancel
                        };
                        _updateDialog.Navigate(downDlg);

                        _downloadRequest = (HttpWebRequest)HttpWebRequest.Create(LastInformation.DownloadInstaller);
                        _downloadRequest.BeginGetResponse(DownloadAsyncCallback, null);
                    }
                }
            };

            _updateDialog.Show(AttachedForm);
        }

        /// <summary>
        /// Gets the target filename used when downloading the update from the Internet.
        /// </summary>
        private string UpdateInstallerPath {
            get {
                var downloadPath = Native.FilesystemMethods.DownloadsPath;

                string versionName = (LastInformation != null) ?
                    LastInformation.LatestVersion.ToString() : string.Empty;
                string filename = string.Format("OnTopReplica-Update-{0}.exe", versionName);

                return Path.Combine(downloadPath, filename);
            }
        }

        /// <summary>
        /// Handles background downloading.
        /// </summary>
        private void DownloadAsyncCallback(IAsyncResult result) {
            if (_downloadRequest == null || _updateDialog == null)
                return;

            try {
                var response = _downloadRequest.EndGetResponse(result);
                var responseStream = response.GetResponseStream();
                long total = response.ContentLength;

                byte[] buffer = new byte[1024];

                using (var stream = new FileStream(UpdateInstallerPath, FileMode.Create)) {
                    int readTotal = 0;
                    while (true) {
                        int read = responseStream.Read(buffer, 0, buffer.Length);
                        readTotal += read;
                        
                        if (read <= 0) //EOF
                            break;

                        stream.Write(buffer, 0, read);

                        _updateDialog.Content = string.Format(Strings.UpdateDownloadingContent, readTotal, total);
                        _updateDialog.ProgressBarPosition = (int)((readTotal * 100.0) / total);
                    }
                }
            }
            catch (Exception ex) {
                DownloadShowError(ex.Message);
                return;
            }

            _updateDownloaded = true;

            var okDlg = new TaskDialog {
                Title = Strings.UpdateTitle,
                Instruction = Strings.UpdateReadyInstruction,
                Content = string.Format(Strings.UpdateReadyContent, LastInformation.LatestVersion),
                UseCommandLinks = true,
                CommonButtons = TaskDialogButton.Cancel,
                CustomButtons = new CustomButton[] {
                    new CustomButton(Result.OK, Strings.UpdateReadyCommandOk)
                }
            };
            _updateDialog.Navigate(okDlg);
        }

        private void DownloadShowError(string msg) {
            if (_updateDialog == null)
                return;

            _updateDialog.ProgressBarState = WindowsFormsAero.ProgressBar.States.Error;
            _updateDialog.Content = msg;
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
                Process.Start("http://ontopreplica.codeplex.com");
            };
            dlg.Show(AttachedForm);
        }

        #endregion

    }

}
