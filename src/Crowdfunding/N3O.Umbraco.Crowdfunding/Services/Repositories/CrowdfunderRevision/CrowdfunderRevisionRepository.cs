using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Localization;
using NPoco;
using System;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfunderRevisionRepository : ICrowdfunderRevisionRepository {
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;
    private readonly IForexConverter _forexConverter;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly ILocalClock _localClock;

    public CrowdfunderRevisionRepository(ICrowdfundingUrlBuilder crowdfundingUrlBuilder,
                                         IForexConverter forexConverter,
                                         IUmbracoDatabaseFactory umbracoDatabaseFactory,
                                         ILocalClock localClock) {
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
        _forexConverter = forexConverter;
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _localClock = localClock;
    }
    
    public async Task AddOrUpdateAsync(ICrowdfunderContent crowdfunderContent, int revision) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var existing = await GetLatestRevisionAsync(db, crowdfunderContent.Key);

            if (existing.HasValue()) {
                var wasActive = existing.ActiveTo == null;
                var isActive = crowdfunderContent.Status == CrowdfunderStatuses.Active;

                if (wasActive && !isActive) {
                    await DeactivateAsync(crowdfunderContent.Key);
                }
                
                await UpdateAsync(crowdfunderContent, revision);
            } else {
                await AddToDbAsync(db, crowdfunderContent, revision);
            }
        }
    }
    
    private async Task DeactivateAsync(Guid id) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var latestRevision = await GetLatestRevisionAsync(db, id);
            
            await DeactivateInDbAsync(db, latestRevision);
        }
    }

    private async Task UpdateAsync(ICrowdfunderContent crowdfunderContent, int revision) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var latestRevision = await GetLatestRevisionAsync(db, crowdfunderContent.Key);

            if (latestRevision.Name != crowdfunderContent.Name) {
                await UpdateRevisionNameAsync(db, crowdfunderContent.Key, crowdfunderContent.Name);
            }
        
            if (latestRevision.GoalsTotalQuote != crowdfunderContent.Goals.Sum(x => x.Amount)) {
                await DeactivateInDbAsync(db, latestRevision);
                await AddToDbAsync(db, crowdfunderContent, revision);
            }
        }
    }
    
    private async Task AddToDbAsync(IUmbracoDatabase db, ICrowdfunderContent crowdfunderContent, int revision) {
        var crowdfunderRevision = await GetRevisionAsync(crowdfunderContent, revision);

        await db.InsertAsync(crowdfunderRevision);
    }

    private async Task DeactivateInDbAsync(IUmbracoDatabase db, CrowdfunderRevision crowdfunderRevision) {
        crowdfunderRevision.ActiveTo = _localClock.GetUtcNow();

        await db.UpdateAsync(crowdfunderRevision);
    }

    private async Task<CrowdfunderRevision> GetLatestRevisionAsync(IUmbracoDatabase db, Guid contentKey) {
        var query = Sql.Builder
                       .Append($"SELECT TOP(1) * FROM {CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                       .Where($"{nameof(Crowdfunder.ContentKey)} = @0", contentKey)
                       .Append($"ORDER BY {nameof(CrowdfunderRevision.ContentRevision)} DESC");
        
        var result = await db.FirstOrDefaultAsync<CrowdfunderRevision>(query);
        
        return result;
    }

    private async Task UpdateRevisionNameAsync(IUmbracoDatabase db, Guid id, string name) {
        var sql = Sql.Builder
                     .Append($"UPDATE {CrowdfundingConstants.Tables.CrowdfunderRevisions.Name}")
                     .Append($"SET {nameof(CrowdfunderRevision.Name)} = @0", name)
                     .Where($"{nameof(CrowdfunderRevision.ContentKey)} = @0", id);

        await db.ExecuteAsync(sql);
    }

    private async Task<CrowdfunderRevision> GetRevisionAsync(ICrowdfunderContent crowdfunder, int revision) {
        var goalsTotalQuoteAmount = crowdfunder.Goals.Sum(x => x.Amount);

        var goalsTotalForex = await _forexConverter.QuoteToBase()
                                                   .FromCurrency(crowdfunder.Currency)
                                                   //TODO
                                                   //.UsingRateOn()
                                                   .ConvertAsync(goalsTotalQuoteAmount);
        
        var crowdfunderRevision = new CrowdfunderRevision();
        crowdfunderRevision.Name = crowdfunder.Name;
        crowdfunderRevision.ContentRevision = revision;
        crowdfunderRevision.Type = (int) crowdfunder.Type.Key;
        crowdfunderRevision.Url = crowdfunder.Url(_crowdfundingUrlBuilder);
        crowdfunderRevision.ContentKey = crowdfunder.Key;
        crowdfunderRevision.CurrencyCode = crowdfunder.Currency.Code;
        crowdfunderRevision.GoalsTotalQuote = goalsTotalQuoteAmount;
        crowdfunderRevision.GoalsTotalBase = goalsTotalForex.Base.Amount;
        crowdfunderRevision.ActiveFrom = _localClock.GetUtcNow();

        return crowdfunderRevision;
    }
}