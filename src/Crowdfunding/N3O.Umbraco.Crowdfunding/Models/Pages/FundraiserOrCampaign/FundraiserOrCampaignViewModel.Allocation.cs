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

        public static Allocation For<T>(ICrowdfundingHelper crowdfundingHelper,
                                        CrowdfunderGoalElement<T> crowdfunderGoal)
            where T : CrowdfunderGoalElement<T> {
            var allocation = new Allocation();

            allocation.Title = crowdfunderGoal.Title;
            allocation.Amount = crowdfunderGoal.Amount;
            allocation.FundDimension1Value = crowdfunderGoal.FundDimension1;
            allocation.FundDimension2Value = crowdfunderGoal.FundDimension2;
            allocation.FundDimension3Value = crowdfunderGoal.FundDimension3;
            allocation.FundDimension4Value = crowdfunderGoal.FundDimension4;
            allocation.Type = crowdfunderGoal.Type;
            allocation.DonationItem = crowdfunderGoal.Fund?.DonationItem;
            allocation.FeedbackScheme = crowdfunderGoal.Feedback?.Scheme;
            allocation.PriceHandles = crowdfunderGoal.PriceHandles
                                                     .ToReadOnlyList(x => PriceHandle.For(crowdfundingHelper, x));

            return allocation;
        }
    }
}