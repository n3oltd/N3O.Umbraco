using N3O.Umbraco.Data.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Data.Converters;

public class PublishedContentExcelCellConverter : ExcelCellConverter<IPublishedContent>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(Column column, IPublishedContent value) {
        return value.Name;
    }

    protected override ExcelNumberFormat GetNumberFormat(Column column, IPublishedContent value) {
        return new StringExcelNumberFormat();
    }
}
