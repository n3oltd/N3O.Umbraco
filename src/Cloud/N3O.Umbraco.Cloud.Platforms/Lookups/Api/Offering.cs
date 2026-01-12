using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class Offering : ContentOrPublishedLookup {
    public Offering(string id, string name, Guid? contentId, string campaignId) : base(id, name, contentId) {
        CampaignId = campaignId;
    }
    
    public string CampaignId { get; }
}