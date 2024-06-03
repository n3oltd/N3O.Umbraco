using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public interface IPageModeAccessor {
    PageMode GetPageMode(IPublishedContent content);
}