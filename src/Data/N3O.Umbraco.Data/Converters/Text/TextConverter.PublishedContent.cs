using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Data.Converters;

public class PublishedContentTextConverter : ITextConverter<IPublishedContent> {
    public string ToInvariantText(IPublishedContent value) {
        return value.Name;
    }

    public string ToText(IFormatter formatter, IPublishedContent value) {
        return ToInvariantText(value);
    }
}
