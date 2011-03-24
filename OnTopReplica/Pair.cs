using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica {

    /// <summary>
    /// Simple tuple with two values.
    /// </summary>
    struct Pair<T1, T2> {
        public T1 Item1;
        public T2 Item2;

        public Pair(T1 value1, T2 value2) {
            Item1 = value1;
            Item2 = value2;
        }
    }

}
