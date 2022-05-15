using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Parsing {
    public class DayMonthYearDateParser : DateParser {
        public DayMonthYearDateParser(Timezone timezone) : base(timezone) {
            AddPattern("dd'/'MM'/'uuuu");
            AddPattern("d'/'MM'/'uuuu");
            AddPattern("dd'/'M'/'uuuu");
            AddPattern("d'/'M'/'uuuu");
            AddPattern("dd'/'MM'/'uu");
            AddPattern("d'/'MM'/'uu");
            AddPattern("dd'/'M'/'uu");
            AddPattern("d'/'M'/'uu");

            AddPattern("dd'-'MM'-'uuuu");
            AddPattern("d'-'MM'-'uuuu");
            AddPattern("dd'-'M'-'uuuu");
            AddPattern("d'-'M'-'uuuu");
            AddPattern("dd'-'MM'-'uu");
            AddPattern("d'-'MM'-'uu");
            AddPattern("dd'-'M'-'uu");
            AddPattern("d'-'M'-'uu");
        }
    }
}