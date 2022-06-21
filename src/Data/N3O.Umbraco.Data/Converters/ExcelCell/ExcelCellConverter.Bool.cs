using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Data.Converters;

public class BoolExcelCellConverter : ExcelCellConverter<bool?>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(Column column, bool? value) {
        return value.ToYesNoString(column.Formatter.Text);
    }

    protected override ExcelNumberFormat GetNumberFormat(Column column, bool? value) {
        return new BoolExcelNumberFormat();
    }
}
