using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Converters;

public class LookupExcelCellConverter : ExcelCellConverter<INamedLookup>, IDefaultExcelCellConverter {
    protected override object GetExcelValue(INamedLookup value, IFormatter formatter) {
        return formatter.Text.FormatLookupName(value);
    }

    protected override ExcelNumberFormat GetNumberFormat(INamedLookup value, IFormatter formatter) {
        return new LookupExcelNumberFormat();
    }
}
