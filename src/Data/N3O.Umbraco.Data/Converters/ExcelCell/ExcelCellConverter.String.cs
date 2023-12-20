using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class StringExcelCellConverter : ExcelCellConverter<string>, IDefaultExcelCellConverter {
    protected override ExcelNumberFormat GetNumberFormat(string value, IFormatter formatter) {
        return new StringExcelNumberFormat();
    }
}
