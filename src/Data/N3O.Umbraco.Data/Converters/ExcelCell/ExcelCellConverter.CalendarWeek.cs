using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class CalendarWeekExcelCellConverter : ExcelCellConverter<CalendarWeek>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(CalendarWeek value, IFormatter formatter) {
        return value?.ToString();
    }
    
    protected override ExcelNumberFormat GetNumberFormat(CalendarWeek value, IFormatter formatter) {
        return new CalendarWeekExcelNumberFormat();
    }
}
