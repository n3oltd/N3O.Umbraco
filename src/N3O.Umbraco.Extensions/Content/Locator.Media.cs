using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Media;

public class MediaLocator : Locator, IMediaLocator {
    public MediaLocator(IUmbracoContextAccessor umbracoContextAccessor) : base(umbracoContextAccessor) { }
    
    protected override IPublishedCache GetCache(IUmbracoContextAccessor umbracoContextAccessor) {
        return umbracoContextAccessor.GetMediaCache();
    }
}