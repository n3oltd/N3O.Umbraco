using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class IntegerExcelCellConverter : ExcelCellConverter<long?>, IDefaultExcelCellConverter {
    protected override ExcelNumberFormat GetNumberFormat(long? value, IFormatter formatter) {
        return new IntegerExcelNumberFormat();
    }

    protected override void ApplyFormatting(Cell<long?> cell, ExcelFormatting formatting, IFormatter formatter) {
        formatting.HorizontalAlignment = HorizontalAlignment.Right;
    }
}
