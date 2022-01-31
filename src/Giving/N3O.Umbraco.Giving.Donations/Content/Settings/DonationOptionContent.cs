using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.FundDimensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class DonationOptionContent : UmbracoContent<DonationOptionContent>, IHoldFundDimensions {
        private static readonly string FundDonationOptionAlias = AliasHelper<FundDonationOptionContent>.ContentTypeAlias();
        private static readonly string SponsorshipDonationOptionAlias = AliasHelper<SponsorshipDonationOptionContent>.ContentTypeAlias();

        [ValueIgnore]
        public override IPublishedContent Content {
            get => base.Content;

            set {
                base.Content = value;

                if (Type == AllocationTypes.Fund) {
                    Fund = new FundDonationOptionContent();
                    Fund.Content = Content;
                } else if (Type == AllocationTypes.Fund) {
                    Sponsorship = new SponsorshipDonationOptionContent();
                    Sponsorship.Content = Content;
                } else {
                    throw UnrecognisedValueException.For(Type);
                } 
            }
        }
        
        public FundDimension1Option Dimension1 => GetAs(x => x.Dimension1);
        public FundDimension2Option Dimension2 => GetAs(x => x.Dimension2);
        public FundDimension3Option Dimension3 => GetAs(x => x.Dimension3);
        public FundDimension4Option Dimension4 => GetAs(x => x.Dimension4);
        public bool HideQuantity => GetValue(x => x.HideQuantity);
        public bool HideDonation => GetValue(x => x.HideDonation);
        public bool HideRegularGiving => GetValue(x => x.HideRegularGiving);

        public FundDonationOptionContent Fund { get; private set; }
        public SponsorshipDonationOptionContent Sponsorship { get; private set; }

        public IHoldFundDimensionOptions GetFundDimensionOptions() {
            return (IHoldFundDimensionOptions) Fund?.DonationItem ?? Sponsorship?.Scheme;
        }
        
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