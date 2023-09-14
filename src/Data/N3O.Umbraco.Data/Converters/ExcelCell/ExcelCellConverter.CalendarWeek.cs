using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Converters;

public class CalendarWeekExcelCellConverter : ExcelCellConverter<CalendarWeek>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(Column column, CalendarWeek value) {
        return value?.ToString();
    }
    
    protected override ExcelNumberFormat GetNumberFormat(Column column, CalendarWeek value) {
        return new CalendarWeekExcelNumberFormat();
    }
}
