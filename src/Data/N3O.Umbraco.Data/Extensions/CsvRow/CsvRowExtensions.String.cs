using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Extensions {
    public static partial class CsvRowExtensions {
        public static string GetString(this CsvRow csvRow, string heading) {
            return GetString(csvRow, CsvSelect.For(heading));
        }

        public static string GetString(this CsvRow csvRow, int index) {
            return GetString(csvRow, CsvSelect.For(index));
        }

        public static string GetString(this CsvRow csvRow, CsvSelect select) {
            return csvRow.ParseField(select,
                                     (parser, field) => parser.String.Parse(field, DataTypes.String.GetClrType()));
        }
    }
}