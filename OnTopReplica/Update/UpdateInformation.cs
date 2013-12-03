using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Reflection;

namespace OnTopReplica.Update {
    
    /// <summary>
    /// Contains information about the latest OnTopReplica update available.
    /// </summary>
    public class UpdateInformation {

        Version _latestVersion;

        /// <summary>
        /// Gets the latest available version of the software.
        /// </summary>
        [XmlIgnore]
        public Version LatestVersion {
            get {
                return _latestVersion;
            }
            set {
                _latestVersion = value;
            }
        }

        [XmlElement("latestVersion")]
        public string LatestVersionInternal {
            get {
                return _latestVersion.ToString();
            }
            set {
                _latestVersion = new Version(value);
            }
        }

        /// <summary>
        /// Returns whether this update information instance represents data about
        /// a new available version.
        /// </summary>
        public bool IsNewVersion {
            get {
                var currentVersion = CurrentVersion;

                return (LatestVersion > currentVersion);
            }
        }

        /// <summary>
        /// Gets the currently installed version.
        /// </summary>
        public Version CurrentVersion {
            get {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        /// <summary>
        /// Indicates when the latest version was released.
        /// </summary>
        [XmlElement("latestVersionRelease")]
        public DateTime LatestVersionRelease { get; set; }

        /// <summary>
        /// Gets the URL of the page that allows the user to download the updated installer.
        /// </summary>
        [XmlElement("downloadPage")]
        public string DownloadPage { get; set; }

        /// <summary>
        /// Gets the URL of the installer executable.
        /// </summary>
        /// <remarks>New after version 3.3.1.</remarks>
        [XmlElement("downloadInstaller")]
        public string DownloadInstaller { get; set; }

        /// <summary>
        /// Deserializes an UpdateInformation object from a stream.
        /// </summary>
        public static UpdateInformation Deserialize(Stream source) {
            var serializer = new XmlSerializer(typeof(UpdateInformation));
            var info = serializer.Deserialize(source) as UpdateInformation;
            return info;
        }

    }

}
