using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Content;

public class ContentLocator : Locator, IContentLocator {
    public ContentLocator(IUmbracoContextAccessor umbracoContextAccessor) : base(umbracoContextAccessor) { }
    
    protected override IPublishedCache GetCache(IUmbracoContextAccessor umbracoContextAccessor) {
        return umbracoContextAccessor.GetContentCache();
    }
}
