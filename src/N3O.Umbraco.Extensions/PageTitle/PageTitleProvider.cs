using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.PageTitle;

public abstract class PageTitleProvider<T> : IPageTitleProvider where T : IPublishedContent {
    public string GetPageTitle(IPublishedContent page) {
        return GetPageTitle((T) page);
    }

    public virtual bool IsProviderFor(IPublishedContent page) {
        return page.GetType().IsAssignableTo(typeof(T));
    }

    protected abstract string GetPageTitle(T page);
}
