using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

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
        /// Deserializes an UpdateInformation object from a stream.
        /// </summary>
        public static UpdateInformation Deserialize(Stream source) {
            var serializer = new XmlSerializer(typeof(UpdateInformation));
            var info = serializer.Deserialize(source) as UpdateInformation;
            return info;
        }

        public static string Serialize(UpdateInformation information) {
            var serializer = new XmlSerializer(typeof(UpdateInformation));
            var sb = new StringBuilder();
            using(var writer = new StringWriter(sb)){
                serializer.Serialize(writer, information);
            }
            return sb.ToString();
        }

    }

}
