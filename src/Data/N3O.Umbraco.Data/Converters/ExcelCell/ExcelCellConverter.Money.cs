using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Data.Converters {
    public class MoneyExcelCellConverter : ExcelCellConverter<Money>, IDefaultExcelCellConverter {
        protected override object GetExcelValue(Column column, Money value) {
            return value?.Amount;
        }

        protected override ExcelNumberFormat GetNumberFormat(Column column, Money value) {
            return new MoneyExcelNumberFormat(value?.Currency);
        }

        protected override void ApplyFormatting(Column column, Cell<Money> cell, ExcelFormatting formatting) {
            formatting.HorizontalAlignment = HorizontalAlignment.Right;
        }
    }
}