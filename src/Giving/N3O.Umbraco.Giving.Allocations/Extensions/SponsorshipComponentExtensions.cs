using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Extensions;

public static class SponsorshipComponentExtensions {
    public static SponsorshipScheme GetSponsorshipScheme(this SponsorshipComponent sponsorshipComponent,
                                                         IContentLocator contentLocator) {
        if (sponsorshipComponent.ContentId.HasValue()) {
            var sponsorshipComponentContent = contentLocator.ById<SponsorshipComponentContent>(sponsorshipComponent.ContentId.GetValueOrThrow());
            
            return sponsorshipComponentContent.GetScheme();
        }
        
        return null;
    }
}