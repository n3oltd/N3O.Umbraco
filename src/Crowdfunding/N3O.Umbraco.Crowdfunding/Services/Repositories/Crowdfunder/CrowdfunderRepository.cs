using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Scheduler;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfunderRepository : ICrowdfunderRepository {
    private const string TagsSeperator = "þ";
    
    private readonly IBackgroundJob _backgroundJob;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;
    private readonly IForexConverter _forexConverter;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;

    public CrowdfunderRepository(IBackgroundJob backgroundJob,
                                 ICrowdfundingUrlBuilder crowdfundingUrlBuilder,
                                 IForexConverter forexConverter,
                                 IUmbracoDatabaseFactory umbracoDatabaseFactory) {
        _backgroundJob = backgroundJob;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
        _forexConverter = forexConverter;
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
    }

    public async Task AddOrUpdateCrowdfunderAsync(ICrowdfunderContent crowdfunderContent) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var existing = db.SingleOrDefault<Crowdfunder>($"WHERE {nameof(Crowdfunder.ContentKey)} = @0", crowdfunderContent.Key);

            if (existing.HasValue()) {
                await PopulateCrowdfunderPropertiesAsync(existing, crowdfunderContent);

                await db.UpdateAsync(existing);
            } else {
                var crowdfunder = await GetCrowdfunderAsync(crowdfunderContent);

                await db.InsertAsync(crowdfunder);
            }
        }
    }
    
    public async Task<IReadOnlyList<Crowdfunder>> FilterByTagAsync(string tag) {
        var sql = Sql.Builder
                     .Append("Select *")
                     .From($"{CrowdfundingConstants.Tables.Crowdfunders.Name}")
                     .Where($"{nameof(Crowdfunder.Tags)} Like %{tag}%");
        
        var crowdfunders = await FetchCrowdfundersAsync(sql);

        return crowdfunders;
    }

    public async Task<IReadOnlyList<string>> GetActiveTagsAsync() {
        var sql = Sql.Builder
                     .Select($"{nameof(Crowdfunder.Tags)}")
                     .From($"{CrowdfundingConstants.Tables.Crowdfunders.Name}")
                     .Where($"{nameof(Crowdfunder.StatusKey)} = {(int) CrowdfunderStatuses.Active.Key}");

        var crowdfunders = await FetchCrowdfundersAsync(sql);
            
        var tags = crowdfunders.Select(x => x.Tags.Split(TagsSeperator)).SelectMany(x => x).Distinct();

        return tags.ToList();
    }

    public Task RefreshCrowdfunderStatistics(Guid crowdfunderId, CrowdfunderType crowdfunderType) {
        CrowdfunderDebouncer.Enqueue(crowdfunderId, crowdfunderType, EnqueueUpdateCrowdfunderStatisticsCommand);

        return Task.CompletedTask;
    }
    
    public async Task<IReadOnlyList<Crowdfunder>> SearchAsync(CrowdfunderType type, string query) {
        var sql = Sql.Builder
                     .Append("Select *")
                     .From($"{CrowdfundingConstants.Tables.Crowdfunders.Name}");

        if (type.HasValue()) {
            sql.Where($"{nameof(Crowdfunder.Type)} = {(int) type.Key} AND {nameof(Crowdfunder.FullText)} Like %{query}%");
        } else {
            sql.Where($"{nameof(Crowdfunder.FullText)} Like %{query}%");
        }
        
        var crowdfunders = await FetchCrowdfundersAsync(sql);

        return crowdfunders;
    }

    public async Task UpdateNonDonationsTotalAsync(Guid crowdfunderId, ForexMoney nonDonationsForex) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var crowdfunder = db.Single<Crowdfunder>($"WHERE {nameof(Crowdfunder.ContentKey)} = @0", crowdfunderId);

            crowdfunder.NonDonationsTotalBase = nonDonationsForex.Base.Amount;
            crowdfunder.NonDonationsTotalQuote = nonDonationsForex.Quote.Amount;

            await db.UpdateAsync(crowdfunder);
        }
    }

    private async Task<IReadOnlyList<Crowdfunder>> FetchCrowdfundersAsync(Sql sql) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var crowdfunders = await db.FetchAsync<Crowdfunder>(sql);
            
            return crowdfunders;
        }
    }
    
    private async Task<Crowdfunder> GetCrowdfunderAsync(ICrowdfunderContent crowdfunderContent) {
        var crowdfunder = new Crowdfunder();
        crowdfunder.CurrencyCode = crowdfunderContent.Currency.Code;
        crowdfunder.Type = (int) crowdfunderContent.Type.Key;
        crowdfunder.ContentKey = crowdfunderContent.Key;
        
        if (crowdfunderContent.Type == CrowdfunderTypes.Fundraiser) {
            crowdfunder.Owner = ((FundraiserContent) crowdfunderContent).Owner.Name;
        }

        await PopulateCrowdfunderPropertiesAsync(crowdfunder, crowdfunderContent);

        return crowdfunder;
    }
    
    private async Task PopulateCrowdfunderPropertiesAsync(Crowdfunder crowdfunder,
                                                          ICrowdfunderContent crowdfunderContent) {
        var baseForex = await _forexConverter.QuoteToBase()
                                             .FromCurrency(crowdfunderContent.Currency)
                                             .ConvertAsync(crowdfunderContent.Goals.Sum(x => x.Amount));
        
        crowdfunder.Name = crowdfunderContent.Name;
        crowdfunder.Url = crowdfunderContent.Url(_crowdfundingUrlBuilder);
        crowdfunder.StatusKey = (int?) crowdfunderContent.Status?.Key;
        crowdfunder.FullText = crowdfunderContent.GetFullText();
        crowdfunder.GoalsTotalQuote = crowdfunderContent.Goals.Sum(x => x.Amount);
        crowdfunder.GoalsTotalBase = baseForex.Base.Amount;
        crowdfunder.Tags = crowdfunderContent.Tags.Select(x => x.Name).Join(TagsSeperator);
    }
    
    private void EnqueueUpdateCrowdfunderStatisticsCommand(Guid crowdfunderId, CrowdfunderType crowdfunderType) {
        _backgroundJob.Enqueue<UpdateCrowdfunderStatisticsCommand>($"{nameof(UpdateCrowdfunderStatisticsCommand).Replace("Command", "")} {crowdfunderId.ToString()}",
                                                                   p => { 
                                                                       p.Add<ContentId>(crowdfunderId.ToString());
                                                                       p.Add<CrowdfunderTypeId>(crowdfunderType.Id);
                                                                   });
    }
}