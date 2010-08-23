using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Cache;
using System.IO;
using System.Reflection;
using VistaControls.TaskDialog;
using System.Diagnostics;
using System.Windows.Forms;

namespace OnTopReplica.Update {
    
    class UpdateManager {

        const string UpdateManifestUrl = "http://www.klopfenstein.net/public/Uploads/ontopreplica/update.xml";

        public void CheckForUpdate() {
            //Build web request
            var request = (HttpWebRequest)HttpWebRequest.Create(UpdateManifestUrl);
            request.AllowAutoRedirect = true;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

            //Begin async request...
            request.BeginGetResponse(new AsyncCallback(EndCheckForUpdate), request);
        }

        private void EndCheckForUpdate(IAsyncResult result) {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;

            try {
                var response = request.EndGetResponse(result);
                var info = UpdateInformation.Deserialize(response.GetResponseStream());

                OnUpdateCheckSuccess(info);
            }
            catch (Exception ex) {
                OnUpdateCheckError(ex);
                return;
            }
        }

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

        /// <summary>
        /// Handles the results of an update check. Must be called from main GUI thread.
        /// </summary>
        /// <param name="information">The retrieved update information.</param>
        /// <param name="verbose">Determines if the lack of updated should be notified to the user.</param>
        public void HandleUpdateCheck(Form parent, UpdateInformation information, bool verbose) {
            if (information == null)
                return;

            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

            if (information.LatestVersion > currentVersion) {
                //New version found
                var dlg = new TaskDialog(
                    string.Format(Strings.AskUpdate, information.LatestVersion),
                    Strings.AskUpdateTitle,
                    Strings.AskUpdateContent) {
                    CustomButtons = new CustomButton[] {
                        new CustomButton(Result.OK, string.Format(Strings.AskUpdateButtonOk, information.LatestVersion)),
                        new CustomButton(Result.Cancel, Strings.AskUpdateButtonCancel)
                    },
                    UseCommandLinks = true,
                    CommonIcon = TaskDialogIcon.Information,
                    ExpandedInformation = string.Format(Strings.AskUpdateExpanded, currentVersion, information.LatestVersion)
                };
                
                if (dlg.Show(parent).CommonButton == Result.OK) {
                    Process.Start(information.DownloadPage);
                }
            }
            else if(verbose) {
                //No updates, but need to inform the user
                var dlg = new TaskDialog(Strings.InfoUpToDate, Strings.InfoUpToDateTitle) {
                    CommonButtons = TaskDialogButton.OK,
                    CommonIcon = TaskDialogIcon.Information,
                    Footer = information.LatestVersion.ToString()
                };
                dlg.Show();
            }
        }

    }

}
