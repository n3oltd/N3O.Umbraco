using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters;

public class ContentExcelCellConverter : ExcelCellConverter<IContent>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(IContent value, IFormatter formatter) {
        return value.Name;
    }

    protected override ExcelNumberFormat GetNumberFormat(IContent value, IFormatter formatter) {
        return new StringExcelNumberFormat();
    }
}
