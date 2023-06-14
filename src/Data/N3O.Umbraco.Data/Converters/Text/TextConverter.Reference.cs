using N3O.Umbraco.Localization;
using N3O.Umbraco.References;

namespace N3O.Umbraco.Data.Converters;

public class ReferenceTextConverter : ITextConverter<Reference> {
    public string ToInvariantText(Reference value) {
        return value?.Text;
    }

    public string ToText(IFormatter formatter, Reference value) {
        return ToInvariantText(value);
    }
}
