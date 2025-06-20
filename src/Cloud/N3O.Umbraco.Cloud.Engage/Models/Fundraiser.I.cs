using System;

namespace N3O.Umbraco.Cloud.Engage.Models;

public interface IFundraiser : ICrowdfunder {
    Guid CampaignId { get; }
}