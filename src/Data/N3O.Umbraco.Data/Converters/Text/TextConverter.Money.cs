using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters {
    public class MoneyTextConverter : ITextConverter<Money> {
        public string Convert(IFormatter formatter, Money value) {
            return formatter.Number.FormatMoney(value);
        }
    }
}