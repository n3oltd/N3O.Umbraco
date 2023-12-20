using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using N3O.Umbraco.References;

namespace N3O.Umbraco.Data.Converters;

public class ReferenceExcelCellConverter : ExcelCellConverter<Reference>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(Reference value, IFormatter formatter) {
        return value?.Text;
    }

    protected override ExcelNumberFormat GetNumberFormat(Reference value, IFormatter formatter) {
        return new ReferenceExcelNumberFormat();
    }
}
