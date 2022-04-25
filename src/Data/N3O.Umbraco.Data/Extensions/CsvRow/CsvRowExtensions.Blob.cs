using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Extensions {
    public static partial class CsvRowExtensions {
        public static Blob GetBlob(this CsvRow csvRow, string heading) {
            return GetBlob(csvRow, CsvSelect.For(heading));
        }

        public static Blob GetBlob(this CsvRow csvRow, int index) {
            return GetBlob(csvRow, CsvSelect.For(index));
        }

        public static Blob GetBlob(this CsvRow csvRow, CsvSelect select) {
            return csvRow.ParseField(select, (parser, field) => parser.Blob.Parse(field, DataTypes.Blob.GetClrType()));
        }
    }
}