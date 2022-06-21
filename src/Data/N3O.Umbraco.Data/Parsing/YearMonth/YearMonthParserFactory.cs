using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Exceptions;

namespace N3O.Umbraco.Data.Parsing;

public class YearMonthParserFactory : IYearMonthParserFactory {
    public IYearMonthParser Create(DatePattern pattern) {
        if (pattern == DatePatterns.DayMonthYear) {
            return new DayMonthYearYearMonthParser();
        } else if (pattern == DatePatterns.MonthDayYear) {
            return new MonthDayYearYearMonthParser();
        } else if (pattern == DatePatterns.YearMonthDay) {
            return new YearMonthDayYearMonthParser();
        } else {
            throw UnrecognisedValueException.For(pattern);
        }
    }
}
