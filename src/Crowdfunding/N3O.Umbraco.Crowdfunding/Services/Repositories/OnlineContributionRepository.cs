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
                               ICrowdfunderData crowdfunderData,
                               string email,
                               bool taxRelief,
                               GivingType givingType,
                               Allocation allocation) {
        var onlineContribution = await GetOnlineContributionAsync(checkoutReference,
                                                                              timestamp,
                                                                              crowdfunderData,
                                                                              email,
                                                                              taxRelief,
                                                                              givingType,
                                                                              allocation);
        
        _toCommit.Add(onlineContribution);
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
        return await FindContributionsAsync(Sql.Builder.Where($"{nameof(OnlineContribution.FundraiserId)} IN (@0)", fundraiserIds));
    }

    private async Task<OnlineContribution> GetOnlineContributionAsync(string checkoutReference,
                                                                                  Instant timestamp,
                                                                                  ICrowdfunderData crowdfunderData,
                                                                                  string email,
                                                                                  bool taxRelief,
                                                                                  GivingType givingType,
                                                                                  Allocation allocation) {
        var campaign = _contentService.GetById(crowdfunderData.CampaignId);
        var team = crowdfunderData.TeamId.IfNotNull(x => _contentService.GetById(x));
        var date = timestamp.InZone(_localClock.GetZone()).Date;
        var value = await _forexConverter.QuoteToBase()
                                         .UsingRateOn(date)
                                         .FromCurrency(allocation.Value.Currency)
                                         .ConvertAsync(allocation.Value.Amount);
        
        var onlineContribution = new OnlineContribution();
        onlineContribution.Timestamp = timestamp.ToDateTimeUtc();
        onlineContribution.CampaignId = campaign.Key;
        onlineContribution.CampaignName = campaign.Name;
        onlineContribution.TeamId = team?.Key;
        onlineContribution.TeamName = team?.Name;
        onlineContribution.FundraiserId = crowdfunderData.FundraiserId;
        onlineContribution.FundraiserUrl = crowdfunderData.FundraiserUrl;
        onlineContribution.CheckoutReference = checkoutReference;
        onlineContribution.GivingTypeId = givingType.Id;
        onlineContribution.CurrencyCode = value.Quote.Currency.Code;
        onlineContribution.QuoteAmount = value.Quote.Amount;
        onlineContribution.BaseAmount = value.Base.Amount;
        onlineContribution.TaxRelief = taxRelief;
        onlineContribution.Anonymous = crowdfunderData.Anonymous;
        onlineContribution.Name = checkoutReference;
        onlineContribution.Email = email;
        onlineContribution.Comment = crowdfunderData.Comment;
        onlineContribution.Status = OnlineContributionStatuses.Visible;
        onlineContribution.AllocationJson = _jsonProvider.SerializeObject(allocation);

        return onlineContribution;
    }
    
    private async Task<IEnumerable<OnlineContribution>> FindContributionsAsync(Sql whereClause) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var sql = new Sql($"SELECT * FROM {CrowdfundingConstants.Tables.OnlineContributions.Name}").Append(whereClause);
            
            return await db.FetchAsync<OnlineContribution>(sql);
        }
    }
}