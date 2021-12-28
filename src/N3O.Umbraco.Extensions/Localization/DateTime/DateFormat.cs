using N3O.Umbraco.Constants;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Localization;

public class DateFormat : NamedLookup {
    public DateFormat(string id,
                      string name,
                      string cultureCode,
                      string pattern,
                      string separator)
        : base(id, name) {
        CultureCode = cultureCode;
        Pattern = pattern;
        Separator = separator;
    }

    public string CultureCode { get; }
    public string Pattern { get; }
    public string Separator { get; }
}

public class DateFormats : StaticLookupsCollection<DateFormat> {
    public static readonly DateFormat DayMonthYearSlashes = new DateFormat("dmy_slashes", "Day/Month/Year", "en-GB", DatePatterns.DayMonthYear, DateSeparators.Slash);
    public static readonly DateFormat DayMonthYearDashses = new DateFormat("dmy_dashes", "Day-Month-Year", "en-GB", DatePatterns.DayMonthYear, DateSeparators.Dash);
    public static readonly DateFormat MonthDayYearSlashses = new DateFormat("mdy_slashes", "Month/Day/Year", "en-US", DatePatterns.MonthDayYear, DateSeparators.Slash);
    public static readonly DateFormat MonthDayYearDashses = new DateFormat("mdy_dashes", "Month-Day-Year", "en-US", DatePatterns.MonthDayYear, DateSeparators.Dash);
    public static readonly DateFormat YearMonthDaySlashes = new DateFormat("ymd_slashes", "Year/Month/Day", "en-ZA", DatePatterns.YearMonthDay, DateSeparators.Slash);
    public static readonly DateFormat YearMonthDayDashes = new DateFormat("ymd_dashes", "Year-Month-Day", "en-ZA", DatePatterns.YearMonthDay, DateSeparators.Dash);
}