using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Parsing;

public class MonthDayYearDateParser : DateParser {
    public MonthDayYearDateParser(Timezone timezone) : base(timezone) {
        AddPattern("MM'/'dd'/'uuuu");
        AddPattern("M'/'dd'/'uuuu");
        AddPattern("MM'/'d'/'uuuu");
        AddPattern("M'/'d'/'uuuu");
        AddPattern("MM'/'dd'/'uu");
        AddPattern("M'/'dd'/'uu");
        AddPattern("MM'/'d'/'uu");
        AddPattern("M'/'d'/'uu");

        AddPattern("MM'-'dd'-'uuuu");
        AddPattern("MM'-'dd'-'uuuu");
        AddPattern("MM'-'d'-'uuuu");
        AddPattern("M'-'d'-'uuuu");
        AddPattern("MM'-'dd'-'uu");
        AddPattern("M'-'dd'-'uu");
        AddPattern("MM'-'d'-'uu");
        AddPattern("M'-'d'-'uu");
    }
}
