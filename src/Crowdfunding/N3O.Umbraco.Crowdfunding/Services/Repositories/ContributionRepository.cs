using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.TaxRelief;
using NodaTime;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding;

public class ContributionRepository : IContributionRepository {
    private readonly List<Contribution> _toCommit = new();
    
    private readonly IForexConverter _forexConverter;
    private readonly ILocalClock _localClock;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingUrlBuilder _urlBuilder;
    private readonly IJsonProvider _jsonProvider;
    private readonly ITaxReliefSchemeAccessor _taxReliefSchemeAccessor;

    public ContributionRepository(IUmbracoDatabaseFactory umbracoDatabaseFactory,
                                  IContentLocator contentLocator,
                                  ICrowdfundingUrlBuilder urlBuilder,
                                  IForexConverter forexConverter,
                                  ILocalClock localClock,
                                  IJsonProvider jsonProvider,
                                  ITaxReliefSchemeAccessor taxReliefSchemeAccessor) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _contentLocator = contentLocator;
        _urlBuilder = urlBuilder;
        _forexConverter = forexConverter;
        _localClock = localClock;
        _jsonProvider = jsonProvider;
        _taxReliefSchemeAccessor = taxReliefSchemeAccessor;
    }

    public async Task AddOnlineContributionAsync(string checkoutReference,
                                                 Instant timestamp,
                                                 ICrowdfunderData crowdfunderData,
                                                 string email,
                                                 string name,
                                                 bool taxRelief,
                                                 GivingType givingType,
                                                 Allocation allocation) {
        var contribution = await GetContributionAsync(ContributionType.Online,
                                                      checkoutReference,
                                                      timestamp,
                                                      crowdfunderData,
                                                      email,
                                                      name,
                                                      taxRelief,
                                                      givingType,
                                                      allocation);
        
        _toCommit.Add(contribution);
    }

    public async Task CommitAsync() {
        if (_toCommit.Any()) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                await db.InsertBatchAsync(_toCommit);
            }
        }
        
        _toCommit.Clear();
    }

    public async Task<IReadOnlyList<Contribution>> FindByCampaignAsync(params Guid[] campaignIds) {
        return await FindContributionsAsync(Sql.Builder.Where($"{nameof(Contribution.CampaignId)} IN (@0)", campaignIds));
    }

    public async Task<IReadOnlyList<Contribution>> FindByFundraiserAsync(params Guid[] fundraiserIds) {
        return await FindContributionsAsync(Sql.Builder.Where($"{nameof(Contribution.FundraiserId)} IN (@0)", fundraiserIds));
    }

    private async Task<Contribution> GetContributionAsync(ContributionType type,
                                                          string transactionReference,
                                                          Instant timestamp,
                                                          ICrowdfunderData crowdfunderData,
                                                          string email,
                                                          string name,
                                                          bool taxRelief,
                                                          GivingType givingType,
                                                          Allocation allocation) {
        var crowdfunder = GetCrowdfunderContent(crowdfunderData);

        var taxReliefScheme = _taxReliefSchemeAccessor.GetScheme();
        var date = timestamp.InZone(_localClock.GetZone()).Date;
        var baseForex = (await _forexConverter.QuoteToBase()
                                              .UsingRateOn(date)
                                              .FromCurrency(allocation.Value.Currency)
                                              .ConvertAsync(allocation.Value.Amount));
        var crowdfunderForex = (await _forexConverter.BaseToQuote()
                                                     .UsingRateOn(date)
                                                     .ToCurrency(crowdfunder.Currency)
                                                     .ConvertAsync(baseForex.Base.Amount));
        
        var contribution = new Contribution();
        contribution.Timestamp = timestamp.ToDateTimeUtc();
        contribution.Date = date.ToDateTimeUnspecified();
        contribution.CampaignId = crowdfunder.CampaignId;
        contribution.CampaignName = crowdfunder.CampaignName;
        contribution.TeamId = crowdfunder.TeamId;
        contribution.TeamName = crowdfunder.TeamName;
        contribution.FundraiserId = crowdfunder.FundraiserId;
        contribution.FundraiserUrl = crowdfunder.FundraiserId.HasValue() ? crowdfunder.Url(_urlBuilder) : null;
        contribution.TransactionReference = transactionReference;
        contribution.GivingTypeId = givingType.Id;
        contribution.CurrencyCode = allocation.Value.Currency.Code;
        contribution.QuoteAmount = allocation.Value.Amount;
        contribution.BaseAmount = baseForex.Base.Amount;
        contribution.CrowdfunderAmount = crowdfunderForex.Base.Amount;
        contribution.TaxReliefQuoteAmount = taxRelief ? taxReliefScheme.GetAllowanceValue(date, allocation.Value).Amount : 0m;
        contribution.TaxReliefBaseAmount = taxRelief ? taxReliefScheme.GetAllowanceValue(date, baseForex.Base).Amount : 0m;
        contribution.TaxReliefCrowdfunderAmount = taxRelief ? taxReliefScheme.GetAllowanceValue(date, crowdfunderForex.Base).Amount : 0m;
        contribution.Anonymous = crowdfunderData.Anonymous;
        contribution.Name = name;
        contribution.Email = email;
        contribution.Comment = crowdfunderData.Comment;
        contribution.Status = ContributionStatuses.Visible;
        contribution.ContributionType = (int) type;
        contribution.AllocationSummary = allocation.Summary;
        contribution.FundDimension1 = allocation.FundDimensions.Dimension1.Name;
        contribution.FundDimension2 = allocation.FundDimensions.Dimension2.Name;
        contribution.FundDimension3 = allocation.FundDimensions.Dimension3.Name;
        contribution.FundDimension4 = allocation.FundDimensions.Dimension4.Name;
        contribution.AllocationJson = _jsonProvider.SerializeObject(allocation);

        return contribution;
    }

    private ICrowdfunderContent GetCrowdfunderContent(ICrowdfunderData crowdfunderData) {
        if (crowdfunderData.Type == CrowdfunderTypes.Campaign) {
            return _contentLocator.ById<CampaignContent>(crowdfunderData.Id);
        } else if (crowdfunderData.Type == CrowdfunderTypes.Fundraiser) {
            return _contentLocator.ById<FundraiserContent>(crowdfunderData.Id);
        } else {
            throw UnrecognisedValueException.For(crowdfunderData.Type);
        }
    }

    private async Task<IReadOnlyList<Contribution>> FindContributionsAsync(Sql whereClause) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var sql = new Sql($"SELECT * FROM {CrowdfundingConstants.Tables.Contributions.Name}").Append(whereClause);
            
            return await db.FetchAsync<Contribution>(sql);
        }
    }
}