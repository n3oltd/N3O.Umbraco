using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class StringTextConverter : ITextConverter<string> {
    public string ToInvariantText(string value) {
        return value;
    }

    public string ToText(IFormatter formatter, string value) {
        return ToInvariantText(value);
    }
}
