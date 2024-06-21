using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfundingData {
    Guid CampaignId { get; }
    Guid? TeamId { get; }
    Guid PageId { get; }
    string PageUrl { get; }
    string Comment { get; }
    bool Anonymous { get; }
}