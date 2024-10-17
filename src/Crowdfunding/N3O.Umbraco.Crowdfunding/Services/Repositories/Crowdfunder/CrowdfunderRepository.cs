using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Cropper;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Extensions;
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
    private readonly IImageCropper _imageCropper;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;

    public CrowdfunderRepository(IBackgroundJob backgroundJob,
                                 ICrowdfundingUrlBuilder crowdfundingUrlBuilder,
                                 IForexConverter forexConverter,
                                 IImageCropper imageCropper,
                                 IUmbracoDatabaseFactory umbracoDatabaseFactory) {
        _backgroundJob = backgroundJob;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
        _forexConverter = forexConverter;
        _imageCropper = imageCropper;
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
    }

    public async Task AddOrUpdateAsync(ICrowdfunderContent crowdfunderContent) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var existing = db.SingleOrDefault<Crowdfunder>($"WHERE {nameof(Crowdfunder.ContentKey)} = @0", crowdfunderContent.Key);

            if (existing.HasValue()) {
                await UpdateCrowdfunderAsync(existing, crowdfunderContent);

                await db.UpdateAsync(existing);
            } else {
                var crowdfunder = await CreateCrowdfunderAsync(crowdfunderContent);

                await db.InsertAsync(crowdfunder);
            }
        }
    }
    
    public async Task<IReadOnlyList<Crowdfunder>> FilterByTagAsync(string tag) {
        var crowdfunders = await FetchCrowdfundersAsync(sql => sql.Select("*"),
                                                        sql => sql.Where($"{nameof(Crowdfunder.Tags)} LIKE %{TagsSeperator}{tag}{TagsSeperator}%"));

        return crowdfunders;
    }

    public async Task<IReadOnlyList<string>> GetActiveTagsAsync() {
        var crowdfunders = await FetchCrowdfundersAsync(sql => sql.Select($"{nameof(Crowdfunder.Tags)}"),
                                                        sql => sql.Where($"{nameof(Crowdfunder.StatusKey)} = {CrowdfunderStatuses.Active.Key}"));
            
        var tags = crowdfunders.Select(x => x.Tags.Split(TagsSeperator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                               .SelectMany(x => x).Distinct();

        return tags.ToList();
    }

    public async Task<IReadOnlyList<Crowdfunder>> GetAlmostCompleteFundraisersAsync(int? take = null) {
        return await FetchCrowdfundersAsync(sql => sql.SelectTop("*", take),
                                            sql => sql.Where($"{nameof(Crowdfunder.Type)} = {(int) CrowdfunderTypes.Fundraiser.Key} AND {nameof(Crowdfunder.StatusKey)} = {CrowdfunderStatuses.Active.Key}"),
                                            //LeftToRaiseQuote = GoalsTotal - NonDonationsQuote + ContributionsTotal
                                            sql => sql.Append($"ORDER BY {nameof(Crowdfunder.LeftToRaiseQuote)}"));
    }

    public async Task<IReadOnlyList<Crowdfunder>> GetFeaturedCampaignsAsync(int? take = null) {
        return await FetchCrowdfundersAsync(sql => sql.SelectTop("*", take),
                                            sql => sql.Where($"{nameof(Crowdfunder.Type)} = {(int) CrowdfunderTypes.Campaign.Key} AND {nameof(Crowdfunder.StatusKey)} = {CrowdfunderStatuses.Active.Key}"),
                                            sql => sql.Append($"ORDER BY {nameof(Crowdfunder.CreatedAt)} DESC"));
    }

    public async Task<IReadOnlyList<Crowdfunder>> GetNewFundraisersAsync(int? take = null) {
        return await FetchCrowdfundersAsync(sql => sql.SelectTop("*", take),
                                            sql => sql.Where($"{nameof(Crowdfunder.Type)} = {(int) CrowdfunderTypes.Fundraiser.Key} AND {nameof(Crowdfunder.StatusKey)} = {CrowdfunderStatuses.Active.Key}"),
                                            sql => sql.Append($"ORDER BY {nameof(Crowdfunder.CreatedAt)} DESC"));
    }

    public void QueueRecalculateContributionsTotal(Guid id, CrowdfunderType type) {
        CrowdfunderDebouncer.Debounce(id, type, EnqueueRecalculateContributionsTotal);
    }

    public async Task RecalculateContributionsTotalAsync(Guid id) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var contributionsQuoteSumSql = Sql.Builder
                                              .Select($"SUM({nameof(Contribution.QuoteAmount)})")
                                              .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                                              .Where($"{nameof(Crowdfunder.Id)} = {id.ToString()}");

            var contributionsBaseSumSql = Sql.Builder
                                             .Select($"SUM({nameof(Contribution.BaseAmount)})")
                                             .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                                             .Where($"{nameof(Crowdfunder.Id)} = {id.ToString()}");

            var sql = Sql.Builder
                         .Append($"UPDATE {CrowdfundingConstants.Tables.Crowdfunders.Name} SET {nameof(Crowdfunder.ContributionsTotalQuote)} = ({contributionsQuoteSumSql.SQL}), {nameof(Crowdfunder.ContributionsTotalBase)} = ({contributionsBaseSumSql.SQL})")
                         .Where($"{nameof(Crowdfunder.Id)} = {id.ToString()}");

            await db.ExecuteAsync(sql);
        }
    }

    public async Task<IReadOnlyList<Crowdfunder>> SearchAsync(CrowdfunderType type, string query) {
        Action<Sql> where;

        if (type.HasValue()) {
            where = sql => sql.Where($"{nameof(Crowdfunder.Type)} = {(int) type.Key} AND {nameof(Crowdfunder.FullText)} Like %{query}%");
        } else {
            where = sql => sql.Where($"{nameof(Crowdfunder.FullText)} Like %{query}%");
        }

        
        var crowdfunders = await FetchCrowdfundersAsync(sql => sql.Select("*"), where);

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

    private async Task<IReadOnlyList<Crowdfunder>> FetchCrowdfundersAsync(Action<Sql> select,
                                                                          Action<Sql> where,
                                                                          Action<Sql> orderBy = null) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var sql = Sql.Builder;
            select(sql);
            sql.From($"{CrowdfundingConstants.Tables.Crowdfunders.Name}");
            where(sql);
            orderBy?.Invoke(sql);
            
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
        var heroImage = crowdfunderContent.HeroImages.First().Image;
        
        var baseForex = await _forexConverter.QuoteToBase()
                                             .FromCurrency(crowdfunderContent.Currency)
                                             .UsingRateOn(crowdfunderContent.CreatedDated.ToLocalDate())
                                             .ConvertAsync(crowdfunderContent.Goals.Sum(x => x.Amount));
        
        crowdfunder.Name = crowdfunderContent.Name;
        crowdfunder.Url = crowdfunderContent.Url(_crowdfundingUrlBuilder);
        crowdfunder.StatusKey = (int?) crowdfunderContent.Status?.Key;
        crowdfunder.FullText = crowdfunderContent.GetFullText();
        crowdfunder.GoalsTotalQuote = crowdfunderContent.Goals.Sum(x => x.Amount);
        crowdfunder.GoalsTotalBase = baseForex.Base.Amount;
        crowdfunder.Tags = $"{TagsSeperator}{crowdfunderContent.Tags.Select(x => x.Name).Join(TagsSeperator)}{TagsSeperator}";
        crowdfunder.JumboImage = await _imageCropper.GetImagePathAsync(heroImage, ImageCropperExtensions.JumboCropDefinition);
        crowdfunder.TallImage = await _imageCropper.GetImagePathAsync(heroImage, ImageCropperExtensions.TallCropDefinition);
        crowdfunder.WideImage = await _imageCropper.GetImagePathAsync(heroImage, ImageCropperExtensions.WideCropDefinition);
    }
    
    private void EnqueueRecalculateContributionsTotal(Guid id, CrowdfunderType type) {
        _backgroundJob.Enqueue<RecalculateContributionTotalsCommand>($"{nameof(RecalculateContributionTotalsCommand).Replace("Command", "")} {id.ToString()}",
                                                                     p => { 
                                                                         p.Add<ContentId>(id.ToString());
                                                                         p.Add<CrowdfunderTypeId>(type.Id);
                                                                     });
    }
}