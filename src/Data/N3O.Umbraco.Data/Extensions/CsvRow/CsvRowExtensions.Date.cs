using N3O.Umbraco.Data.Models;
using NodaTime;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Extensions {
    public static partial class CsvRowExtensions {
        public static LocalDate? GetDate(this CsvRow csvRow, string heading) {
            return GetDate(csvRow, CsvSelect.For(heading));
        }

        public static LocalDate? GetDate(this CsvRow csvRow, int index) {
            return GetDate(csvRow, CsvSelect.For(index));
        }

        public static LocalDate? GetDate(this CsvRow csvRow, CsvSelect select) {
            return csvRow.ParseField(select,
                                     (parser, field) => parser.Date.Parse(field, OurDataTypes.Date.GetClrType()));
        }
    }
}