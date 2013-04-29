using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OnTopReplica.Native;

namespace OnTopReplica {
    /// <summary>
    /// Facility class that attempts to locate the region occupied by plugins inside another window.
    /// </summary>
    class PluginRegionLocator {

        static PluginRegionLocator() {
            _pluginClassNames = new HashSet<string>() {
                //Opera 11 Flash plugin
                "aPluginWinClass",

                //IE 9 Flash plugin
                "MacromediaFlashPlayerActiveX",

                //Google Chrome
                "NativeWindowClass", //Flash plugin
                "Chrome_RenderWidgetHostHWND", //Tab content

                //Firefox 9 Flash plugin
                "GeckoPluginWindow",
            };
        }

        static readonly HashSet<string> _pluginClassNames;

        /// <summary>
        /// Attempts to locate a plugin region inside a window.
        /// </summary>
        /// <param name="handle">The handle to the parent window.</param>
        /// <returns>The region where a plugin window is located or null if none found.</returns>
        public Rectangle? LocatePluginRegion(WindowHandle handle) {
            if (handle == null)
                throw new ArgumentNullException();

            WindowManagerMethods.EnumChildWindows(handle.Handle, LocatingWndProc, IntPtr.Zero);

            if (_selectedHandle != null) {
                Console.Out.WriteLine("Selected {0} '{1}' (class {2})", _selectedHandle.Handle, _selectedHandle.Title, _selectedHandle.Class);

                NRectangle rect;
                WindowMethods.GetWindowRect(_selectedHandle.Handle, out rect);

                NRectangle clientRect;
                WindowMethods.GetClientRect(_selectedHandle.Handle, out clientRect);

                Console.Out.WriteLine("WindowRect: {0}", rect);

                NRectangle ownerRect;
                WindowMethods.GetWindowRect(handle.Handle, out ownerRect);

                Console.Out.WriteLine("Owner WindowRect: {0}", ownerRect);

                var ret = new Rectangle {
                    X = rect.Left - ownerRect.Left,
                    Y = rect.Top - ownerRect.Top,
                    Width = clientRect.Width,
                    Height = clientRect.Height
                };

                //Safety check (this may happen when the plugin client area is 0 pixel large)
                if (ret.Width < 0 || ret.Height < 0)
                    return null;

                Console.Out.WriteLine("Selected region: {0}", ret);

                return ret;
            }
            else {
                Console.Out.WriteLine("None found.");
                return null;
            }
        }

        WindowHandle _selectedHandle = null;

        private bool LocatingWndProc(IntPtr handle, IntPtr lParam) {
            //Skip non visible windows
            if (!WindowManagerMethods.IsWindowVisible(handle)) {
                return true;
            }

            //Class name check
            string cl = WindowMethods.GetWindowClass(handle);
            System.Diagnostics.Trace.WriteLine(string.Format("Child window, class {0}", cl));

            if (_pluginClassNames.Contains(cl)) {
                //Found plugin window, stop now
                _selectedHandle = new WindowHandle(handle);
                return false;
            }

            return true;
        }

    }
}
