using N3O.Umbraco.Constants;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Models {
    public class DateExcelNumberFormat : ExcelNumberFormat {
        public DateExcelNumberFormat(DateFormat dateFormat) {
            var separator = dateFormat.Separator;

            switch (dateFormat.Pattern) {
                case DatePatterns.DayMonthYear:
                    Pattern = $"dd{separator}mm{separator}yyyy";
                    break;

                case DatePatterns.MonthDayYear:
                    Pattern = $"mm{separator}dd{separator}yyyy";
                    break;

                case DatePatterns.YearMonthDay:
                    Pattern = $"yyyy{separator}mm{separator}dd";
                    break;

                default:
                    throw UnrecognisedValueException.For(dateFormat.Pattern);
            }
        }
    }
}