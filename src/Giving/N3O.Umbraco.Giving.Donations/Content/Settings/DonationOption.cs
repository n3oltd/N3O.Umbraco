using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class DonationOption : UmbracoContent<DonationOption> {
        public DonationOption() {
            Fund = new FundDonationOption();
            Sponsorship = new SponsorshipDonationOption();
        }

        [ValueIgnore]
        public override IPublishedContent Content {
            get => base.Content;

            set {
                base.Content = value;
                Fund.Content = value;
                Sponsorship.Content = value;
            }
        }
        
        public FundDonationOption Fund { get; }
        public SponsorshipDonationOption Sponsorship { get; }
        
        [JsonIgnore]
        public AllocationType Type {
            get {
                if (Content.ContentType.Alias.EqualsInvariant(AliasHelper<FundDonationOption>.ContentTypeAlias())) {
                    return AllocationTypes.Fund;
                } else if (Content.ContentType.Alias.EqualsInvariant(AliasHelper<SponsorshipDonationOption>.ContentTypeAlias())) {
                    return AllocationTypes.Sponsorship;
                } else {
                    throw UnrecognisedValueException.For(Content.ContentType.Alias);
                }
            }
        }
    }
}