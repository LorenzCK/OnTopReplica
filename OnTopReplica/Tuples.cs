using System;
using System.Collections.Generic;
using System.Text;

namespace OnTopReplica {
    
    public class Tuple<T1, T2> {

        public Tuple(T1 item1, T2 item2) {
            Item1 = item1;
            Item2 = item2;
        }

        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }

    }

}
