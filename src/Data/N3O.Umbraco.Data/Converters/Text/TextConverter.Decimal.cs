using N3O.Umbraco.Localization;
using System.Globalization;

namespace N3O.Umbraco.Data.Converters;

public class DecimalTextConverter : ITextConverter<decimal?> {
    public string ToInvariantText(decimal? value) {
        return value?.ToString(CultureInfo.InvariantCulture);
    }

    public string ToText(IFormatter formatter, decimal? value) {
        return formatter.Number.Format(value);
    }
}
