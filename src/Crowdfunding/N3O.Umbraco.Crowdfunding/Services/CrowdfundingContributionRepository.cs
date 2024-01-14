using N3O.Umbraco.Crowdfunding.Konstrukt;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingContributionRepository : ICrowdfundingContributionRepository {
    private readonly List<CrowdfundingContribution> _crowdfundingContributions = new();
    
    private readonly IContentService _contentService;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IJsonProvider _jsonProvider;

    public CrowdfundingContributionRepository(IUmbracoDatabaseFactory umbracoDatabaseFactory, IContentService contentService, IJsonProvider jsonProvider) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _contentService = contentService;
        _jsonProvider = jsonProvider;
    }

    public async Task AppendAsync(string checkoutReference,
                                  Instant timestamp,
                                  Guid? campaignId,
                                  Guid? teamId,
                                  Guid? pageId,
                                  bool isAnonymous,
                                  string pageUrl,
                                  string comment,
                                  Money value,
                                  string email,
                                  Allocation allocation) {
        var campaign = _contentService.GetById(campaignId.GetValueOrThrow());
        var team = _contentService.GetById(teamId.GetValueOrThrow());

        var crowdfundingContribution = GetCrowdfundingContribution(checkoutReference,
                                                                   timestamp,
                                                                   pageId,
                                                                   campaign,
                                                                   team,
                                                                   isAnonymous,
                                                                   pageUrl,
                                                                   comment,
                                                                   value,
                                                                   email,
                                                                   allocation);
        
        _crowdfundingContributions.Add(crowdfundingContribution);
    }

    public async Task CommitAsync() {
        if (_crowdfundingContributions.Any()) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                foreach (var crowdfundingContribution in _crowdfundingContributions) {
                    await db.InsertAsync(crowdfundingContribution);
                }
            }
        }
        
        _crowdfundingContributions.Clear();
    }

    private CrowdfundingContribution GetCrowdfundingContribution(string checkoutReference,
                                                                 Instant timestamp,
                                                                 Guid? pageId,
                                                                 IContent campaign,
                                                                 IContent team,
                                                                 bool isAnonymous,
                                                                 string pageUrl,
                                                                 string comment,
                                                                 Money value,
                                                                 string email,
                                                                 Allocation allocation) {
        var crowdfundingContribution = new CrowdfundingContribution();
        crowdfundingContribution.CheckoutReference = checkoutReference;
        crowdfundingContribution.Name = checkoutReference;
        crowdfundingContribution.Status = "Complete";
        crowdfundingContribution.Anonymous = isAnonymous;
        crowdfundingContribution.Comment = comment;
        crowdfundingContribution.Currency = value.Currency.Code;
        crowdfundingContribution.Email = email ?? "talha.malik@n3o.ltd";
        crowdfundingContribution.Timestamp = timestamp.ToDateTimeUtc();
        crowdfundingContribution.BaseAmount = value.Amount;
        crowdfundingContribution.QuoteAmount = value.Amount;
        crowdfundingContribution.BaseTaxReliefAmount = value.Amount;
        crowdfundingContribution.QuoteTaxReliefAmount = value.Amount;
        crowdfundingContribution.CampaignId = campaign.Key;
        crowdfundingContribution.CampaignName = campaign?.Name ?? "name";
        crowdfundingContribution.PageId = pageId.GetValueOrThrow();
        crowdfundingContribution.PageUrl = pageUrl;
        crowdfundingContribution.TeamId = team.Key;
        crowdfundingContribution.TeamName = team.Name;
        crowdfundingContribution.Allocation = _jsonProvider.SerializeObject(allocation);

        return crowdfundingContribution;
    }
}