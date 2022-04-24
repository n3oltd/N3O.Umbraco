using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Converters {
    public class DecimalExcelCellConverter : ExcelCellConverter<decimal?>, IDefaultExcelCellConverter {
        protected override ExcelNumberFormat GetNumberFormat(Column column, decimal? value) {
            return new DecimalExcelNumberFormat();
        }

        protected override void ApplyFormatting(Column column, Cell<decimal?> cell, ExcelFormatting formatting) {
            formatting.HorizontalAlignment = HorizontalAlignment.Right;
        }
    }
}