using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class MoneyExcelCellConverter : ExcelCellConverter<Money>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(Money value, IFormatter formatter) {
        return value?.Amount;
    }

    protected override ExcelNumberFormat GetNumberFormat(Money value, IFormatter formatter) {
        return new MoneyExcelNumberFormat(value?.Currency);
    }

    protected override void ApplyFormatting(Cell<Money> cell, ExcelFormatting formatting, IFormatter formatter) {
        formatting.HorizontalAlignment = HorizontalAlignment.Right;
    }
}
