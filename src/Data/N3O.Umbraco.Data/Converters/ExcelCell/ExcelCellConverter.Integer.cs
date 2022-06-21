using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Converters;

public class IntegerExcelCellConverter : ExcelCellConverter<long?>, IDefaultExcelCellConverter {
    protected override ExcelNumberFormat GetNumberFormat(Column column, long? value) {
        return new IntegerExcelNumberFormat();
    }

    protected override void ApplyFormatting(Column column, Cell<long?> cell, ExcelFormatting formatting) {
        formatting.HorizontalAlignment = HorizontalAlignment.Right;
    }
}
