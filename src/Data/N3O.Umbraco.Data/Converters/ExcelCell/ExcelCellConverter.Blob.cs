using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Converters {
    public class BlobExcelCellConverter : ExcelCellConverter<Blob>, IDefaultExcelCellConverter {
        protected override object GetExcelValue(Column column, Blob value) {
            return value.Filename;
        }
        
        protected override ExcelNumberFormat GetNumberFormat(Column column, Blob value) {
            return new StringExcelNumberFormat();
        }
    }
}