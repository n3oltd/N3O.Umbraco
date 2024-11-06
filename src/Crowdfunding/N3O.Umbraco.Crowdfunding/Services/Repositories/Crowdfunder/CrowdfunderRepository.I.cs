using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfunderRepository {
    Task AddOrUpdateAsync(ICrowdfunderContent crowdfunderContent);
    Task<IReadOnlyList<Crowdfunder>> FindFundraisersAsync(string text);
    Task<IReadOnlyList<Crowdfunder>> FindFundraisersWithTagAsync(string tag);
    Task<IReadOnlyList<string>> GetActiveFundraiserTagsAsync();
    Task<IReadOnlyList<Crowdfunder>> GetAlmostCompleteFundraisersAsync(int? take = null);
    Task<IReadOnlyList<Crowdfunder>> GetFeaturedCampaignsAsync(int? take = null);
    Task<IReadOnlyList<Crowdfunder>> GetNewFundraisersAsync(int? take = null);
    Task RecalculateContributionsTotalAsync(Guid id);
    Task<IReadOnlyList<Crowdfunder>> SearchAsync(CrowdfunderType type, string query);
    Task UpdateNonDonationsTotalAsync(Guid id, ForexMoney value);
}