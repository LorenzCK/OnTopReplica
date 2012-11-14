using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace OnTopReplica.StartupOptions {
    abstract class FourValueTypeConverter<T> : TypeConverter {

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
            return destinationType == typeof(T);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
            if (value != null && destinationType == typeof(T)) {
                var sVal = value.ToString();
                return Convert(sVal);
            }
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }

        static Regex _sizeRegex = new Regex("^\\D*(?<one>\\d*)\\s*,\\s*(?<two>\\d*)\\s*,\\s*(?<three>\\d*)\\s*,\\s*(?<four>\\d*)\\D*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private T Convert(string s) {
            var match = _sizeRegex.Match(s);

            var v1 = match.Groups["one"];
            var v2 = match.Groups["two"];
            var v3 = match.Groups["three"];
            var v4 = match.Groups["four"];

            if (match.Success && v1.Success && v2.Success && v3.Success && v4.Success) {
                int v1v, v2v, v3v, v4v;
                bool v1b, v2b, v3b, v4b;
                v1b = Int32.TryParse(v1.Value, out v1v);
                v2b = Int32.TryParse(v2.Value, out v2v);
                v3b = Int32.TryParse(v3.Value, out v3v);
                v4b = Int32.TryParse(v4.Value, out v4v);

                if (v1b && v2b && v3b && v4b) {
                    return CreateValue(v1v, v2v, v3v, v4v);
                }
                else {
                    throw new ArgumentException("Argument '" + s + "' contains a non numeric value.");
                }
            }
            else {
                throw new ArgumentException("Argument '" + s + "' is in the wrong format.");
            }
        }

        protected abstract T CreateValue(int v1, int v2, int v3, int v4);

    }
}
