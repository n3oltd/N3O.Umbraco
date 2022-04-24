using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters {
    public class BoolTextConverter : ITextConverter<bool?> {
        public string Convert(IFormatter formatter, bool? value) {
            return value.ToYesNoString(formatter.Text);
        }
    }
}