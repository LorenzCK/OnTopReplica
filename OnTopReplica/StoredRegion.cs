using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace OnTopReplica {

	[Serializable]
	public class StoredRegion : IXmlSerializable {

		public StoredRegion() {
		}

		public StoredRegion(Rectangle r, string n) {
			Bounds = r;
			Name = n;
		}

		public Rectangle Bounds {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public override string ToString() {
			return Name;
		}


		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema() {
			return null;
		}

		public void ReadXml(System.Xml.XmlReader reader) {
			if (reader.MoveToAttribute("name"))
				Name = reader.Value;
			else
				throw new Exception();

			reader.Read();

			XmlSerializer x = new XmlSerializer(typeof(Rectangle));
			Bounds = (Rectangle)x.Deserialize(reader);
		}

		public void WriteXml(System.Xml.XmlWriter writer) {
			writer.WriteAttributeString("name", Name);

			XmlSerializer x = new XmlSerializer(typeof(Rectangle));
			x.Serialize(writer, Bounds);
		}

		#endregion
	}

}
