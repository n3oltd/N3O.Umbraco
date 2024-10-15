using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfunderRepository {
    Task AddOrUpdateCrowdfunderAsync(ICrowdfunderContent crowdfunderContent);
    Task<IReadOnlyList<Entities.Crowdfunder>> FilterByTagAsync(string tag);
    Task<IReadOnlyList<string>> GetActiveTagsAsync();
    Task RefreshCrowdfunderStatistics(Guid crowdfunderId, CrowdfunderType keyType);
    Task<IReadOnlyList<Entities.Crowdfunder>> SearchAsync(CrowdfunderType type, string query);
    Task UpdateNonDonationsTotalAsync(Guid crowdfunderId, ForexMoney value);
}