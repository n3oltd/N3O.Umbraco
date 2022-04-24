using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Converters {
    public class LookupTextConverter : ITextConverter<INamedLookup> {
        public string Convert(IFormatter formatter, INamedLookup value) {
            var text = formatter.Text.FormatLookupName(value);

            return text;
        }
    }
}