using N3O.Umbraco.Crowdfunding.Lookups;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

public class FundraiserContent : CrowdfunderContent<FundraiserContent> {
    public string Account => GetValue(x => x.Account);
    public string Slug => GetValue(x => x.Slug);
    public CampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public IPublishedContent Owner => GetPickedAs(x => x.Owner);
    public FundraiserStatus Status => GetStaticLookupByNameAs(x => x.Status);
}