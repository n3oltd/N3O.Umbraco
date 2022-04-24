using N3O.Umbraco.Localization;
using N3O.Umbraco.References;

namespace N3O.Umbraco.Data.Converters {
    public class ReferenceTextConverter : ITextConverter<Reference> {
        public string Convert(IFormatter formatter, Reference value) {
            return value.Text;
        }
    }
}