using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using NodaTime;

namespace N3O.Umbraco.Data.Converters;

public class DateExcelCellConverter : ExcelCellConverter<LocalDate?>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(Column column, LocalDate? value) {
        return value?.ToDateTimeUnspecified();
    }

    protected override ExcelNumberFormat GetNumberFormat(Column column, LocalDate? value) {
        return new DateExcelNumberFormat(column.Formatter.DateTime.DateFormat);
    }

    protected override void ApplyFormatting(Column column, Cell<LocalDate?> cell, ExcelFormatting formatting) {
        formatting.HorizontalAlignment = HorizontalAlignment.Right;
    }
}
