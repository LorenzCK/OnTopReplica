using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica {
    /// <summary>
    /// Extension methods for IEnumerable.
    /// Poor man's LINQ.
    /// </summary>
    static class EnumerableExtensions {

        /// <summary>
        /// Gets the first element of an enumeration of a default value.
        /// </summary>
        public static T FirstOrDefault<T>(this IEnumerable<T> collection) {
            if (collection == null)
                throw new ArgumentNullException();

            using (var enumerator = collection.GetEnumerator()) {
                if (!enumerator.MoveNext())
                    return default(T);
                else
                    return enumerator.Current;
            }
        }

        /// <summary>
        /// Checks whether an enumeration contains a value.
        /// </summary>
        public static bool Contains<T>(this IEnumerable<T> collection, T value) {
            foreach (var v in collection)
                if (v.Equals(value))
                    return true;

            return false;
        }

    }
}
