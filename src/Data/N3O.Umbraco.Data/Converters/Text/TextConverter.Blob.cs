using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class BlobTextConverter : ITextConverter<Blob> {
    public string ToInvariantText(Blob value) {
        return value?.Filename;
    }

    public string ToText(IFormatter formatter, Blob value) {
        return ToInvariantText(value);
    }
}
