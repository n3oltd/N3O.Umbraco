using System;

namespace N3O.Umbraco.Crm.Models;

public interface IFundraiser : ICrowdfunder {
    Guid CampaignId { get; }
}