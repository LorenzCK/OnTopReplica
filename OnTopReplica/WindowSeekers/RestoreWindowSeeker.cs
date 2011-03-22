using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;

namespace OnTopReplica.WindowSeekers {
    
    /// <summary>
    /// Window seeker that attempts to locate a window to restore (by class, title and ID).
    /// </summary>
    class RestoreWindowSeeker : BaseWindowSeeker {

        public RestoreWindowSeeker(IntPtr handle, string title, string className){
            Handle = handle;
            Title = title;
            Class = className;
        }

        public IntPtr Handle { get; private set; }

        public string Title { get; private set; }

        public string Class { get; private set; }

        bool _mustBeOrdered = true;

        public override void Refresh() {
            //Whenever the window list is refreshed, the list must be reordered
            _mustBeOrdered = true;
            _points = new Dictionary<long, int>();

            base.Refresh();
        }

        Dictionary<long, int> _points = new Dictionary<long, int>();

        protected override bool InspectWindow(IntPtr hwnd, string title, ref bool terminate) {
            if (!WindowManagerMethods.IsTopLevel(hwnd))
                return false;

            int points = 0;

            //Class exact match
            if (!string.IsNullOrEmpty(Class)) {
                string wndClass = WindowMethods.GetWindowClass(hwnd);
                if (Class.Equals(wndClass, StringComparison.InvariantCulture)) {
                    points += 10;
                }
            }

            //Title match (may not be exact, but let's try)
            if (!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(title)) {
                if (title.StartsWith(Title, StringComparison.InvariantCultureIgnoreCase)) {
                    points += 10;
                }
            }

            //Handle match (will probably not work, but anyhow)
            if (Handle != IntPtr.Zero) {
                if (Handle == hwnd) {
                    points += 5;
                }
            }

            //Store handle if it matches
            if (points > 0) {
                _points.Add(hwnd.ToInt64(), points);
                return true;
            }
            else
                return false;
        }

        public override IList<WindowHandle> Windows {
            get {
                if (_mustBeOrdered) {
                    WindowHandle[] arr = new WindowHandle[base.Windows.Count];
                    base.Windows.CopyTo(arr, 0);
                    Array.Sort<WindowHandle>(arr, new PointComparer(_points));
                    
                    //Store ordered array
                    base.Windows = arr;

                    _mustBeOrdered = false;
                }

                return base.Windows;
            }
        }

        private class PointComparer : IComparer<WindowHandle> {

            public PointComparer(IDictionary<long, int> pointDict) {
                _pointDict = pointDict;
            }

            IDictionary<long, int> _pointDict;

            public int Compare(WindowHandle x, WindowHandle y) {
                int px = 0;
                _pointDict.TryGetValue(x.Handle.ToInt64(), out px);
                int py = 0;
                _pointDict.TryGetValue(y.Handle.ToInt64(), out py);

                return py.CompareTo(px); //inverse comparison (from max points to min)
            }

        }

    }

}
