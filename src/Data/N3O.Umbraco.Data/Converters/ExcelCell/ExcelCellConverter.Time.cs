using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using NodaTime;
using System;

namespace N3O.Umbraco.Data.Converters {
    public class TimeExcelCellConverter : ExcelCellConverter<LocalTime?>, IDefaultExcelCellConverter {
        protected override object GetExcelValue(Column column, LocalTime? value) {
            if (value == null) {
                return null;
            }

            return TimeSpan.FromTicks(value.Value.TickOfDay);
        }

        protected override ExcelNumberFormat GetNumberFormat(Column column, LocalTime? value) {
            return new TimeExcelNumberFormat(column.LocalizationSettings.TimeFormat);
        }

        protected override void ApplyFormatting(Column column, Cell<LocalTime?> cell, ExcelFormatting formatting) {
            formatting.HorizontalAlignment = HorizontalAlignment.Right;
        }
    }
}