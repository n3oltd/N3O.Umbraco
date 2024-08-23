using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public partial class FundraiserOrCampaignViewModel<TContent> {
    public class Allocation {
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public FundDimension1Value FundDimension1Value { get; set; }
        public FundDimension2Value FundDimension2Value { get; set; }
        public FundDimension3Value FundDimension3Value { get; set; }
        public FundDimension4Value FundDimension4Value { get; set; }
        public AllocationType Type { get; set; }
        public DonationItem DonationItem { get; set; }
        public SponsorshipScheme SponsorshipScheme { get; set; }
        public FeedbackScheme FeedbackScheme { get; set; }
        public IReadOnlyList<PriceHandle> PriceHandles { get; set; }

        public static Allocation For(ICrowdfundingHelper crowdfundingHelper,
                                     FundraiserAllocationElement fundraiserAllocation) {
            var allocation = new Allocation();

            allocation.Title = fundraiserAllocation.Title;
            allocation.Amount = fundraiserAllocation.Amount;
            allocation.FundDimension1Value = fundraiserAllocation.FundDimension1;
            allocation.FundDimension2Value = fundraiserAllocation.FundDimension2;
            allocation.FundDimension3Value = fundraiserAllocation.FundDimension3;
            allocation.FundDimension4Value = fundraiserAllocation.FundDimension4;
            allocation.Type = fundraiserAllocation.Type;
            allocation.DonationItem = fundraiserAllocation.Fund?.DonationItem;
            allocation.FeedbackScheme = fundraiserAllocation.Feedback?.Scheme;
            allocation.PriceHandles = fundraiserAllocation.PriceHandles
                                                          .ToReadOnlyList(x => PriceHandle.For(crowdfundingHelper, x));

            return allocation;
        }
    }
}