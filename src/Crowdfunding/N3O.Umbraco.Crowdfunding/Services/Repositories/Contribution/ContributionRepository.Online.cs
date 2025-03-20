using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using NodaTime;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public partial class ContributionRepository {
    public async Task AddOnlineContributionAsync(string transactionReference,
                                                 Instant timestamp,
                                                 ICrowdfunderData crowdfunderData,
                                                 string email,
                                                 string name,
                                                 bool taxRelief,
                                                 string fundDimension1,
                                                 string fundDimension2,
                                                 string fundDimension3,
                                                 string fundDimension4,
                                                 Money value,
                                                 GivingType givingType,
                                                 string summary,
                                                 object allocation) {
        var contribution = await GetContributionAsync(ContributionType.Online,
                                                      crowdfunderData.Type,
                                                      crowdfunderData.Id,
                                                      transactionReference,
                                                      timestamp,
                                                      email,
                                                      name,
                                                      crowdfunderData.Anonymous,
                                                      crowdfunderData.Comment,
                                                      taxRelief,
                                                      fundDimension1,
                                                      fundDimension2,
                                                      fundDimension3,
                                                      fundDimension4,
                                                      givingType,
                                                      value,
                                                      summary,
                                                      allocation);
        
        _toCommit.Add(contribution);
    }
}