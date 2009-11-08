using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.Properties;

namespace OnTopReplica {
	static class WindowListHelper {

        public class WindowSelectionData {
            public WindowHandle Handle { get; set; }
            public StoredRegion Region { get; set; }
        }

		const int cMaxWindowTitleLength = 55;

		public static void PopulateMenu(Form ownerForm, WindowManager windowManager, ToolStrip menu,
                                        WindowHandle currentHandle, EventHandler clickHandler) {
            var regions = Settings.Default.SavedRegions;

			//Clear
			menu.Items.Clear();

			//"None" selection
			var nullTsi = new ToolStripMenuItem(Strings.MenuWindowsNone);
			nullTsi.Tag = null;
			nullTsi.Click += clickHandler;
			nullTsi.Checked = (currentHandle == null);
			menu.Items.Add(nullTsi);

			//Add an item for each window
			foreach (WindowHandle h in windowManager.Windows) {
                //Skip if in the same process
                if (h.Handle.Equals(ownerForm.Handle))
                    continue;

				var tsi = new ToolStripMenuItem();
				
                //Window title
                if (h.Title.Length > cMaxWindowTitleLength) {
					tsi.Text = h.Title.Substring(0, cMaxWindowTitleLength) + "...";
					tsi.ToolTipText = h.Title;
				}
				else
					tsi.Text = h.Title;

                //Icon
				if (h.Icon != null) {
					tsi.Image = h.Icon.ToBitmap();
				}

                //Check if this is the currently displayed window
				tsi.Checked = h.Equals(currentHandle);

                //Add direct click if no stored regions
                tsi.Tag = new WindowSelectionData {
                    Handle = h,
                    Region = null
                };
                tsi.Click += clickHandler;

                if (regions != null && regions.Count > 0) {
                    //Add subitem for no region
                    var nullRegionItem = new ToolStripMenuItem(Strings.MenuWindowsWholeRegion);
                    nullRegionItem.Tag = new WindowSelectionData {
                        Handle = h,
                        Region = null
                    };
                    nullRegionItem.Image = Resources.regions;
                    nullRegionItem.Click += clickHandler;

                    tsi.DropDownItems.Add(nullRegionItem);
                    
                    foreach (StoredRegion region in regions) {
                        var regionItem = new ToolStripMenuItem(region.Name);
                        regionItem.Tag = new WindowSelectionData {
                            Handle = h,
                            Region = region
                        };
                        regionItem.Click += clickHandler;

                        tsi.DropDownItems.Add(regionItem);
                    }
                }

				menu.Items.Add(tsi);
			}
		}

	}
}
