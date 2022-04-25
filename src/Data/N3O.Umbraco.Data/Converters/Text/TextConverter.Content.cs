using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters {
    public class ContentTextConverter : ITextConverter<IContent> {
        public string Convert(IFormatter formatter, IContent value) {
            return value.Name;
        }
    }
}