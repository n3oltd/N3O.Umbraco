﻿using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Cloud.Engage.Models;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Goal.Fund.Alias)]
public class FundGoalElement : UmbracoElement<FundGoalElement>, IFundCrowdfunderGoal {
    public DonationItem GetDonationItem(ILookups lookups) {
        return GetLookup<DonationItem>(lookups, CrowdfundingConstants.CampaignGoalOption.Fund.Properties.DonationItem);
    }
}