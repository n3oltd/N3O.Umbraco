using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Converters;

public class GuidTextConverter : ITextConverter<Guid?> {
    public string Convert(IFormatter formatter, Guid? value) {
        return value?.ToString();
    }
}
