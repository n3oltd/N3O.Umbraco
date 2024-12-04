using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using NodaTime;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public partial class ContributionRepository {
    public async Task AddOnlineContributionAsync(string checkoutReference,
                                                 Instant timestamp,
                                                 ICrowdfunderData crowdfunderData,
                                                 string email,
                                                 string name,
                                                 bool taxRelief,
                                                 GivingType givingType,
                                                 Allocation allocation) {
        var contribution = await GetContributionAsync(ContributionType.Online,
                                                      crowdfunderData.Type,
                                                      crowdfunderData.Id,
                                                      checkoutReference,
                                                      timestamp,
                                                      email,
                                                      name,
                                                      crowdfunderData.Anonymous,
                                                      crowdfunderData.Comment,
                                                      taxRelief,
                                                      allocation.FundDimensions.Dimension1?.Name,
                                                      allocation.FundDimensions.Dimension2?.Name,
                                                      allocation.FundDimensions.Dimension3?.Name,
                                                      allocation.FundDimensions.Dimension4?.Name,
                                                      givingType,
                                                      allocation.Value,
                                                      allocation);
        
        _toCommit.Add(contribution);
    }
}