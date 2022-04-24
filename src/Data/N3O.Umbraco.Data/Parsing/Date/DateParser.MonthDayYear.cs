using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Parsing {
    public class MonthDayYearDateParser : DateParser {
        public MonthDayYearDateParser(Timezone timezone) : base(timezone) {
            AddPattern("MM/dd/yyyy");
            AddPattern("M/dd/yyyy");
            AddPattern("MM/d/yyyy");
            AddPattern("M/d/yyyy");
            AddPattern("MM/dd/yy");
            AddPattern("M/dd/yy");
            AddPattern("MM/d/yy");
            AddPattern("M/d/yy");

            AddPattern("MM-dd-yyyy");
            AddPattern("MM-dd-yyyy");
            AddPattern("MM-d-yyyy");
            AddPattern("M-d-yyyy");
            AddPattern("MM-dd-yy");
            AddPattern("M-dd-yy");
            AddPattern("MM-d-yy");
            AddPattern("M-d-yy");
        }
    }
}