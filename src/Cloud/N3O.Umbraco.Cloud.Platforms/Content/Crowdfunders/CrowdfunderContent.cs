using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Crowdfunders.Crowdfunder.Alias)]
public class CrowdfunderContent : UmbracoContent<CrowdfunderContent> {
    public Guid Key => Content().Key;

    // The campaign this crowdfunder raises for. Empty until an editor picks one, so every consumer has to allow
    // for null - GetPickedAs returns default rather than throwing.
    public IPublishedContent Campaign => GetPickedAs(x => x.Campaign);
}
