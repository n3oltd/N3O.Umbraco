using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Converters {
    public class LookupExcelCellConverter : ExcelCellConverter<INamedLookup>, IDefaultExcelCellConverter {
        protected override object GetExcelValue(Column column, INamedLookup value) {
            return column.Formatter.Text.FormatLookupName(value);
        }

        protected override ExcelNumberFormat GetNumberFormat(Column column, INamedLookup value) {
            return new LookupExcelNumberFormat();
        }
    }
}