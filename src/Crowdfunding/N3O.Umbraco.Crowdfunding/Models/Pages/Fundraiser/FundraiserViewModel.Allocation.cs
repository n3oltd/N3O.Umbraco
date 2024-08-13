using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public partial class FundraiserViewModel {
    public class Allocation {
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public FundDimension1Value FundDimension1Value { get; set; }
        public FundDimension2Value FundDimension2Value { get; set; }
        public FundDimension3Value FundDimension3Value { get; set; }
        public FundDimension4Value FundDimension4Value { get; set; }
        public IReadOnlyList<PriceHandle> PriceHandles { get; set; }

        public DonationItem DonationItem { get; set; }
        public SponsorshipScheme SponsorshipScheme { get; set; }
        public FeedbackScheme FeedbackScheme { get; set; }

        public static Allocation For(ICrowdfundingHelper crowdfundingHelper,
                                     FundraiserAllocationElement fundraiserAllocation) {
            var allocation = new Allocation();

            allocation.Title = fundraiserAllocation.Title;
            allocation.Amount = fundraiserAllocation.Amount;
            allocation.FundDimension1Value = fundraiserAllocation.Dimension1;
            allocation.FundDimension2Value = fundraiserAllocation.Dimension2;
            allocation.FundDimension3Value = fundraiserAllocation.Dimension3;
            allocation.FundDimension4Value = fundraiserAllocation.Dimension4;
            allocation.DonationItem = fundraiserAllocation.DonationItem;
            allocation.SponsorshipScheme = fundraiserAllocation.SponsorshipScheme;
            allocation.FeedbackScheme = fundraiserAllocation.FeedbackScheme;
            allocation.FeedbackScheme = fundraiserAllocation.FeedbackScheme;
            allocation.PriceHandles = fundraiserAllocation.PriceHandles
                                                          .ToReadOnlyList(x => PriceHandle.For(crowdfundingHelper, x));

            return allocation;
        }
    }
}