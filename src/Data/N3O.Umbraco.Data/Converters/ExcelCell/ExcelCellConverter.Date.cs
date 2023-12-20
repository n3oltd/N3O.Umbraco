using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using NodaTime;

namespace N3O.Umbraco.Data.Converters;

public class DateExcelCellConverter : ExcelCellConverter<LocalDate?>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(LocalDate? value, IFormatter formatter) {
        return value?.ToDateTimeUnspecified();
    }

    protected override ExcelNumberFormat GetNumberFormat(LocalDate? value, IFormatter formatter) {
        return new DateExcelNumberFormat(formatter.DateTime.DateFormat);
    }

    protected override void ApplyFormatting(Cell<LocalDate?> cell, ExcelFormatting formatting, IFormatter formatter) {
        formatting.HorizontalAlignment = HorizontalAlignment.Right;
    }
}
