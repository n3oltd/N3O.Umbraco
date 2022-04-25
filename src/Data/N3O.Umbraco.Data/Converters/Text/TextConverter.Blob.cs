using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters {
    public class BlobTextConverter : ITextConverter<Blob> {
        public string Convert(IFormatter formatter, Blob value) {
            return value?.Filename;
        }
    }
}