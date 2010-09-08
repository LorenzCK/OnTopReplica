using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica {

    /// <summary>
    /// Poor man's LINQ.
    /// </summary>
    static class EnumerationExtensions {

        public static bool Contains<T>(IEnumerable<T> collection, T value){
            foreach (var v in collection)
                if (v.Equals(value))
                    return true;

            return false;
        }

    }

}
