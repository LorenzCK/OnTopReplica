using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OnTopReplica {
	public class CloseRequestEventArgs : EventArgs {

		public WindowHandle LastWindowHandle { get; set; }

        public Rectangle? LastRegion { get; set; }

	}
}
