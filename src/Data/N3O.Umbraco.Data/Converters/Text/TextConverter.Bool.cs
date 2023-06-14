using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class BoolTextConverter : ITextConverter<bool?> {
    public string ToInvariantText(bool? value) {
        return value?.ToString().ToLowerInvariant();
    }

    public string ToText(IFormatter formatter, bool? value) {
        return value.ToYesNoString(formatter.Text);
    }
}
