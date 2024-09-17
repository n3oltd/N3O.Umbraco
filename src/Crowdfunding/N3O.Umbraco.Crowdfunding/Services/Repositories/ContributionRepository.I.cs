using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface IContributionRepository {
    Task AddOnlineContributionAsync(string checkoutReference,
                                    Instant timestamp,
                                    ICrowdfunderData crowdfunderData,
                                    string email,
                                    bool taxRelief,
                                    GivingType givingType,
                                    Allocation allocation);

    Task CommitAsync();
    Task<IEnumerable<Contribution>> FindByCampaignAsync(params Guid[] campaignIds);
    Task<IEnumerable<Contribution>> FindByFundraiserAsync(params Guid[] fundraiserIds);
}