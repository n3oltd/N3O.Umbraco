using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using NodaTime;

namespace N3O.Umbraco.Data.Converters;

public class DateTimeExcelCellConverter : ExcelCellConverter<LocalDateTime?>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(LocalDateTime? value, IFormatter formatter) {
        return value?.ToDateTimeUnspecified();
    }

    protected override ExcelNumberFormat GetNumberFormat(LocalDateTime? value, IFormatter formatter) {
        return new DateTimeExcelNumberFormat(formatter.DateTime.DateFormat, formatter.DateTime.TimeFormat);
    }

    protected override void ApplyFormatting(Cell<LocalDateTime?> cell,
                                            ExcelFormatting formatting,
                                            IFormatter formatter) {
        formatting.HorizontalAlignment = HorizontalAlignment.Right;
    }
}
