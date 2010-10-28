using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica {
    /// <summary>
    /// Extension methods for IEnumerable.
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

    }
}
