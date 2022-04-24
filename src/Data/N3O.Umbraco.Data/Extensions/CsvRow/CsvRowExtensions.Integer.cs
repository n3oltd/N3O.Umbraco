using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Extensions {
    public static partial class CsvRowExtensions {
        public static long? GetInteger(this CsvRow csvRow, string heading) {
            return GetInteger(csvRow, CsvSelect.For(heading));
        }

        public static long? GetInteger(this CsvRow csvRow, int index) {
            return GetInteger(csvRow, CsvSelect.For(index));
        }

        public static long? GetInteger(this CsvRow csvRow, CsvSelect select) {
            return csvRow.ParseField(select,
                                     (parser, field) => parser.Integer.Parse(field, DataTypes.Integer.GetClrType()));
        }
    }
}