using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class BlobExcelCellConverter : ExcelCellConverter<Blob>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(Blob value, IFormatter formatter) {
        return value.Filename;
    }
    
    protected override ExcelNumberFormat GetNumberFormat(Blob value, IFormatter formatter) {
        return new StringExcelNumberFormat();
    }
}
