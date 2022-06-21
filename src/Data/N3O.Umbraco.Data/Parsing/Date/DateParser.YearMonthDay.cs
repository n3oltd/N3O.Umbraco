using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Parsing;

public class YearMonthDayDateParser : DateParser {
    public YearMonthDayDateParser(Timezone timezone) : base(timezone) {
        AddPattern("uuuu'/'MM'/'dd");
        AddPattern("uuuu'/'M'/'dd");
        AddPattern("uuuu'/'MM'/'d");
        AddPattern("uuuu'/'M'/'d");
        AddPattern("uu'/'MM'/'dd");
        AddPattern("uu'/'M'/'dd");
        AddPattern("uu'/'MM'/'d");
        AddPattern("uu'/'M'/'d");

        AddPattern("uuuu'-'MM'-'dd");
        AddPattern("uuuu'-'MM'-'dd");
        AddPattern("uuuu'-'MM'-'d");
        AddPattern("uuuu'-'M'-'d");
        AddPattern("uu'-'MM'-'dd");
        AddPattern("uu'-'M'-'dd");
        AddPattern("uu'-'MM'-'d");
        AddPattern("uu'-'M'-'d");
    }
}
