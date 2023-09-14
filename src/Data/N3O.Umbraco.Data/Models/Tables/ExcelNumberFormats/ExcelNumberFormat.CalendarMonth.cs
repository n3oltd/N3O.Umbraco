namespace N3O.Umbraco.Data.Models;

public class CalendarMonthExcelNumberFormat : ExcelNumberFormat {
    public CalendarMonthExcelNumberFormat() {
        Pattern = "mmm yyyy";
    }
}
