using N3O.Umbraco.Data.Models;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Extensions {
    public static partial class CsvRowExtensions {
        public static Guid? GetGuid(this CsvRow csvRow, string heading) {
            return GetGuid(csvRow, CsvSelect.For(heading));
        }

        public static Guid? GetGuid(this CsvRow csvRow, int index) {
            return GetGuid(csvRow, CsvSelect.For(index));
        }

        public static Guid? GetGuid(this CsvRow csvRow, CsvSelect select) {
            return csvRow.ParseField(select,
                                     (parser, field) => parser.Guid.Parse(field, OurDataTypes.Guid.GetClrType()));
        }
    }
}