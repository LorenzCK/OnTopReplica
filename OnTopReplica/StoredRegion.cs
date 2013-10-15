using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace OnTopReplica {

	public class StoredRegion {

        public StoredRegion(ThumbnailRegion r, string name) {
            Region = r;
            Name = name;
        }

        public ThumbnailRegion Region {
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

	}

}
