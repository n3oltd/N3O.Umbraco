using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.PageTitle;

public interface IPageTitleProvider {
    string GetPageTitle(IPublishedContent page);
    bool IsProviderFor(IPublishedContent page);
}
