using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class DonationOption : UmbracoContent {
        [JsonIgnore]
        public AllocationType Type {
            get {
                if (Content.ContentType.Alias.EqualsInvariant(AliasHelper.ForContentType<FundDonationOption>())) {
                    return AllocationTypes.Fund;
                } else if (Content.ContentType.Alias.EqualsInvariant(AliasHelper.ForContentType<SponsorshipDonationOption>())) {
                    return AllocationTypes.Sponsorship;
                } else {
                    throw UnrecognisedValueException.For(Content.ContentType.Alias);
                }
            }
        }
    }
}