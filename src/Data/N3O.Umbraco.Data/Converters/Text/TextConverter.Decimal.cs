using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class DecimalTextConverter : ITextConverter<decimal?> {
    public string Convert(IFormatter formatter, decimal? value) {
        return formatter.Number.Format(value);
    }
}
