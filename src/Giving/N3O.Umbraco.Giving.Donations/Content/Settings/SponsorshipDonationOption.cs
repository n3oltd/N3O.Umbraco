using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Donations.Content;

public class SponsorshipDonationOption : DonationOption {
    public SponsorshipScheme Scheme => GetValue<SponsorshipDonationOption, SponsorshipScheme>(x => x.Scheme);
}
