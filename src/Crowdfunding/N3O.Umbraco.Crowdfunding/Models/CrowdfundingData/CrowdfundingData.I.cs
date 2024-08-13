using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfundingData {
    Guid CampaignId { get; }
    Guid? TeamId { get; }
    Guid FundraiserId { get; }
    string FundraiserUrl { get; }
    string Comment { get; }
    bool Anonymous { get; }
}