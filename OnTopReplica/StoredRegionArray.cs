using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace OnTopReplica {

    /// <summary>
    /// Strongly styped array of StoredRegion elements.
    /// </summary>
    /// <remarks>
    /// Handles XML serialization.
    /// </remarks>
	public class StoredRegionArray : List<StoredRegion>, IXmlSerializable {
		
        #region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema() {
			return null;
		}

		public void ReadXml(System.Xml.XmlReader reader) {
			this.Clear();

            var doc = XDocument.Load(reader);
            foreach (var xmlRegion in doc.Descendants("StoredRegion")) {
                System.Diagnostics.Debug.WriteLine(string.Format("Found region '{0}'.", xmlRegion.Attribute("name")));

                StoredRegion parsedRegion = ParseStoredRegion(xmlRegion);
                if (parsedRegion != null) {
                    this.Add(parsedRegion);
                }
            }
		}

        private StoredRegion ParseStoredRegion(XElement xmlRegion) {
            var xName = xmlRegion.Attribute("name");
            if (xName == null || string.IsNullOrWhiteSpace(xName.Value)) {
                System.Diagnostics.Debug.Fail("Parsed stored region has no name attribute.");
                return null;
            }

            ThumbnailRegion region = ParseRegion(xmlRegion);
            if (region == null) {
                System.Diagnostics.Debug.Fail("Parsed stored region has no valid region.");
                return null;
            }

            return new StoredRegion(region, xName.Value);
        }

        private ThumbnailRegion ParseRegion(XElement xmlRegion) {
            var xRectangle = xmlRegion.Element("Rectangle");
            if (xRectangle != null) {
                System.Drawing.Rectangle rectangle = ParseRectangle(xRectangle);
                return new ThumbnailRegion(rectangle);
            }

            var xPadding = xmlRegion.Element("Padding");
            if (xPadding != null) {
                System.Windows.Forms.Padding padding = ParsePadding(xPadding);
                return new ThumbnailRegion(padding);
            }

            return null;
        }

        private System.Windows.Forms.Padding ParsePadding(XElement xPadding) {
            var p = new System.Windows.Forms.Padding();
            try {
                p.Left = Int32.Parse(xPadding.Element("Left").Value);
                p.Top = Int32.Parse(xPadding.Element("Top").Value);
                p.Right = Int32.Parse(xPadding.Element("Right").Value);
                p.Bottom = Int32.Parse(xPadding.Element("Bottom").Value);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.Fail("Failure while parsing padding data.", ex.ToString());
            }
            return p;
        }

        private System.Drawing.Rectangle ParseRectangle(XElement xRectangle) {
            var r = new System.Drawing.Rectangle();
            try {
                r.X = Int32.Parse(xRectangle.Element("X").Value);
                r.Y = Int32.Parse(xRectangle.Element("Y").Value);
                r.Width = Int32.Parse(xRectangle.Element("Width").Value);
                r.Height = Int32.Parse(xRectangle.Element("Height").Value);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.Fail("Failure while parsing rectangle data.", ex.ToString());
            }
            return r;
        }

		public void WriteXml(System.Xml.XmlWriter writer) {
            foreach (var region in this) {
                WriteRegion(writer, region);
            }
		}

        private void WriteRegion(XmlWriter writer, StoredRegion region) {
            writer.WriteStartElement("StoredRegion");
            writer.WriteAttributeString("name", region.Name);

            if (region.Region.Relative) {
                WriteRelativeRegion(writer, region);
            }
            else {
                WriteAbsoluteRegion(writer, region);
            }

            writer.WriteEndElement();
        }

        private void WriteAbsoluteRegion(XmlWriter writer, StoredRegion region) {
            writer.WriteStartElement("Rectangle");
            
            var bounds = region.Region.Bounds;
            writer.WriteElementString("X", bounds.X.ToString());
            writer.WriteElementString("Y", bounds.Y.ToString());
            writer.WriteElementString("Width", bounds.Width.ToString());
            writer.WriteElementString("Height", bounds.Height.ToString());

            writer.WriteEndElement();
        }

        private void WriteRelativeRegion(XmlWriter writer, StoredRegion region) {
            writer.WriteStartElement("Padding");

            var padding = region.Region.BoundsAsPadding;
            writer.WriteElementString("Left", padding.Left.ToString());
            writer.WriteElementString("Top", padding.Top.ToString());
            writer.WriteElementString("Right", padding.Right.ToString());
            writer.WriteElementString("Bottom", padding.Bottom.ToString());

            writer.WriteEndElement();
        }

		#endregion

	}

}
