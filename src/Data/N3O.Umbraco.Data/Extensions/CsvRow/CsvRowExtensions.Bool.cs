using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Extensions {
    public static partial class CsvRowExtensions {
        public static bool? GetBool(this CsvRow csvRow, string heading) {
            return GetBool(csvRow, CsvSelect.For(heading));
        }

        public static bool? GetBool(this CsvRow csvRow, int index) {
            return GetBool(csvRow, CsvSelect.For(index));
        }

        public static bool? GetBool(this CsvRow csvRow, CsvSelect select) {
            return csvRow.ParseField(select, (parser, field) => parser.Bool.Parse(field, DataTypes.Bool.GetClrType()));
        }
    }
}