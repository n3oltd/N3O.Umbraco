using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using NodaTime;
using NPoco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingContributionRepository {
    Task AddAsync(string checkoutReference,
                  Instant timestamp,
                  ICrowdfundingData crowdfundingData,
                  string email,
                  bool taxRelief,
                  GivingType givingType,
                  Allocation allocation);

    Task CommitAsync();
    Task<IEnumerable<CrowdfundingContribution>> GetAllContributionsAsync();
    Task<IEnumerable<CrowdfundingContribution>> FetchContributionsAsync(Sql sql);
}