using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Engage.Models;
using System;
using System.Text;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Fundraiser.Alias)]
public class FundraiserContent : CrowdfunderContent<FundraiserContent>, IFundraiser {
    public string Account => GetValue(x => x.Account);
    public string Slug => GetValue(x => x.Slug);
    public CampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public MemberContent Owner => GetPickedAs(x => x.Owner);
    
    public override Guid CampaignId => Campaign.Key;
    public override string CampaignName => Campaign.Name;
    public override Guid? TeamId => null;
    public override string TeamName => null;
    public override Guid? FundraiserId => Key;
    public override string FundraiserName => Name;
    
    public override string Url(ICrowdfundingUrlBuilder urlBuilder) {
        return ViewEditFundraiserPage.Url(urlBuilder, Key);
    }
    
    protected override void PopulateFullText(StringBuilder sb) { }
}