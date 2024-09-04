using N3O.Umbraco.Crowdfunding.Entities;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding;

public class OnlineContributionRepository : IOnlineContributionRepository {
    private readonly List<OnlineContribution> _toCommit = new();
    
    private readonly IContentService _contentService;
    private readonly IForexConverter _forexConverter;
    private readonly ILocalClock _localClock;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IJsonProvider _jsonProvider;

    public OnlineContributionRepository(IUmbracoDatabaseFactory umbracoDatabaseFactory,
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
        var OnlineContribution = await GetOnlineContributionAsync(checkoutReference,
                                                                              timestamp,
                                                                              crowdfundingData,
                                                                              email,
                                                                              taxRelief,
                                                                              givingType,
                                                                              allocation);
        
        _toCommit.Add(OnlineContribution);
    }

    public async Task CommitAsync() {
        if (_toCommit.Any()) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                await db.InsertBatchAsync(_toCommit);
            }
        }
        
        _toCommit.Clear();
    }

    public async Task<IEnumerable<OnlineContribution>> FindByCampaignAsync(params Guid[] campaignIds) {
        return await FindContributionsAsync(Sql.Builder.Where($"{nameof(OnlineContribution.CampaignId)} IN (@0)", campaignIds));
    }

    public async Task<IEnumerable<OnlineContribution>> FindByFundraiserAsync(params Guid[] fundraiserIds) {
        return await FindContributionsAsync(Sql.Builder.Where($"{nameof(OnlineContribution.CampaignId)} IN (@0)", fundraiserIds));
    }

    private async Task<OnlineContribution> GetOnlineContributionAsync(string checkoutReference,
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
        
        var OnlineContribution = new OnlineContribution();
        OnlineContribution.Timestamp = timestamp.ToDateTimeUtc();
        OnlineContribution.CampaignId = campaign.Key;
        OnlineContribution.CampaignName = campaign.Name;
        OnlineContribution.TeamId = team?.Key;
        OnlineContribution.TeamName = team?.Name;
        OnlineContribution.FundraiserId = crowdfundingData.FundraiserId;
        OnlineContribution.FundraiserUrl = crowdfundingData.FundraiserUrl;
        OnlineContribution.CheckoutReference = checkoutReference;
        OnlineContribution.GivingTypeId = givingType.Id;
        OnlineContribution.CurrencyCode = value.Quote.Currency.Code;
        OnlineContribution.QuoteAmount = value.Quote.Amount;
        OnlineContribution.BaseAmount = value.Base.Amount;
        OnlineContribution.TaxRelief = taxRelief;
        OnlineContribution.Anonymous = crowdfundingData.Anonymous;
        OnlineContribution.Name = checkoutReference;
        OnlineContribution.Email = email;
        OnlineContribution.Comment = crowdfundingData.Comment;
        OnlineContribution.Status = OnlineContributionStatuses.Visible;
        OnlineContribution.AllocationJson = _jsonProvider.SerializeObject(allocation);

        return OnlineContribution;
    }
    
    private async Task<IEnumerable<OnlineContribution>> FindContributionsAsync(Sql whereClause) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var sql = new Sql($"SELECT * FROM {CrowdfundingConstants.Tables.OnlineContributions.Name}").Append(whereClause);
            
            return await db.FetchAsync<OnlineContribution>(sql);
        }
    }
}