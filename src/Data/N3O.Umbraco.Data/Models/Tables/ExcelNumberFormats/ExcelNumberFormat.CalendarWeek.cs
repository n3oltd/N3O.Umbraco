namespace N3O.Umbraco.Data.Models;

public class CalendarWeekExcelNumberFormat : ExcelNumberFormat {
    public CalendarWeekExcelNumberFormat() {
        Pattern = "@";
    }
}
