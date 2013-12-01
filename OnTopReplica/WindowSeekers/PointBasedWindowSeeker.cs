using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnTopReplica.WindowSeekers {
    /// <summary>
    /// Window seeker that uses a point system to get a list of matching windows listed by optimality.
    /// </summary>
    abstract class PointBasedWindowSeeker : BaseWindowSeeker {

        IList<WindowHandle> _currentWindowList = new List<WindowHandle>();

        public override IList<WindowHandle> Windows {
            get {
                return _currentWindowList;
            }
        }

        List<Tuple<int, WindowHandle>> _sortingList = null;

        public override void Refresh() {
            _sortingList = new List<Tuple<int, WindowHandle>>();

            base.Refresh();

            //Sort and store
            _currentWindowList = (from t in _sortingList
                                  where t.Item1 > 0
                                  orderby t.Item1 descending
                                  select t.Item2).ToList();

            _sortingList = null;
        }

        protected override bool InspectWindow(WindowHandle handle) {
            int points = EvaluatePoints(handle);
            if(points >= 0){
                _sortingList.Add(new Tuple<int, WindowHandle>(points, handle));
            }

            return true;
        }

        /// <summary>
        /// Evalutes the points for a window handle.
        /// </summary>
        /// <param name="handle">Handle to the window.</param>
        /// <returns>
        /// Number of points. Higher points identify better suited windows.
        /// Windows with negative points are discarded altogether.
        /// </returns>
        protected abstract int EvaluatePoints(WindowHandle handle);

    }
}
