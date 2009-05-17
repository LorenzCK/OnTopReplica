using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace OnTopReplica {
	class ClickThroughLabel : Label {

		protected override void WndProc(ref Message m) {
			if (m.Msg == NativeMethods.WM_NCHITTEST) {
				m.Result = new IntPtr(NativeMethods.HTTRANSPARENT);
				return;
			}

			base.WndProc(ref m);
		}

	}
}
