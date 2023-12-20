using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Data.Converters;

public class PublishedContentExcelCellConverter : ExcelCellConverter<IPublishedContent>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(IPublishedContent value, IFormatter formatter) {
        return value.Name;
    }

    protected override ExcelNumberFormat GetNumberFormat(IPublishedContent value, IFormatter formatter) {
        return new StringExcelNumberFormat();
    }
}
