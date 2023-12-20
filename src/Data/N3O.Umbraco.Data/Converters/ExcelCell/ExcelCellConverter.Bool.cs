using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class BoolExcelCellConverter : ExcelCellConverter<bool?>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(bool? value, IFormatter formatter) {
        return value.ToYesNoString(formatter.Text);
    }

    protected override ExcelNumberFormat GetNumberFormat(bool? value, IFormatter formatter) {
        return new BoolExcelNumberFormat();
    }
}
