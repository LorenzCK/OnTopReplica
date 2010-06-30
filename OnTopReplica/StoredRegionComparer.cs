using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace OnTopReplica {

    /// <summary>
    /// Compares two StoredRegions based on their name.
    /// </summary>
    class StoredRegionComparer : IComparer {
        
        #region IComparer Members

        public int Compare(object x, object y) {
            StoredRegion a = x as StoredRegion;
            StoredRegion b = y as StoredRegion;

            if (a == null || b == null)
                return -1; //this is wrong, but anyway

            return a.Name.CompareTo(b.Name);
        }

        #endregion

    }
}
