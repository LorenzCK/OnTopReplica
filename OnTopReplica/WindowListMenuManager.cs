using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OnTopReplica.WindowSeekers;
using OnTopReplica.Properties;

namespace OnTopReplica {
    /// <summary>
    /// Manages the window list displayed when allowing the user to select a window to clone.
    /// </summary>
    class WindowListMenuManager {

        const int MaxWindowTitleLength = 55;

        readonly MainForm _owner;
        readonly ContextMenuStrip _windowsMenu;

        public WindowListMenuManager(MainForm owner, ContextMenuStrip windowsMenu) {
            _owner = owner;
            _windowsMenu = windowsMenu;

            WindowSeeker = new TaskWindowSeeker() {
                OwnerHandle = owner.Handle,
                SkipNotVisibleWindows = true
            };

            //Bind events
            windowsMenu.Opening += new System.ComponentModel.CancelEventHandler(WindowsMenu_opening);
        }

        void WindowsMenu_opening(object sender, System.ComponentModel.CancelEventArgs e) {
            WindowSeeker.Refresh();
            PopulateMenu(_owner.CurrentThumbnailWindowHandle);
        }

        /// <summary>
        /// Populates the menu with windows from the window seeker instance.
        /// </summary>
        /// <param name="currentSelection">Handle of the currently selected window or null if none selected.</param>
        private void PopulateMenu(WindowHandle currentSelection) {
            var regions = GetStoredRegions();

            _windowsMenu.Items.Clear();

            //"None" selection
            var nullTsi = new ToolStripMenuItem(Strings.MenuWindowsNone);
            nullTsi.Tag = null;
            nullTsi.Click += MenuWindowClickHandler;
            nullTsi.Checked = (currentSelection == null);
            _windowsMenu.Items.Add(nullTsi);

            //Add an item for each window
            foreach (WindowHandle h in WindowSeeker.Windows) {
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
                tsi.Checked = h.Equals(currentSelection);

                //Click handler
                tsi.Tag = h;
                tsi.Click += MenuWindowClickHandler;

                PopulateRegionsDropdown(tsi, h, regions);

                _windowsMenu.Items.Add(tsi);
            }
        }

        private void PopulateRegionsDropdown(ToolStripMenuItem parent, WindowHandle parentHandle, StoredRegion[] regions) {
            parent.DropDownItems.Clear();
            
            //No region
            var nullRegionItem = new ToolStripMenuItem(Strings.MenuWindowsWholeRegion);
            nullRegionItem.Tag = parentHandle;
            nullRegionItem.Image = Resources.regions;
            nullRegionItem.Click += MenuWindowClickHandler;
            parent.DropDownItems.Add(nullRegionItem);

            //Video detector
            var detectorItem = new ToolStripMenuItem("Autodetect plugin");
            detectorItem.Tag = parentHandle;
            detectorItem.Click += MenuVideoCropperClickHandler;
            parent.DropDownItems.Add(detectorItem);

            //Regions (if any)
            if (regions == null || regions.Length == 0)
                return;

            parent.DropDownItems.Add(new ToolStripSeparator());

            foreach (StoredRegion region in regions) {
                var regionItem = new ToolStripMenuItem(region.Name);
                regionItem.Tag = new Tuple<WindowHandle, StoredRegion>(parentHandle, region);
                regionItem.Click += MenuRegionWindowClickHandler;

                parent.DropDownItems.Add(regionItem);
            }
        }

        private void MenuWindowClickHandler(object sender, EventArgs args) {
            CommonClickHandler();

            var tsi = (ToolStripMenuItem)sender;
            if (tsi.Tag == null) {
                _owner.UnsetThumbnail();
            }
            else {
                var handle = (WindowHandle)tsi.Tag;
                _owner.SetThumbnail(handle, null);
            }
        }

        private void MenuRegionWindowClickHandler(object sender, EventArgs args) {
            CommonClickHandler();

            var tsi = (ToolStripMenuItem)sender;
            var tuple = (Tuple<WindowHandle, StoredRegion>)tsi.Tag;
            _owner.SetThumbnail(tuple.Item1,
                (tuple.Item2 != null) ? (System.Drawing.Rectangle?)tuple.Item2.Bounds : null);
        }

        PluginRegionLocator _pluginRegionLocator = null;

        private void MenuVideoCropperClickHandler(object sender, EventArgs args){
            CommonClickHandler();

            var tsi = (ToolStripMenuItem)sender;
            var handle = (WindowHandle)tsi.Tag;

            if (_pluginRegionLocator == null)
                _pluginRegionLocator = new PluginRegionLocator();

            var detectedRegion = _pluginRegionLocator.LocatePluginRegion(handle);
            _owner.SetThumbnail(handle, detectedRegion);
        }

        private void CommonClickHandler() {
            _windowsMenu.Close();
            foreach (ContextMenuStrip menu in _parentMenus)
                menu.Close();
        }

        /// <summary>
        /// Gets an array of stored regions.
        /// </summary>
        private StoredRegion[] GetStoredRegions() {
            if (Settings.Default.SavedRegions == null || Settings.Default.SavedRegions.Count == 0)
                return null;

            StoredRegion[] ret = new StoredRegion[Settings.Default.SavedRegions.Count];
            Settings.Default.SavedRegions.CopyTo(ret);

            Array.Sort<StoredRegion>(ret, (a, b) => {
                return a.Name.CompareTo(b.Name);
            });

            return ret;
        }

        /// <summary>
        /// Gets or sets the window seeker instance used to list windows.
        /// </summary>
        public BaseWindowSeeker WindowSeeker { get; set; }

        ContextMenuStrip[] _parentMenus = new ContextMenuStrip[0];

        /// <summary>
        /// Gets the parent menus which are bound to the context menu handled by this manager.
        /// </summary>
        public ContextMenuStrip[] ParentMenus {
            get {
                return (ContextMenuStrip[])_parentMenus.Clone();
            }
            set {
                if(value == null)
                    _parentMenus = new ContextMenuStrip[0];
                else
                    _parentMenus = (ContextMenuStrip[])value.Clone();
            }
        }

    }
}
