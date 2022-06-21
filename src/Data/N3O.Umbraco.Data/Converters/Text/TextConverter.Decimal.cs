using N3O.Umbraco.Localization;
using System.Globalization;

namespace N3O.Umbraco.Data.Converters;

public class DecimalTextConverter : ITextConverter<decimal?> {
    public string Convert(IFormatter formatter, decimal? value) {
        return value?.ToString(CultureInfo.InvariantCulture);
    }
}
