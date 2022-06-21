namespace N3O.Umbraco.Data.Parsing;

public class DayMonthYearYearMonthParser : YearMonthParser {
    public DayMonthYearYearMonthParser() {
        AddPattern("MM/yyyy");
        AddPattern("M/yyyy");
        AddPattern("MM/yy");
        AddPattern("M/yy");

        AddPattern("MM-yyyy");
        AddPattern("M-yyyy");
        AddPattern("MM-yy");
        AddPattern("M-yy");
    }
}
