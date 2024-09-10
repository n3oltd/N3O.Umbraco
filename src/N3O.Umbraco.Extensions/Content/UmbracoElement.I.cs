using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public interface IUmbracoElement {
    IPublishedElement Content();
    IPublishedContent Parent();
    void Content(IPublishedElement content, IPublishedContent parent);
}