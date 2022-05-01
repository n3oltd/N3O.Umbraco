using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Financial;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Extensions {
    public static partial class CsvRowExtensions {
        public static Money GetMoney(this CsvRow csvRow, string heading) {
            return GetMoney(csvRow, CsvSelect.For(heading));
        }

        public static Money GetMoney(this CsvRow csvRow, int index) {
            return GetMoney(csvRow, CsvSelect.For(index));
        }

        public static Money GetMoney(this CsvRow csvRow, CsvSelect select) {
            return csvRow.ParseField(select,
                                     (parser, field) => parser.Money.Parse(field, OurDataTypes.Money.GetClrType()));
        }
    }
}