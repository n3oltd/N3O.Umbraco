using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.SponsorshipDesignation.Alias)]
public class SponsorshipDesignationContent : UmbracoContent<SponsorshipDesignationContent> {
    public SponsorshipScheme GetScheme(ILookups lookups) {
        return GetLookup<SponsorshipScheme>(lookups, PlatformsConstants.SponsorshipDesignation.Properties.Scheme);
    }
}