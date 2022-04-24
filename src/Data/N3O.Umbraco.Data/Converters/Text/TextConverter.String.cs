using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters {
    public class StringTextConverter : ITextConverter<string> {
        public string Convert(IFormatter formatter, string value) {
            return value;
        }
    }
}