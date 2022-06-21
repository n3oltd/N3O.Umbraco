using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Models;

public class DateTimeExcelNumberFormat : ExcelNumberFormat {
    public DateTimeExcelNumberFormat(DateFormat dateFormat, TimeFormat timeFormat) {
        var dateNumberFormat = new DateExcelNumberFormat(dateFormat);
        var timeNumberFormat = new TimeExcelNumberFormat(timeFormat);

        Pattern = $"{dateNumberFormat.Pattern} {timeNumberFormat.Pattern}";
    }
}
