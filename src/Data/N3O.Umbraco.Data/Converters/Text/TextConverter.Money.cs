using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;
using System.Globalization;

namespace N3O.Umbraco.Data.Converters;

public class MoneyTextConverter : ITextConverter<Money> {
    public string ToInvariantText(Money value) {
        return value.IfNotNull(x => $"{x.Currency.Symbol}{x.Amount.ToString(CultureInfo.InvariantCulture)}");
    }

    public string ToText(IFormatter formatter, Money value) {
        return formatter.Number.FormatMoney(value);
    }
}
