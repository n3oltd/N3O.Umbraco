using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public interface IUmbracoContent {
    IPublishedContent Content();
    void SetContent(IPublishedContent content);
    void SetVariationContext(VariationContext variationContext);
}
