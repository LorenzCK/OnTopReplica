using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;

namespace OnTopReplica {

	public class StoredRegionArray : ArrayList, IXmlSerializable {
		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema() {
			return null;
		}

		public void ReadXml(System.Xml.XmlReader reader) {
			this.Clear();
			XmlSerializer x = new XmlSerializer(typeof(StoredRegion));
			while (reader.ReadToFollowing("StoredRegion")) {
				object o = x.Deserialize(reader);

				if (o is StoredRegion)
					this.Add(o);
			}
		}

		public void WriteXml(System.Xml.XmlWriter writer) {
			XmlSerializer x = new XmlSerializer(typeof(StoredRegion));
			foreach (StoredRegion sr in this) {
				x.Serialize(writer, sr);
			}
		}

		#endregion
	}

}
