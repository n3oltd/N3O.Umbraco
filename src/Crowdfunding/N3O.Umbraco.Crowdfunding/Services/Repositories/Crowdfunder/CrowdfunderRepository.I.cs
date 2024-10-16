using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfunderRepository {
    Task AddOrUpdateAsync(ICrowdfunderContent crowdfunderContent);
    Task<IReadOnlyList<Entities.Crowdfunder>> FilterByTagAsync(string tag);
    Task<IReadOnlyList<string>> GetActiveTagsAsync();
    void QueueRecalculateContributionsTotal(Guid id, CrowdfunderType type);
    Task RecalculateContributionsTotalAsync(Guid id);
    Task<IReadOnlyList<Entities.Crowdfunder>> SearchAsync(CrowdfunderType type, string query);
    Task UpdateNonDonationsTotalAsync(Guid id, ForexMoney value);
}