using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Redirects.Content;

public class RedirectContent : UmbracoContent<RedirectContent> {
    public IPublishedContent LinkContent => GetAs(x => x.LinkContent);
    public string LinkExternalUrl => GetValue(x => x.LinkExternalUrl);
    public bool Temporary => GetValue(x => x.Temporary);

    public string GetLinkUrl() {
        if (LinkContent.HasValue()) {
            return LinkContent.AbsoluteUrl();
        } 
        
        return LinkExternalUrl;
    }
}
