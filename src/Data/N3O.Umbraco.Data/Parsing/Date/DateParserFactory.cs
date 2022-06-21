using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Parsing;

public class DateParserFactory : IDateParserFactory {
    public IDateParser Create(DatePattern pattern, Timezone timezone) {
        if (pattern == DatePatterns.DayMonthYear) {
            return new DayMonthYearDateParser(timezone);
        } else if (pattern == DatePatterns.MonthDayYear) {
            return new MonthDayYearDateParser(timezone);
        } else if (pattern == DatePatterns.YearMonthDay) {
            return new YearMonthDayDateParser(timezone);
        } else {
            throw UnrecognisedValueException.For(pattern);
        }
    }
}
