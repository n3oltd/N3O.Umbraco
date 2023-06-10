using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class IntegerTextConverter : ITextConverter<long?> {
    public string Convert(IFormatter formatter, long? value) {
        return formatter.Number.Format(value);
    }
}
