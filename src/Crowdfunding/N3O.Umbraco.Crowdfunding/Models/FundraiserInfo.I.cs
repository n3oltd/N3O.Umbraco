using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface IFundraiserInfo {
    Guid CampaignId { get; }
    Guid? TeamId { get; }
}