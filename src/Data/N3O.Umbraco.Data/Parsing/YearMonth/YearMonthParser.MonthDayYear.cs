namespace N3O.Umbraco.Data.Parsing;

public class MonthDayYearYearMonthParser : YearMonthParser {
    public MonthDayYearYearMonthParser() {
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
