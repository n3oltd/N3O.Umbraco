using N3O.Umbraco.Content;
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
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingUrlBuilder _urlBuilder;
    private readonly IForexConverter _forexConverter;
    private readonly IImageCropper _imageCropper;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;

    public CrowdfunderRepository(IBackgroundJob backgroundJob,
                                 IContentLocator contentLocator,
                                 ICrowdfundingUrlBuilder urlBuilder,
                                 IForexConverter forexConverter,
                                 IImageCropper imageCropper,
                                 IUmbracoDatabaseFactory umbracoDatabaseFactory) {
        _contentLocator = contentLocator;
        _backgroundJob = backgroundJob;
        _urlBuilder = urlBuilder;
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

    public async Task<IReadOnlyList<Crowdfunder>> FindFundraisersAsync(string text) {
        var crowdfunders = await FetchCrowdfundersAsync(sql => sql.Select("*"),
                                                        sql => sql.Where($"{nameof(Crowdfunder.FullText)} LIKE '%{0}%'", text));

        return crowdfunders;
    }
    
    public async Task<IReadOnlyList<Crowdfunder>> FindFundraisersWithTagAsync(string tag) {
        var crowdfunders = await FetchCrowdfundersAsync(sql => sql.Select("*"),
                                                        sql => sql.Where($"{nameof(Crowdfunder.Tags)} LIKE '%{TagsSeperator}{tag}{TagsSeperator}%'"));

        return crowdfunders;
    }

    public async Task<IReadOnlyList<string>> GetActiveFundraiserTagsAsync() {
        var crowdfunders = await FetchCrowdfundersAsync(sql => sql.Select($"{nameof(Crowdfunder.Tags)}"),
                                                        sql => sql.Where($"{nameof(Crowdfunder.Type)} = {CrowdfunderTypes.Fundraiser.Key} AND {nameof(Crowdfunder.StatusKey)} = {CrowdfunderStatuses.Active.Key} AND {nameof(Crowdfunder.Tags)} IS NOT NULL"));
            
        var tags = crowdfunders.Select(x => x.Tags.Split(TagsSeperator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                               .SelectMany(x => x).Distinct();

        return tags.ToList();
    }

    public async Task<IReadOnlyList<Crowdfunder>> GetAlmostCompleteFundraisersAsync(int? take = null) {
        return await FetchCrowdfundersAsync(sql => sql.SelectTop("*", take),
                                            sql => sql.Where($"{nameof(Crowdfunder.Type)} = {CrowdfunderTypes.Fundraiser.Key} AND {nameof(Crowdfunder.StatusKey)} = {CrowdfunderStatuses.Active.Key}"),
                                            sql => sql.Append($"ORDER BY {nameof(Crowdfunder.LeftToRaiseQuote)}"));
    }

    public async Task<IReadOnlyList<Crowdfunder>> GetFeaturedCampaignsAsync(int? take = null) {
        return await FetchCrowdfundersAsync(sql => sql.SelectTop("*", take),
                                            sql => sql.Where($"{nameof(Crowdfunder.Type)} = {CrowdfunderTypes.Campaign.Key} AND {nameof(Crowdfunder.StatusKey)} = {CrowdfunderStatuses.Active.Key}"),
                                            sql => sql.Append($"ORDER BY {nameof(Crowdfunder.CreatedAt)} DESC"));
    }

    public async Task<IReadOnlyList<Crowdfunder>> GetNewFundraisersAsync(int? take = null) {
        return await FetchCrowdfundersAsync(sql => sql.SelectTop("*", take),
                                            sql => sql.Where($"{nameof(Crowdfunder.Type)} = {CrowdfunderTypes.Fundraiser.Key} AND {nameof(Crowdfunder.StatusKey)} = {CrowdfunderStatuses.Active.Key}"),
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
                                              .Where($"{nameof(Contribution.CrowdfunderId)} = '{id.ToString()}'");

            var contributionsBaseSumSql = Sql.Builder
                                             .Select($"SUM({nameof(Contribution.BaseAmount)})")
                                             .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                                             .Where($"{nameof(Contribution.CrowdfunderId)} = '{id.ToString()}'");

            var sql = Sql.Builder
                         .Append($"UPDATE {CrowdfundingConstants.Tables.Crowdfunders.Name} SET {nameof(Crowdfunder.ContributionsTotalQuote)} = ({contributionsQuoteSumSql.SQL}), {nameof(Crowdfunder.ContributionsTotalBase)} = ({contributionsBaseSumSql.SQL})")
                         .Where($"{nameof(Crowdfunder.ContentKey)} = '{id.ToString()}'");
            
            var updateLeftToRaiseSql = Sql.Builder
                                          .Append($"UPDATE {CrowdfundingConstants.Tables.Crowdfunders.Name} SET {nameof(Crowdfunder.LeftToRaiseBase)} = {nameof(Crowdfunder.GoalsTotalBase)} - ({nameof(Crowdfunder.ContributionsTotalBase)} + {nameof(Crowdfunder.NonDonationsTotalBase)})")
                                          .Append($", {nameof(Crowdfunder.LeftToRaiseQuote)} = {nameof(Crowdfunder.GoalsTotalQuote)} - ({nameof(Crowdfunder.ContributionsTotalQuote)} + {nameof(Crowdfunder.NonDonationsTotalQuote)})")
                                          .Where($"{nameof(Crowdfunder.ContentKey)} = '{id.ToString()}'");

            await db.ExecuteAsync(sql);
            await db.ExecuteAsync(updateLeftToRaiseSql);
        }
    }

    public async Task<IReadOnlyList<Crowdfunder>> SearchAsync(CrowdfunderType type, string query) {
        Action<Sql> where;

        if (type.HasValue()) {
            where = sql => sql.Where($"{nameof(Crowdfunder.Type)} = {type.Key} AND {nameof(Crowdfunder.FullText)} Like '%{query}%'");
        } else {
            where = sql => sql.Where($"{nameof(Crowdfunder.FullText)} Like '%{query}%'");
        }

        
        var crowdfunders = await FetchCrowdfundersAsync(sql => sql.Select("*"), where);

        return crowdfunders;
    }

    public async Task UpdateNonDonationsTotalAsync(Guid id, ForexMoney nonDonationsForex) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var crowdfunder = db.Single<Crowdfunder>($"WHERE {nameof(Crowdfunder.ContentKey)} = @0", id);

            crowdfunder.NonDonationsTotalBase = nonDonationsForex.Base.Amount;
            crowdfunder.NonDonationsTotalQuote = nonDonationsForex.Quote.Amount;
            
            crowdfunder.LeftToRaiseBase = crowdfunder.GoalsTotalBase - (crowdfunder.NonDonationsTotalBase + crowdfunder.ContributionsTotalBase);
            crowdfunder.LeftToRaiseQuote = crowdfunder.GoalsTotalQuote - (crowdfunder.NonDonationsTotalQuote + crowdfunder.ContributionsTotalQuote);

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
        crowdfunder.CreatedAt = crowdfunderContent.CreatedDate;
        
        
        PopulateOwnerInfo(crowdfunderContent, crowdfunder);
        await UpdateCrowdfunderAsync(crowdfunder, crowdfunderContent);

        return crowdfunder;
    }

    private void PopulateOwnerInfo(ICrowdfunderContent crowdfunderContent, Crowdfunder crowdfunder) {
        if (crowdfunderContent.Type == CrowdfunderTypes.Campaign) {
            var ourProfile = _contentLocator.Single<OurProfileSettingsContent>();
            
            crowdfunder.OwnerName = ourProfile.DisplayName;
            crowdfunder.OwnerProfilePicture = ourProfile.ProfileImage.Src;
        } else {
            crowdfunder.OwnerName = ((FundraiserContent) crowdfunderContent).Owner.Name;
            crowdfunder.OwnerEmail = ((FundraiserContent) crowdfunderContent).Owner.Email;
            crowdfunder.OwnerProfilePicture = ((FundraiserContent) crowdfunderContent).Owner.AvatarLink;
        }
    }
    
    private async Task UpdateCrowdfunderAsync(Crowdfunder crowdfunder, ICrowdfunderContent crowdfunderContent) {
        var heroImage = crowdfunderContent.HeroImages.First().Image;
        var tags = crowdfunderContent.Tags.OrEmpty().Select(x => x.Name).ToList();
        
        var baseForex = await _forexConverter.QuoteToBase()
                                             .FromCurrency(crowdfunderContent.Currency)
                                             .UsingRateOn(crowdfunderContent.CreatedDate.ToLocalDate())
                                             .ConvertAsync(crowdfunderContent.Goals.Sum(x => x.Amount));
        
        crowdfunder.Name = crowdfunderContent.Name;
        crowdfunder.Url = crowdfunderContent.Url(_urlBuilder);
        crowdfunder.StatusKey = (int?) crowdfunderContent.Status?.Key;
        crowdfunder.FullText = crowdfunderContent.GetFullText();
        crowdfunder.GoalsTotalQuote = crowdfunderContent.Goals.Sum(x => x.Amount);
        crowdfunder.GoalsTotalBase = baseForex.Base.Amount;
        crowdfunder.Tags = tags.HasAny() ? $"{TagsSeperator}{tags.Join(TagsSeperator)}{TagsSeperator}" : null;
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