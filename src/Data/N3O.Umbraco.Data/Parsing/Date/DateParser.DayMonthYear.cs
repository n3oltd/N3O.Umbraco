using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Parsing {
    public class DayMonthYearDateParser : DateParser {
        public DayMonthYearDateParser(Timezone timezone) : base(timezone) {
            AddPattern("dd/MM/yyyy");
            AddPattern("d/MM/yyyy");
            AddPattern("dd/M/yyyy");
            AddPattern("d/M/yyyy");
            AddPattern("dd/MM/yy");
            AddPattern("d/MM/yy");
            AddPattern("dd/M/yy");
            AddPattern("d/M/yy");

            AddPattern("dd-MM-yyyy");
            AddPattern("d-MM-yyyy");
            AddPattern("dd-M-yyyy");
            AddPattern("d-M-yyyy");
            AddPattern("dd-MM-yy");
            AddPattern("d-MM-yy");
            AddPattern("dd-M-yy");
            AddPattern("d-M-yy");
        }
    }
}