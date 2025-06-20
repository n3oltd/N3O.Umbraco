using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public interface IContributionRepository {
    Task AddOnlineContributionAsync(string transactionReference,
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
                                    object allocation);
    
    Task AddOfflineContributionAsync(string transactionReference,
                                     LocalDate localDate,
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
                                     string summary);

    Task CommitAsync();
    void DeleteOfflineContributions(Guid crowdfunderId);
    Task<IReadOnlyList<Contribution>> FindByCampaignAsync(params Guid[] campaignIds);
    Task<IReadOnlyList<Contribution>> FindByFundraiserAsync(params Guid[] fundraiserIds);
    Task UpdateCrowdfunderNameAsync(ICrowdfunderContent crowdfunderContent, CrowdfunderType crowdfunderType);
}