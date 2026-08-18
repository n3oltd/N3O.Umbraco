using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Platforms.Commands;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Handlers;

public class PopulateCrowdfundersHandler : IRequestHandler<PopulateCrowdfundersCommand, None, None> {
    private readonly IContentEditor _contentEditor;
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;
    private readonly ILogger<PopulateCrowdfundersHandler> _logger;

    public PopulateCrowdfundersHandler(IContentEditor contentEditor,
                                       IContentService contentService,
                                       IContentTypeService contentTypeService,
                                       ILogger<PopulateCrowdfundersHandler> logger) {
        _contentEditor = contentEditor;
        _contentService = contentService;
        _contentTypeService = contentTypeService;
        _logger = logger;
    }

    public Task<None> Handle(PopulateCrowdfundersCommand req, CancellationToken cancellationToken) {
        var crowdfunders = GetOrCreateCrowdfunders();

        if (crowdfunders == null) {
            return Task.FromResult(None.Empty);
        }

        var existing = _contentService.GetCrowdfundersByCampaign(_contentTypeService);

        foreach (var campaign in _contentService.GetCrowdfundingEnabledCampaigns(_contentTypeService)) {
            if (existing.ContainsKey(campaign.Key)) {
                continue;
            }

            CreateCrowdfunder(crowdfunders.Key, campaign);
        }

        return Task.FromResult(None.Empty);
    }

    private void CreateCrowdfunder(Guid crowdfundersId, IContent campaign) {
        var contentPublisher = _contentEditor.New(campaign.Name,
                                                 crowdfundersId,
                                                 PlatformsConstants.Crowdfunders.Crowdfunder.Alias);

        contentPublisher.Content
                        .Property<ContentPickerPropertyBuilder>(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.Campaign)
                        .SetContent(campaign.Key);

        var templateSourceAlias = campaign.FirstAliasWithValue(CrowdfunderContentSources.All);

        if (templateSourceAlias != null) {
            var templateValue = campaign.GetValue(templateSourceAlias);

            contentPublisher.Content
                            .Property<RawPropertyBuilder>(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.PageTemplate)
                            .Set(templateValue);

            contentPublisher.Content
                            .Property<RawPropertyBuilder>(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.Page)
                            .Set(templateValue);
        }

        var heroImageSourceAlias =
            campaign.FirstAliasWithValue([PlatformsConstants.CrowdfundingCampaign.Properties.HeroImage,
                                          PlatformsConstants.Campaigns.Properties.HeroImage]);

        if (heroImageSourceAlias != null) {
            var heroImageValue = campaign.GetValue(heroImageSourceAlias);

            contentPublisher.Content
                            .Property<RawPropertyBuilder>(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.PageTemplateHeroImage)
                            .Set(heroImageValue);

            contentPublisher.Content
                            .Property<RawPropertyBuilder>(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.PageHeroImage)
                            .Set(heroImageValue);
        }

        foreach (var alias in CrowdfunderContentSources.CarriedOver) {
            var carriedOverValue = campaign.GetValue(alias);

            if (carriedOverValue != null) {
                contentPublisher.Content
                                .Property<RawPropertyBuilder>(CrowdfunderContentSources.PageAlias(alias))
                                .Set(carriedOverValue);

                contentPublisher.Content
                                .Property<RawPropertyBuilder>(CrowdfunderContentSources.PageTemplateAlias(alias))
                                .Set(carriedOverValue);
            }
        }

        if (campaign.Published) {
            contentPublisher.SaveAndPublish();
        } else {
            contentPublisher.SaveUnpublished();
        }

        _logger.LogInformation("Created crowdfunder for campaign {CampaignId}, published {Published}",
                               campaign.Key,
                               campaign.Published);
    }

    private IContent GetFirstOfType(string contentTypeAlias) {
        var contentType = _contentTypeService.Get(contentTypeAlias);

        if (contentType == null) {
            return null;
        }

        return _contentService.GetPagedOfType(contentType.Id, 0, 1, out _, null).FirstOrDefault();
    }

    private IContent GetOrCreateCrowdfunders() {
        if (_contentTypeService.Get(PlatformsConstants.Crowdfunders.Alias) == null) {
            _logger.LogInformation("No {Alias} document type found, skipping crowdfunder creation",
                                   PlatformsConstants.Crowdfunders.Alias);

            return null;
        }

        var crowdfunders = GetFirstOfType(PlatformsConstants.Crowdfunders.Alias);

        if (crowdfunders != null) {
            return crowdfunders;
        }

        var platforms = GetFirstOfType(PlatformsConstants.Platforms.Alias);

        if (platforms == null) {
            _logger.LogInformation("No {Alias} node found, skipping crowdfunder creation",
                                   PlatformsConstants.Platforms.Alias);

            return null;
        }

        var contentPublisher = _contentEditor.New("Crowdfunders",
                                                 platforms.Key,
                                                 PlatformsConstants.Crowdfunders.Alias);

        var result = contentPublisher.SaveAndPublish();

        _logger.LogInformation("Created crowdfunders container under {PlatformsId}", platforms.Key);

        return result.Content;
    }
}
