using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations.Lookups;
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

public partial class ContributionRepository : IContributionRepository {
    private readonly List<Contribution> _toCommit = [];
    
    private readonly IForexConverter _forexConverter;
    private readonly ILocalClock _localClock;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingUrlBuilder _urlBuilder;
    private readonly IJsonProvider _jsonProvider;
    private readonly ITaxReliefSchemeAccessor _taxReliefSchemeAccessor;
    private Guid? _removeOfflineContributionsForCrowdfunderId;

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

    public async Task CommitAsync() {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            if (_removeOfflineContributionsForCrowdfunderId.HasValue()) {
                await db.DeleteManyAsync<Contribution>()
                        .Where(x => x.CrowdfunderId == _removeOfflineContributionsForCrowdfunderId.GetValueOrThrow() &&
                                    x.ContributionType == (int) ContributionType.Offline)
                        .Execute();
            }
            
            if (_toCommit.Any()) {
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

    public async Task UpdateCrowdfunderNameAsync(ICrowdfunderContent crowdfunderContent,
                                                 CrowdfunderType crowdfunderType) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var sql = Sql.Builder.Append($"UPDATE {CrowdfundingConstants.Tables.Contributions.Name}");

            if (crowdfunderType == CrowdfunderTypes.Campaign) {
                sql.Append($"SET {nameof(Contribution.CampaignName)} = @0", crowdfunderContent.Name)
                   .Where($"{nameof(Contribution.CampaignId)} = (@0)", crowdfunderContent.Key);
            } else {
                sql.Append($"SET {nameof(Contribution.FundraiserName)} = @0", crowdfunderContent.Name)
                   .Where($"{nameof(Contribution.FundraiserId)} = (@0)", crowdfunderContent.Key);
            }
            
            await db.ExecuteAsync(sql);
        }
    }

    private async Task<Contribution> GetContributionAsync(ContributionType type,
                                                          CrowdfunderType crowdfunderType,
                                                          Guid crowdfunderId,
                                                          string transactionReference,
                                                          Instant timestamp,
                                                          string email,
                                                          string name,
                                                          bool anonymous,
                                                          string comment,
                                                          bool taxRelief,
                                                          string fundDimension1,
                                                          string fundDimension2,
                                                          string fundDimension3,
                                                          string fundDimension4,
                                                          GivingType givingType,
                                                          Money value,
                                                          string summary,
                                                          object allocation) {
        var crowdfunder = _contentLocator.GetCrowdfunderContent(crowdfunderId, crowdfunderType);

        var taxReliefScheme = _taxReliefSchemeAccessor.GetScheme();
        var date = timestamp.InZone(_localClock.GetZone()).Date;
        var baseForex = (await _forexConverter.QuoteToBase()
                                              .FromCurrency(value.Currency)
                                              .ConvertAsync(value.Amount));
        var crowdfunderForex = (await _forexConverter.BaseToQuote()
                                                     .ToCurrency(crowdfunder.Currency)
                                                     .ConvertAsync(baseForex.Base.Amount));
        
        var contribution = new Contribution();
        contribution.Timestamp = timestamp.ToDateTimeUtc();
        contribution.Date = date.ToDateTimeUnspecified();
        contribution.CrowdfunderId = crowdfunderId;
        contribution.CampaignId = crowdfunder.CampaignId;
        contribution.CampaignName = crowdfunder.CampaignName;
        contribution.FundraiserName = crowdfunder.FundraiserName;
        contribution.TeamId = crowdfunder.TeamId;
        contribution.TeamName = crowdfunder.TeamName;
        contribution.FundraiserId = crowdfunder.FundraiserId;
        contribution.CrowdfunderUrl = crowdfunder.Url(_urlBuilder);
        contribution.TransactionReference = transactionReference;
        contribution.GivingTypeId = givingType.Id;
        contribution.CurrencyCode = value.Currency.Code;
        contribution.QuoteAmount = value.Amount;
        contribution.BaseAmount = baseForex.Base.Amount;
        contribution.CrowdfunderAmount = crowdfunderForex.Base.Amount;
        contribution.TaxReliefQuoteAmount = taxRelief ? taxReliefScheme.GetAllowanceValue(date, value).Amount : 0m;
        contribution.TaxReliefBaseAmount = taxRelief ? taxReliefScheme.GetAllowanceValue(date, baseForex.Base).Amount : 0m;
        contribution.TaxReliefCrowdfunderAmount = taxRelief ? taxReliefScheme.GetAllowanceValue(date, crowdfunderForex.Base).Amount : 0m;
        contribution.Anonymous = anonymous;
        contribution.Name = name;
        contribution.Email = email;
        contribution.Comment = comment;
        contribution.Status = ContributionStatuses.Visible;
        contribution.ContributionType = (int) type;
        contribution.AllocationSummary = summary;
        contribution.FundDimension1 = fundDimension1;
        contribution.FundDimension2 = fundDimension2;
        contribution.FundDimension3 = fundDimension3;
        contribution.FundDimension4 = fundDimension4;
        contribution.AllocationJson = allocation.IfNotNull(x => _jsonProvider.SerializeObject(x));

        return contribution;
    }

    private async Task<IReadOnlyList<Contribution>> FindContributionsAsync(Sql whereClause) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var sql = new Sql($"SELECT * FROM {CrowdfundingConstants.Tables.Contributions.Name}").Append(whereClause);
            
            return await db.FetchAsync<Contribution>(sql);
        }
    }
}