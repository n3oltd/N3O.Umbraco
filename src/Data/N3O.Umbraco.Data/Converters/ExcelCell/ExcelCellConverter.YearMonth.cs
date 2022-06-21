using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using NodaTime;

namespace N3O.Umbraco.Data.Converters;

public class YearMonthExcelCellConverter : ExcelCellConverter<YearMonth?>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(Column column, YearMonth? value) {
        return value?.StartOfMonth();
    }

    protected override ExcelNumberFormat GetNumberFormat(Column column, YearMonth? value) {
        return new YearMonthExcelNumberFormat();
    }
}
