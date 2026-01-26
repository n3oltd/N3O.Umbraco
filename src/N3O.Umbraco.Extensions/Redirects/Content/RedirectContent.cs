using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using NodaTime;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Redirects;

public class RedirectContent : UmbracoContent<RedirectContent> {
    public int HitCount => GetValue(x => x.HitCount);
    public LocalDate LastHitDate => GetLocalDate(x => x.LastHitDate);
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
