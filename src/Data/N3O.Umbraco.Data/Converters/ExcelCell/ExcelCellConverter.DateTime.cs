using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using NodaTime;

namespace N3O.Umbraco.Data.Converters {
    public class DateTimeExcelCellConverter : ExcelCellConverter<LocalDateTime?>, IDefaultExcelCellConverter {
        protected override object GetExcelValue(Column column, LocalDateTime? value) {
            return value?.ToDateTimeUnspecified();
        }

        protected override ExcelNumberFormat GetNumberFormat(Column column, LocalDateTime? value) {
            return new DateTimeExcelNumberFormat(column.LocalizationSettings.DateFormat,
                                                 column.LocalizationSettings.TimeFormat);
        }

        protected override void ApplyFormatting(Column column, Cell<LocalDateTime?> cell, ExcelFormatting formatting) {
            formatting.HorizontalAlignment = HorizontalAlignment.Right;
        }
    }
}