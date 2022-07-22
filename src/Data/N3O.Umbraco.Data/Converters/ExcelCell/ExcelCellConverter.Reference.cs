using N3O.Umbraco.Data.Models;
using N3O.Umbraco.References;

namespace N3O.Umbraco.Data.Converters;

public class ReferenceExcelCellConverter : ExcelCellConverter<Reference>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(Column column, Reference value) {
        return value?.Text;
    }

    protected override ExcelNumberFormat GetNumberFormat(Column column, Reference value) {
        return new ReferenceExcelNumberFormat();
    }
}
