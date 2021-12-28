using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Extensions {
    public static class BooleanExtensions {
        public static string ToTrueFalseString(this bool? @bool, ITextFormatter textFormatter) {
            if (@bool == null) {
                return null;
            } else {
                return ToTrueFalseString(@bool.Value, textFormatter);
            }
        }

        public static string ToTrueFalseString(this bool value, ITextFormatter textFormatter) {
            return ToVariantString(value, textFormatter, s => s.True, s => s.False);
        }

        public static string ToYesNoString(this bool? @bool, ITextFormatter textFormatter) {
            if (@bool == null) {
                return null;
            } else {
                return ToYesNoString(@bool.Value, textFormatter);
            }
        }

        public static string ToYesNoString(this bool value, ITextFormatter textFormatter) {
            return ToVariantString(value, textFormatter, s => s.Yes, s => s.No);
        }

        private static string ToVariantString(bool value,
                                              ITextFormatter textFormatter,
                                              Func<Strings, string> getTrue,
                                              Func<Strings, string> getFalse) {
            if (value) {
                return textFormatter.Format(getTrue);
            } else {
                return textFormatter.Format(getFalse);
            }
        }

        public class Strings : CodeStrings {
            public string False => "False";
            public string No => "No";
            public string True => "True";
            public string Yes => "Yes";
        }
    }
}
