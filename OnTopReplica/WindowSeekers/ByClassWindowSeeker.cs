using System;
using System.Collections.Generic;
using System.Text;
using OnTopReplica.Native;

namespace OnTopReplica.WindowSeekers {
    /// <summary>
    /// Seeks a single window by matching its window class.
    /// </summary>
    /// <remarks>
    /// Class matching is exact and case-sensititve.
    /// </remarks>
    class ByClassWindowSeeker : BaseWindowSeeker {

        public ByClassWindowSeeker(string className) {
            if (className == null)
                throw new ArgumentNullException();

            ClassName = className;
        }

        public string ClassName { get; private set; }

        protected override bool InspectWindow(IntPtr hwnd, string title, ref bool terminate) {
            var wndClass = WindowMethods.GetWindowClass(hwnd);

            if (ClassName.Equals(wndClass, StringComparison.CurrentCulture)) {
                return true;
            }

            return false;
        }

    }
}
