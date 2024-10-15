using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crm.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Campaign.Alias)]
public class CampaignContent : CrowdfunderContent<CampaignContent>, ICampaign {
    public decimal MinimumAmount => GetValue(x => x.MinimumAmount);
    public IEnumerable<CampaignGoalOptionElement> GoalOptions => GetNestedAs(x => x.GoalOptions);
    
    public override Guid CampaignId => Key;
    public override string CampaignName => Name;
    public override Guid? TeamId => null;
    public override string TeamName => null;
    public override Guid? FundraiserId => null;

    public override string Url(ICrowdfundingUrlBuilder urlBuilder) {
        return ViewCampaignPage.Url(urlBuilder, Key);
    }

    protected override void PopulateFullText(StringBuilder sb) { }
}