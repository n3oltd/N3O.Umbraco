using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Localization;
using NodaTime;
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
                                                  GivingType givingType,
                                                  string summary) {
        var timestamp = date.ToDateTimeUnspecified().InTimezone(Timezones.Utc).ToInstant();
        
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
                                                      summary,
                                                      null);
        
        _toCommit.Add(contribution);
    }
    
    public void DeleteOfflineContributions(Guid crowdfunderId) {
        _removeOfflineContributionsForCrowdfunderId = crowdfunderId;
    }
}