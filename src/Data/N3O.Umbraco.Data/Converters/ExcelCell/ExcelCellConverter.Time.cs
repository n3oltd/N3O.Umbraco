using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using NodaTime;
using System;

namespace N3O.Umbraco.Data.Converters;

public class TimeExcelCellConverter : ExcelCellConverter<LocalTime?>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(LocalTime? value, IFormatter formatter) {
        if (value == null) {
            return null;
        }

        return TimeSpan.FromTicks(value.Value.TickOfDay);
    }

    protected override ExcelNumberFormat GetNumberFormat(LocalTime? value, IFormatter formatter) {
        return new TimeExcelNumberFormat(formatter.DateTime.TimeFormat);
    }

    protected override void ApplyFormatting(Cell<LocalTime?> cell, ExcelFormatting formatting, IFormatter formatter) {
        formatting.HorizontalAlignment = HorizontalAlignment.Right;
    }
}
