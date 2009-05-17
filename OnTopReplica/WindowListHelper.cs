using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica {
	static class WindowListHelper {

		const int cMaxWindowTitleLength = 60;

		public static void PopulateMenu(WindowManager windowManager, ToolStrip menu, WindowHandle currentHandle, EventHandler clickHandler){
			//Clear
			menu.Items.Clear();

			//"None" selection
			var nullTsi = new ToolStripMenuItem(Strings.MenuWindowsNone);
			nullTsi.Tag = null;
			nullTsi.Click += clickHandler;
			nullTsi.Checked = (currentHandle == null);
			menu.Items.Add(nullTsi);

			//Add an item for each window, the tag stores the window index
			int i = 0;
			foreach (WindowHandle h in windowManager.Windows) {
				var tsi = new ToolStripMenuItem();
				if (h.Title.Length > cMaxWindowTitleLength) {
					tsi.Text = h.Title.Substring(0, cMaxWindowTitleLength) + "...";
					tsi.ToolTipText = h.Title;
				}
				else
					tsi.Text = h.Title;
				tsi.Click += clickHandler;
				tsi.Tag = i++;
				if (h.Icon != null) {
					tsi.Image = h.Icon.ToBitmap();
				}
				tsi.Checked = h.Equals(currentHandle);

				menu.Items.Add(tsi);
			}
		}

	}
}
