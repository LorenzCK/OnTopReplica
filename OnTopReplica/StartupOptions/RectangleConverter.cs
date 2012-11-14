using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace OnTopReplica.StartupOptions {

    class RectangleConverter : FourValueTypeConverter<Rectangle> {

        protected override Rectangle CreateValue(int v1, int v2, int v3, int v4) {
            return new Rectangle {
                X = v1,
                Y = v2,
                Width = v3,
                Height = v4
            };
        }

    }

}
