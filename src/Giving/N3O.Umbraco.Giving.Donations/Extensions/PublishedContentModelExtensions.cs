using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Donations.Content;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Donations.Extensions {
    public static class PublishedContentModelExtensions {
        public static DonationOption ToDonationOption(this PublishedContentModel content) {
            var donationOption = content.As<DonationOption>();
        
            if (donationOption.Type == AllocationTypes.Fund) {
                return donationOption.Content.As<FundDonationOption>();   
            } else if (donationOption.Type == AllocationTypes.Sponsorship) {
                return donationOption.Content.As<SponsorshipDonationOption>();   
            } else {
                throw UnrecognisedValueException.For(donationOption.Type);
            }
        }
    }
}