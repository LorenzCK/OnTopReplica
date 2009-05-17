using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OnTopReplica {
	public class CloneClickEventArgs : EventArgs {

		public Point ClientClickLocation { get; set; }

		public bool IsDoubleClick { get; set; }

		public CloneClickEventArgs(Point location) {
			ClientClickLocation = location;
			IsDoubleClick = false;
		}

		public CloneClickEventArgs(Point location, bool doubleClick) {
			ClientClickLocation = location;
			IsDoubleClick = doubleClick;
		}

	}
}
