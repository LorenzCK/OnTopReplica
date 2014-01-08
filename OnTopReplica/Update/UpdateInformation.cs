using System;
using System.Globalization;
using System.Reflection;

namespace OnTopReplica.Update {
    
    /// <summary>
    /// Contains information about the latest OnTopReplica update available.
    /// </summary>
    public class UpdateInformation {

        /// <summary>
        /// Construct update information from raw data.
        /// </summary>
        /// <param name="latestVersion">Latest available version.</param>
        /// <param name="downloadLink">Direct link to the download page (has URL form).</param>
        /// <param name="publicationDate">Publication date of latest version, in standard RTF/RSS format.</param>
        public UpdateInformation(Version latestVersion, string downloadLink, string publicationDate) {
            LatestVersion = latestVersion;
            DownloadPage = downloadLink;

            //RSS date formatted as in: <pubDate>Thu, 29 Nov 2012 12:55:04 GMT</pubDate>
            DateTime parsedPublicationDate;
            if (DateTime.TryParseExact(publicationDate, "R", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out parsedPublicationDate)) {
                LatestVersionRelease = parsedPublicationDate;
            }
        }

        /// <summary>
        /// Gets or sets the latest available version of the software.
        /// </summary>
        public Version LatestVersion { get; private set; }

        /// <summary>
        /// Returns whether this update information instance represents data about
        /// a new available version.
        /// </summary>
        public bool IsNewVersionAvailable {
            get {
                return (LatestVersion > CurrentVersion);
            }
        }

        private Version _currentVersion = null;

        /// <summary>
        /// Gets the currently installed version.
        /// </summary>
        public Version CurrentVersion {
            get {
                if (_currentVersion == null) {
                    _currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                }
                
                return _currentVersion;
            }
        }

        /// <summary>
        /// Indicates when the latest version was released.
        /// </summary>
        public DateTime LatestVersionRelease { get; private set; }

        /// <summary>
        /// Gets the URL of the page that allows the user to download the updated installer.
        /// </summary>
        public string DownloadPage { get; private set; }

    }

}
