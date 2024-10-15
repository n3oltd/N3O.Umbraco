using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using NodaTime;
using NodaTime.Extensions;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public partial class ContributionRepository {
    public async Task AddOfflineContributionAsync(string transactionReference,
                                                  LocalDate date,
                                                  ICrowdfunderInfo crowdfunderInfo,
                                                  string email,
                                                  string name,
                                                  bool anonymous,
                                                  bool taxRelief,
                                                  string fundDimension1,
                                                  string fundDimension2,
                                                  string fundDimension3,
                                                  string fundDimension4,
                                                  Money value,
                                                  GivingType givingType) {
        var timestamp = date.ToDateTimeUnspecified().ToInstant();
        
        var contribution = await GetContributionAsync(ContributionType.Offline,
                                                      crowdfunderInfo.Type,
                                                      crowdfunderInfo.Id,
                                                      transactionReference,
                                                      timestamp,
                                                      email,
                                                      name,
                                                      anonymous,
                                                      null,
                                                      taxRelief,
                                                      fundDimension1,
                                                      fundDimension2,
                                                      fundDimension3,
                                                      fundDimension4,
                                                      givingType,
                                                      value,
                                                      null);
        
        _toCommit.Add(contribution);
    }
    
    public void DeleteOfflineContributions(Guid crowdfunderId) {
        _removeOfflineContributionsForCrowdfunderId = crowdfunderId;
    }
}