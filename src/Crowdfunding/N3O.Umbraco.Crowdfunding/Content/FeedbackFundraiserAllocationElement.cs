using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.FundraiserAllocation.Feedback.Alias)]
public class FeedbackFundraiserAllocationElement : UmbracoElement<FeedbackFundraiserAllocationElement> {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);
}
