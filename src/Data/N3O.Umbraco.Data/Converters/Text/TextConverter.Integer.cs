using N3O.Umbraco.Localization;
using System.Globalization;

namespace N3O.Umbraco.Data.Converters;

public class IntegerTextConverter : ITextConverter<long?> {
    public string ToInvariantText(long? value) {
        return value?.ToString(CultureInfo.InvariantCulture);
    }

    public string ToText(IFormatter formatter, long? value) {
        return formatter.Number.Format(value);
    }
}
