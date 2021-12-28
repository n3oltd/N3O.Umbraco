using System;
using System.Globalization;

namespace N3O.Umbraco.Localization {
    public class AbbreviatedNumberFormatter : ICustomFormatter, IFormatProvider {
        private readonly NumberFormatInfo _numberFormatInfo;

        public AbbreviatedNumberFormatter(NumberFormatInfo numberFormatInfo) {
            _numberFormatInfo = numberFormatInfo;
        }
        
        public object GetFormat(Type formatType) {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
        
        public string Format(string format, object arg, IFormatProvider formatProvider) {
            var value = Convert.ToDecimal(arg);
            string suffix = null;
            
            if (value > 1_000_000) {
                value = Math.Round(value / 1_000_000m, 2);
                suffix = "M";
            } else if (value > 1_000) {
                value = Math.Round(value / 1_000, 2);
                suffix = "K";
            }

            var str = value.ToString("c", _numberFormatInfo);

            return $"{str}{suffix}";
        }

        public static IFormatProvider Provider(NumberFormatInfo numberFormatInfo) {
            return new AbbreviatedNumberFormatter(numberFormatInfo);
        }
    }
}