using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace OnTopReplica.StartupOptions {
    class RectangleConverter : TypeConverter {

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
            if (value != null) {
                var sVal = value.ToString();
                return Convert(sVal);
            }
            else
                return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return destinationType == typeof(Rectangle);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
            if (value != null && destinationType == typeof(Rectangle)) {
                var sVal = value.ToString();
                return Convert(sVal);
            }
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }

        static Regex _sizeRegex = new Regex("^\\D*(?<x>\\d*)\\s*,\\s*(?<y>\\d*)\\s*,\\s*(?<width>\\d*)\\s*,\\s*(?<height>\\d*)\\D*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private Rectangle Convert(string s) {
            var match = _sizeRegex.Match(s);

            var x = match.Groups["x"];
            var y = match.Groups["y"];
            var width = match.Groups["width"];
            var height = match.Groups["height"];

            if (match.Success && x.Success && y.Success && width.Success && height.Success) {
                var xVal = int.Parse(x.Value);
                var yVal = int.Parse(y.Value);
                var widthVal = int.Parse(width.Value);
                var heightVal = int.Parse(height.Value);

                return new Rectangle(xVal, yVal, widthVal, heightVal);
            }
            else
                throw new ArgumentException("Cannot convert '" + s + "' to rectangle.");
        }

    }
}
