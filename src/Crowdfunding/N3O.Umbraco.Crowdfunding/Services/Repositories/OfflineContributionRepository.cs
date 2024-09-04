using N3O.Umbraco.Context;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.CrowdFunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using NPoco;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.CrowdFunding.Services;

public class OfflineContributionRepository : IOfflineContributionRepository {
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
    private readonly IForexConverter _forexConverter;

    public OfflineContributionRepository(IUmbracoDatabaseFactory umbracoDatabaseFactory,
                                         IBaseCurrencyAccessor baseCurrencyAccessor,
                                         IForexConverter forexConverter) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _baseCurrencyAccessor = baseCurrencyAccessor;
        _forexConverter = forexConverter;
    }

    public async Task AddOrUpdateAsync(ICrowdfunderInfo crowdfunder, Money total, int count) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var forexTotal = await _forexConverter.QuoteToBase()
                                                  .FromCurrency(total.Currency)
                                                  .ConvertAsync(total.Amount);
            
            await db.ExecuteAsync($"DELETE FROM {Tables.OfflineContributions.Name} WHERE {nameof(OfflineContribution.CrowdfunderId)} = '{crowdfunder.Id}'");

            var offlineContribution = new OfflineContribution();
            offlineContribution.CrowdfunderId = crowdfunder.Id;
            offlineContribution.Total = forexTotal.Base.Amount;
            offlineContribution.Count = count;

            if (crowdfunder.Type == CrowdfunderTypes.Campaign) {
                offlineContribution.CampaignId = crowdfunder.Id;    
            } else if (crowdfunder.Type == CrowdfunderTypes.Campaign) {
                offlineContribution.CampaignId = crowdfunder.Fundraiser.CampaignId;
                offlineContribution.TeamId = crowdfunder.Fundraiser.TeamId;
            } else {
                throw UnrecognisedValueException.For(crowdfunder.Type);
            }
            
            await db.InsertAsync(offlineContribution);
        }
    }

    public async Task<(Money Total, int Count)> SumByCampaignIdAsync(Guid campaignId) {
        return await SumByAsync(Sql.Builder.Where($"{nameof(OfflineContribution.CampaignId)} = {campaignId}"));
    }

    public async Task<(Money Total, int Count)> SumByFundraiserIdAsync(Guid fundraiserId) {
        return await SumByAsync(Sql.Builder.Where($"{nameof(OfflineContribution.FundraiserId)} = {fundraiserId}"));
    }
    
    private async Task<(Money Total, int Count)> SumByAsync(Sql whereClause) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var sql = new Sql($"SELECT * FROM {Tables.OfflineContributions.Name}").Append(whereClause);
            
            var rows = db.QueryAsync<OfflineContribution>(sql);
            var total = 0m;
            var count = 0;
            
            await foreach (var row in rows) {
                total += row.Total;
                count += row.Count;
            }

            return (new Money(total, _baseCurrencyAccessor.GetBaseCurrency()), count);
        }
    }
}