using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class DecimalExcelCellConverter : ExcelCellConverter<decimal?>, IDefaultExcelCellConverter {
    protected override ExcelNumberFormat GetNumberFormat(decimal? value, IFormatter formatter) {
        return new DecimalExcelNumberFormat();
    }

    protected override void ApplyFormatting(Cell<decimal?> cell, ExcelFormatting formatting, IFormatter formatter) {
        formatting.HorizontalAlignment = HorizontalAlignment.Right;
    }
}
