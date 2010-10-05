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

		const int MaxWindowTitleLength = 55;

		public static void PopulateMenu(Form ownerForm, WindowManager windowManager, ToolStrip menu,
                                        WindowHandle currentHandle, EventHandler clickHandler) {
            var regions = GetRegions();

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
                if (h.Title.Length > MaxWindowTitleLength) {
					tsi.Text = h.Title.Substring(0, MaxWindowTitleLength) + "...";
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

                PopulateRegions(tsi, h, clickHandler, regions);

				menu.Items.Add(tsi);
			}

		}

        private static void PopulateRegions(ToolStripMenuItem tsi, WindowHandle handle,
                                            EventHandler clickHandler, IEnumerable<StoredRegion> regions) {

            if (regions != null) {
                //Add subitem for no region
                var nullRegionItem = new ToolStripMenuItem(Strings.MenuWindowsWholeRegion);
                nullRegionItem.Tag = new WindowSelectionData {
                    Handle = handle,
                    Region = null
                };
                nullRegionItem.Image = Resources.regions;
                nullRegionItem.Click += clickHandler;
                tsi.DropDownItems.Add(nullRegionItem);

                foreach (StoredRegion region in regions) {
                    var regionItem = new ToolStripMenuItem(region.Name);
                    regionItem.Tag = new WindowSelectionData {
                        Handle = handle,
                        Region = region
                    };
                    regionItem.Click += clickHandler;

                    tsi.DropDownItems.Add(regionItem);
                }
            }
        }

        private static IEnumerable<StoredRegion> GetRegions() {
            if (Settings.Default.SavedRegions == null || Settings.Default.SavedRegions.Count == 0)
                return null;

            StoredRegion[] regions = new StoredRegion[Settings.Default.SavedRegions.Count];
            Settings.Default.SavedRegions.CopyTo(regions);

            Array.Sort<StoredRegion>(regions, new Comparison<StoredRegion>((a, b) => {
                return a.Name.CompareTo(b.Name);
            }));

            return regions;
        }

	}
}
