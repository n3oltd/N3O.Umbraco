using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.FundraiserAllocation.Sponsorship.Alias)]
public class SponsorshipFundraiserAllocationElement : UmbracoElement<SponsorshipFundraiserAllocationElement> {
    public SponsorshipScheme Scheme => GetValue(x => x.Scheme);
}
