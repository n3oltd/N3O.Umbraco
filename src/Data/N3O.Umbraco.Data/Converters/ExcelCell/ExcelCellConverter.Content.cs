using N3O.Umbraco.Data.Models;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters {
    public class ContentExcelCellConverter : ExcelCellConverter<IContent>, IDefaultExcelCellConverter {
        protected override object GetExcelValue(Column column, IContent value) {
            return value.Name;
        }

        protected override ExcelNumberFormat GetNumberFormat(Column column, IContent value) {
            return new StringExcelNumberFormat();
        }
    }
}