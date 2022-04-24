using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Lookups {
    public class DatePattern : NamedLookup {
        public DatePattern(string id, string name, DateFormat exampleFormat) : base(id, name) {
            ExampleFormat = exampleFormat;
        }

        public DateFormat ExampleFormat { get; }
    }

    public class DatePatterns : StaticLookupsCollection<DatePattern> {
        public static readonly DatePattern DayMonthYear = new(DataConstants.DatePatterns.DayMonthYear,
                                                              "Day-Month-Year",
                                                              DateFormats.DayMonthYearSlashes);

        public static readonly DatePattern MonthDayYear = new(DataConstants.DatePatterns.MonthDayYear,
                                                              "Month-Day-Year",
                                                              DateFormats.MonthDayYearSlashses);

        public static readonly DatePattern YearMonthDay = new(DataConstants.DatePatterns.YearMonthDay,
                                                              "Year-Month-Day",
                                                              DateFormats.YearMonthDaySlashes);
    }
}