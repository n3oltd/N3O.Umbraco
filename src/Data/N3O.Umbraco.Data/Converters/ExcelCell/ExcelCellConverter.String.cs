using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Converters;

public class StringExcelCellConverter : ExcelCellConverter<string>, IDefaultExcelCellConverter {
    protected override ExcelNumberFormat GetNumberFormat(Column column, string value) {
        return new StringExcelNumberFormat();
    }
}
