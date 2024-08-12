using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public class CrowdfundingPageAllocation {
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public FundDimension1Value FundDimension1Value { get; set; }
    public FundDimension2Value FundDimension2Value { get; set; }
    public FundDimension3Value FundDimension3Value { get; set; }
    public FundDimension4Value FundDimension4Value { get; set; }
    public IEnumerable<CrowdfundingPageAllocationPriceHandle> PriceHandles { get; set; }

    public DonationItem DonationItem { get; set; }
    public SponsorshipScheme SponsorshipScheme { get; set; }
    public FeedbackScheme FeedbackScheme { get; set; }
}