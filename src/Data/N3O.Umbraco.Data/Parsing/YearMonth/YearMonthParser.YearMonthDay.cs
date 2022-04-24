namespace N3O.Umbraco.Data.Parsing {
    public class YearMonthDayYearMonthParser : YearMonthParser {
        public YearMonthDayYearMonthParser() {
            AddPattern("yyyy/MM");
            AddPattern("yyyy/M");
            AddPattern("yy/MM");
            AddPattern("yy/M");

            AddPattern("yyyy-MM");
            AddPattern("yyyy-M");
            AddPattern("yy-MM");
            AddPattern("yy-M");
        }
    }
}