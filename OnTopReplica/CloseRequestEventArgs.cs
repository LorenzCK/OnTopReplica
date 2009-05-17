using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica {
	public class CloseRequestEventArgs : EventArgs {

		public CloseRequestEventArgs(WindowHandle lastHandle) {
			CurrentWindowHandle = lastHandle;
		}

		public WindowHandle CurrentWindowHandle { get; set; }

	}
}
