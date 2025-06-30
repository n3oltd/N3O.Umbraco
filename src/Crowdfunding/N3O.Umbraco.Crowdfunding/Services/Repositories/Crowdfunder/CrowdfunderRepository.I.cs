using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Financial;
using NPoco;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfunderRepository {
    Task AddOrUpdateAsync(ICrowdfunderContent crowdfunderContent);
    Task<Crowdfunder> FindCrowdfunderByIdAsync(Guid id);
    Task<IReadOnlyList<Crowdfunder>> FindFundraisersAsync(string text);
    Task<IReadOnlyList<Crowdfunder>> FindFundraisersWithTagAsync(string tag);
    Task<IReadOnlyList<Crowdfunder>> GetActiveCampaignsAsync(int? take = null);
    Task<IReadOnlyList<string>> GetActiveFundraiserTagsAsync();
    Task<IReadOnlyList<Crowdfunder>> GetAlmostCompleteFundraisersAsync(int? take = null);
    Task<IReadOnlyList<Crowdfunder>> GetNewFundraisersAsync(int? take = null);
    Task<Page<Crowdfunder>> GetCrowdfundersPageAsync(CrowdfunderType type, int currentPage, int itemsPerPage);
    Task RecalculateContributionsTotalAsync(Guid id);
    Task<IReadOnlyList<Crowdfunder>> SearchAsync(CrowdfunderType type, string query);
    Task UpdateNonDonationsTotalAsync(Guid id, ForexMoney value);
}