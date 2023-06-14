using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Converters;

public class GuidTextConverter : ITextConverter<Guid?> {
    public string ToInvariantText(Guid? value) {
        return value?.ToString();
    }

    public string ToText(IFormatter formatter, Guid? value) {
        return ToInvariantText(value);
    }
}
