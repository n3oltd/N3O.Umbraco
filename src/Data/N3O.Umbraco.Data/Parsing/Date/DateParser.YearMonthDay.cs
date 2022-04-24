using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Parsing {
    public class YearMonthDayDateParser : DateParser {
        public YearMonthDayDateParser(Timezone timezone) : base(timezone) {
            AddPattern("yyyy/MM/dd");
            AddPattern("yyyy/M/dd");
            AddPattern("yyyy/MM/d");
            AddPattern("yyyy/M/d");
            AddPattern("yy/MM/dd");
            AddPattern("yy/M/dd");
            AddPattern("yy/MM/d");
            AddPattern("yy/M/d");

            AddPattern("yyyy-MM-dd");
            AddPattern("yyyy-MM-dd");
            AddPattern("yyyy-MM-d");
            AddPattern("yyyy-M-d");
            AddPattern("yy-MM-dd");
            AddPattern("yy-M-dd");
            AddPattern("yy-MM-d");
            AddPattern("yy-M-d");
        }
    }
}