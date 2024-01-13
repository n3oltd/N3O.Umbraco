using N3O.Umbraco.Crowdfunding.Konstrukt;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingContributionRepository : ICrowdfundingWriter {
    private readonly List<CrowdfundingContribution> _crowdfundingContributions = new();
    
    private readonly IContentService _contentService;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;

    public CrowdfundingContributionRepository(IUmbracoDatabaseFactory umbracoDatabaseFactory, IContentService contentService) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _contentService = contentService;
    }

    public async Task AppendAsync(string checkoutReference,
                                  Instant timestamp,
                                  Guid? campaignId,
                                  Guid? teamId,
                                  string teamName,
                                  Guid? pageId,
                                  bool isAnonymous,
                                  string pageUrl,
                                  string comment,
                                  Money value,
                                  string email) {
        var campaign = _contentService.GetById(campaignId.Value);
        
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
        crowdfundingContribution.CampaignId = campaignId.Value;
        crowdfundingContribution.CampaignName = campaign?.Name ?? "name";
        crowdfundingContribution.PageId = pageId.Value;
        crowdfundingContribution.PageUrl = pageUrl;
        crowdfundingContribution.TeamId = teamId;
        crowdfundingContribution.TeamName = teamName;
        
        _crowdfundingContributions.Add(crowdfundingContribution);
    }

    public async Task<int> CommitAsync() {
        if (_crowdfundingContributions.Any()) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                foreach (var crowdfundingContribution in _crowdfundingContributions) {
                    await db.InsertAsync(crowdfundingContribution);
                }
            }
        }

        var count = _crowdfundingContributions.Count;
        
        _crowdfundingContributions.Clear();

        return count;
    }

    public class Strings : CodeStrings {
        public string CannotSpecifyContentIdAndReplacementCriteria => "Content ID and replacement criteria cannot both be specified";
        public string ContainerNotFound_1 => $"Container with ID {"{0}".Quote()} not found";
        public string ContentTypeNotFound_1 => $"Content type with alias {"{0}".Quote()} not found";
        public string MultipleContentMatched_1 => $"More than one content found for {"{0}".Quote()}";
        public string NoContentMatched_1 => $"No content found for {"{0}".Quote()}";
    }
}