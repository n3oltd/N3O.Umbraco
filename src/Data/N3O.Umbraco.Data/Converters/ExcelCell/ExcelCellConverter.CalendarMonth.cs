using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class CalendarMonthExcelCellConverter : ExcelCellConverter<CalendarMonth>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(CalendarMonth value, IFormatter formatter) {
        return value?.ToString();
    }
    
    protected override ExcelNumberFormat GetNumberFormat(CalendarMonth value, IFormatter formatter) {
        return new CalendarMonthExcelNumberFormat();
    }
}
