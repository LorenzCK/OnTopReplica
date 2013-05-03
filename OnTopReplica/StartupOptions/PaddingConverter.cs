using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace OnTopReplica.StartupOptions {
    class PaddingConverter : FourValueTypeConverter<Padding> {

        protected override Padding CreateValue(int v1, int v2, int v3, int v4) {
            return new Padding {
                Left = v1,
                Top = v2,
                Right = v3,
                Bottom = v4
            };
        }

    }
}
