﻿using N3O.Umbraco.Crm.Lookups;
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

    public async Task AddOrUpdateAsync(ICrowdfunderContent content) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var existing = db.SingleOrDefault<Crowdfunder>($"WHERE {nameof(Crowdfunder.ContentKey)} = @0", content.Key);

            if (existing.HasValue()) {
                await UpdateCrowdfunderAsync(existing, content);

                await db.UpdateAsync(existing);
            } else {
                var crowdfunder = await CreateCrowdfunderAsync(content);

                await db.InsertAsync(crowdfunder);
            }
        }
    }
    
    public async Task<IReadOnlyList<Crowdfunder>> FilterByTagAsync(string tag) {
        var sql = Sql.Builder
                     .Append("SELECT *")
                     .From($"{CrowdfundingConstants.Tables.Crowdfunders.Name}")
                     .Where($"{nameof(Crowdfunder.Tags)} LIKE %{TagsSeperator}{tag}{TagsSeperator}%");
        
        var crowdfunders = await FetchCrowdfundersAsync(sql);

        return crowdfunders;
    }

    public async Task<IReadOnlyList<string>> GetActiveTagsAsync() {
        var sql = Sql.Builder
                     .Select($"{nameof(Crowdfunder.Tags)}")
                     .From($"{CrowdfundingConstants.Tables.Crowdfunders.Name}")
                     .Where($"{nameof(Crowdfunder.StatusKey)} = {(int) CrowdfunderStatuses.Active.Key}");

        var crowdfunders = await FetchCrowdfundersAsync(sql);
            
        var tags = crowdfunders.Select(x => x.Tags.Split(TagsSeperator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                               .SelectMany(x => x).Distinct();

        return tags.ToList();
    }

    public void QueueRecalculateContributionsTotal(Guid id, CrowdfunderType type) {
        CrowdfunderDebouncer.Debounce(id, type, EnqueueRecalculateContributionsTotal);
    }

    public async Task RecalculateContributionsTotalAsync(Guid id) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var contributionsQuoteSumSql = Sql.Builder
                                              .Append($"SELECT SUM({nameof(Contribution.QuoteAmount)})")
                                              .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                                              .Where($"{nameof(Crowdfunder.Id)} = {id.ToString()}");

            var contributionsBaseSumSql = Sql.Builder
                                             .Append($"SELECT SUM({nameof(Contribution.BaseAmount)})")
                                             .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                                             .Where($"{nameof(Crowdfunder.Id)} = {id.ToString()}");

            var sql = Sql.Builder
                         .Append($"UPDATE {CrowdfundingConstants.Tables.Crowdfunders.Name} SET {nameof(Crowdfunder.ContributionsTotalQuote)} = ({contributionsQuoteSumSql.SQL}), {nameof(Crowdfunder.ContributionsTotalBase)} = ({contributionsBaseSumSql.SQL})")
                         .Where($"{nameof(Crowdfunder.Id)} = {id.ToString()}");

            await db.ExecuteAsync(sql);
        }
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

    public async Task UpdateNonDonationsTotalAsync(Guid id, ForexMoney nonDonationsForex) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var crowdfunder = db.Single<Crowdfunder>($"WHERE {nameof(Crowdfunder.ContentKey)} = @0", id);

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
    
    private async Task<Crowdfunder> CreateCrowdfunderAsync(ICrowdfunderContent crowdfunderContent) {
        var crowdfunder = new Crowdfunder();
        crowdfunder.CurrencyCode = crowdfunderContent.Currency.Code;
        crowdfunder.Type = (int) crowdfunderContent.Type.Key;
        crowdfunder.ContentKey = crowdfunderContent.Key;
        
        if (crowdfunderContent.Type == CrowdfunderTypes.Fundraiser) {
            crowdfunder.Owner = ((FundraiserContent) crowdfunderContent).Owner.Name;
        }

        await UpdateCrowdfunderAsync(crowdfunder, crowdfunderContent);

        return crowdfunder;
    }
    
    private async Task UpdateCrowdfunderAsync(Crowdfunder crowdfunder, ICrowdfunderContent crowdfunderContent) {
        var baseForex = await _forexConverter.QuoteToBase()
                                             .FromCurrency(crowdfunderContent.Currency)
                                             // TODO
                                             //.UsingRateOn()
                                             .ConvertAsync(crowdfunderContent.Goals.Sum(x => x.Amount));
        
        crowdfunder.Name = crowdfunderContent.Name;
        crowdfunder.Url = crowdfunderContent.Url(_crowdfundingUrlBuilder);
        crowdfunder.StatusKey = (int?) crowdfunderContent.Status?.Key;
        crowdfunder.FullText = crowdfunderContent.GetFullText();
        crowdfunder.GoalsTotalQuote = crowdfunderContent.Goals.Sum(x => x.Amount);
        crowdfunder.GoalsTotalBase = baseForex.Base.Amount;
        crowdfunder.Tags = $"{TagsSeperator}{crowdfunderContent.Tags.Select(x => x.Name).Join(TagsSeperator)}{TagsSeperator}";
    }
    
    private void EnqueueRecalculateContributionsTotal(Guid id, CrowdfunderType type) {
        _backgroundJob.Enqueue<RecalculateContributionTotalsCommand>($"{nameof(RecalculateContributionTotalsCommand).Replace("Command", "")} {id.ToString()}",
                                                                     p => { 
                                                                         p.Add<ContentId>(id.ToString());
                                                                         p.Add<CrowdfunderTypeId>(type.Id);
                                                                     });
    }
}