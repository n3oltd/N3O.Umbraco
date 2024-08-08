using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using NodaTime;
using NPoco;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingContributionRepository : ICrowdfundingContributionRepository {
    private readonly List<CrowdfundingContribution> _crowdfundingContributions = new();
    
    private readonly IContentService _contentService;
    private readonly IForexConverter _forexConverter;
    private readonly ILocalClock _localClock;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IJsonProvider _jsonProvider;

    public CrowdfundingContributionRepository(IUmbracoDatabaseFactory umbracoDatabaseFactory,
                                              IContentService contentService,
                                              IForexConverter forexConverter,
                                              ILocalClock localClock,
                                              IJsonProvider jsonProvider) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _contentService = contentService;
        _forexConverter = forexConverter;
        _localClock = localClock;
        _jsonProvider = jsonProvider;
    }

    public async Task AddAsync(string checkoutReference,
                               Instant timestamp,
                               ICrowdfundingData crowdfundingData,
                               string email,
                               bool taxRelief,
                               GivingType givingType,
                               Allocation allocation) {
        var crowdfundingContribution = await GetCrowdfundingContributionAsync(checkoutReference,
                                                                              timestamp,
                                                                              crowdfundingData,
                                                                              email,
                                                                              taxRelief,
                                                                              givingType,
                                                                              allocation);
        
        _crowdfundingContributions.Add(crowdfundingContribution);
    }

    public async Task CommitAsync() {
        if (_crowdfundingContributions.Any()) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                await db.InsertBatchAsync(_crowdfundingContributions);
            }
        }
        
        _crowdfundingContributions.Clear();
    }

    public async Task<IEnumerable<CrowdfundingContribution>> GetAllContributionsAsync() {
        var query = $"SELECT * FROM {CrowdfundingConstants.Tables.CrowdfundingContributions.Name}";
        
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            return await db.FetchAsync<CrowdfundingContribution>(query);
        }
    }

    public async Task<IEnumerable<CrowdfundingContribution>> FetchContributionsAsync(Sql sql) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            return await db.FetchAsync<CrowdfundingContribution>(sql);
        }
    }

    private async Task<CrowdfundingContribution> GetCrowdfundingContributionAsync(string checkoutReference,
                                                                                  Instant timestamp,
                                                                                  ICrowdfundingData crowdfundingData,
                                                                                  string email,
                                                                                  bool taxRelief,
                                                                                  GivingType givingType,
                                                                                  Allocation allocation) {
        var campaign = _contentService.GetById(crowdfundingData.CampaignId);
        var team = crowdfundingData.TeamId.IfNotNull(x => _contentService.GetById(x));
        var date = timestamp.InZone(_localClock.GetZone()).Date;
        var value = await _forexConverter.QuoteToBase()
                                         .UsingRateOn(date)
                                         .FromCurrency(allocation.Value.Currency)
                                         .ConvertAsync(allocation.Value.Amount);
        
        var crowdfundingContribution = new CrowdfundingContribution();
        crowdfundingContribution.Timestamp = timestamp.ToDateTimeUtc();
        crowdfundingContribution.CampaignId = campaign.Key;
        crowdfundingContribution.CampaignName = campaign.Name;
        crowdfundingContribution.TeamId = team?.Key;
        crowdfundingContribution.TeamName = team?.Name;
        crowdfundingContribution.PageId = crowdfundingData.PageId;
        crowdfundingContribution.PageUrl = crowdfundingData.PageUrl;
        crowdfundingContribution.CheckoutReference = checkoutReference;
        crowdfundingContribution.GivingTypeId = givingType.Id;
        crowdfundingContribution.CurrencyCode = value.Quote.Currency.Code;
        crowdfundingContribution.QuoteAmount = value.Quote.Amount;
        crowdfundingContribution.BaseAmount = value.Base.Amount;
        crowdfundingContribution.TaxRelief = taxRelief;
        crowdfundingContribution.Anonymous = crowdfundingData.Anonymous;
        crowdfundingContribution.Name = checkoutReference;
        crowdfundingContribution.Email = email;
        crowdfundingContribution.Comment = crowdfundingData.Comment;
        crowdfundingContribution.Status = CrowdfundingContributionStatuses.Visible;
        crowdfundingContribution.AllocationJson = _jsonProvider.SerializeObject(allocation);

        return crowdfundingContribution;
    }
}