using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using NodaTime;

namespace N3O.Umbraco.Data.Converters;

public class YearMonthExcelCellConverter : ExcelCellConverter<YearMonth?>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(YearMonth? value, IFormatter formatter) {
        return value?.StartOfMonth();
    }

    protected override ExcelNumberFormat GetNumberFormat(YearMonth? value, IFormatter formatter) {
        return new YearMonthExcelNumberFormat();
    }
}
