using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class DonationOptionContent : UmbracoContent<DonationOptionContent> {
        private static readonly string FundDonationOptionAlias = AliasHelper<FundDonationOptionContent>.ContentTypeAlias();
        private static readonly string SponsorshipDonationOptionAlias = AliasHelper<SponsorshipDonationOptionContent>.ContentTypeAlias();
        
        public DonationOptionContent() {
            Fund = new FundDonationOptionContent();
            Sponsorship = new SponsorshipDonationOptionContent();
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
        
        public FundDonationOptionContent Fund { get; }
        public SponsorshipDonationOptionContent Sponsorship { get; }
        
        [JsonIgnore]
        public AllocationType Type {
            get {
                if (Content.ContentType.Alias.EqualsInvariant(FundDonationOptionAlias)) {
                    return AllocationTypes.Fund;
                } else if (Content.ContentType.Alias.EqualsInvariant(SponsorshipDonationOptionAlias)) {
                    return AllocationTypes.Sponsorship;
                } else {
                    throw UnrecognisedValueException.For(Content.ContentType.Alias);
                }
            }
        }
    }
}