using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Converters;

public class CalendarMonthExcelCellConverter : ExcelCellConverter<CalendarMonth>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(Column column, CalendarMonth value) {
        return value?.ToString();
    }
    
    protected override ExcelNumberFormat GetNumberFormat(Column column, CalendarMonth value) {
        return new CalendarMonthExcelNumberFormat();
    }
}
