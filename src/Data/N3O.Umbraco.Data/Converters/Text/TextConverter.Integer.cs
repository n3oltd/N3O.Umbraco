using N3O.Umbraco.Localization;
using System.Globalization;

namespace N3O.Umbraco.Data.Converters;

public class IntegerTextConverter : ITextConverter<long?> {
    public string Convert(IFormatter formatter, long? value) {
        return value?.ToString(CultureInfo.InvariantCulture);
    }
}
