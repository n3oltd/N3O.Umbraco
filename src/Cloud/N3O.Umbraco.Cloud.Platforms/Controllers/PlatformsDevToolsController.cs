using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Controllers;

[ApiDocument(PlatformsConstants.DevToolsApiName)]
public class PlatformsDevToolsController : BackofficeAuthorizedApiController {
    private const string CampaignsWebhookId = PlatformsConstants.WebhookIds.Campaigns;
    private const string OfferingsWebhookId = PlatformsConstants.WebhookIds.Offerings;

    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;
    private readonly ICloudUrl _cloudUrl;
    private readonly IBackgroundJob _backgroundJob;
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;
    private readonly ILogger<PlatformsDevToolsController> _logger;

    public PlatformsDevToolsController(IContentLocator contentLocator,
                                       IUmbracoMapper mapper,
                                       ICloudUrl cloudUrl,
                                       IBackgroundJob backgroundJob,
                                       ILogger<PlatformsDevToolsController> logger,
                                       IContentService contentService,
                                       IContentTypeService contentTypeService) {
        _contentLocator = contentLocator;
        _mapper = mapper;
        _cloudUrl = cloudUrl;
        _backgroundJob = backgroundJob;
        _logger = logger;
        _contentService = contentService;
        _contentTypeService = contentTypeService;
    }

    [HttpPost("webhooks/resend/campaigns/all")]
    public Task<ActionResult> ResendCampaignsWebhooks() {
        var campaigns = _contentLocator.All(x => x.IsComposedOf(AliasHelper<CampaignContent>.ContentTypeAlias()))
                                       .As<CampaignContent>();

        foreach (var campaign in campaigns) {
            var body = _mapper.Map<CampaignContent, CampaignWebhookBodyReq>(campaign);

            var req = new DispatchWebhookReq();
            req.Body = body;
            req.Url = _cloudUrl.ForWebhook(CampaignsWebhookId);

            _backgroundJob.EnqueueCommand<DispatchWebhookCommand, DispatchWebhookReq>(req, CampaignsWebhookId);
        }

        return Task.FromResult<ActionResult>(Ok());
    }

    [HttpPost("webhooks/resend/offerings/all")]
    public Task<ActionResult> ResendOfferingsWebhooks() {
        var offerings = _contentLocator.All(x => x.IsComposedOf(AliasHelper<OfferingContent>.ContentTypeAlias()))
                                       .As<OfferingContent>();

        foreach (var offering in offerings) {
            try {
                var parent = _contentService.GetParent(offering.Content()
                                                               .Id);

                if (parent?.Published == true) {
                    var body = _mapper.Map<OfferingContent, OfferingWebhookBodyReq>(offering);

                    var req = new DispatchWebhookReq();
                    req.Body = body;
                    req.Url = _cloudUrl.ForWebhook(OfferingsWebhookId);

                    _backgroundJob.EnqueueCommand<DispatchWebhookCommand, DispatchWebhookReq>(req, OfferingsWebhookId);
                }
            } catch (Exception ex) {
                _logger.LogError(ex,
                                 "There was an error while resending offering with id {offeringId}",
                                 offering.Content().Key.ToString());
            }
        }

        return Task.FromResult<ActionResult>(Ok());
    }
    
    [HttpPost("republish/campaigns")]
    public Task<ActionResult> RepublishAllCampaigns() {
        var campaigns = _contentLocator.All(x => x.IsComposedOf(AliasHelper<CampaignContent>.ContentTypeAlias()))
                                       .As<CampaignContent>();

        foreach (var campaign in campaigns) {
            try {
                var content = _contentService.GetById(campaign.Key);

                _contentService.SaveAndPublish(content);
            } catch (Exception ex) {
                _logger.LogError(ex,
                                 "There was an error publishing campaign with id {campaignId}",
                                 campaign.Content().Key.ToString());
            }
        }

        return Task.FromResult<ActionResult>(Ok());
    }
    
    [HttpPost("republish/offerings")]
    public Task<ActionResult> RepublishAllOfferings() {
        var offerings = _contentLocator.All(x => x.IsComposedOf(AliasHelper<OfferingContent>.ContentTypeAlias()))
                                       .As<OfferingContent>();

        foreach (var offering in offerings) {
            try {
                var content = _contentService.GetById(offering.Key);

                _contentService.SaveAndPublish(content);
            } catch (Exception ex) {
                _logger.LogError(ex,
                                 "There was an error publishing offering with id {offeringId}",
                                 offering.Content().Key.ToString());
            }
        }

        return Task.FromResult<ActionResult>(Ok());
    }

    [HttpGet("crowdfunders/migration/status")]
    public Task<ActionResult<CrowdfunderMigrationStatusRes>> GetCrowdfunderMigrationStatus() {
        var campaigns = EvaluateEnabledCampaigns();

        var summary = new CrowdfunderMigrationSummaryRes();
        summary.EnabledCampaignsCount = campaigns.Count;
        summary.FallbackSourceCount = campaigns.Count(x => x.UsedFallbackSource);
        summary.NotReadyCount = campaigns.Count(x => !x.Ready);
        summary.ReadyCount = campaigns.Count(x => x.Ready);

        var res = new CrowdfunderMigrationStatusRes();
        res.Campaigns = campaigns;
        res.CrowdfundersWithoutEnabledCampaign = GetCrowdfundersWithoutEnabledCampaign();
        res.Summary = summary;

        return Task.FromResult<ActionResult<CrowdfunderMigrationStatusRes>>(Ok(res));
    }

    [HttpPost("crowdfunders/migration/complete")]
    public Task<ActionResult<CompleteCrowdfunderMigrationRes>> CompleteCrowdfunderMigration() {
        var res = new CompleteCrowdfunderMigrationRes();
        res.CompositionRemovedFrom = new List<string>();
        res.NotReadyCampaigns = new List<CrowdfunderMigrationCampaignRes>();

        var legacyContentType = _contentTypeService.Get(PlatformsConstants.CrowdfundingCampaign.CompositionAlias);

        if (legacyContentType == null) {
            _logger.LogInformation("The {Alias} content type no longer exists so there is nothing to do",
                                   PlatformsConstants.CrowdfundingCampaign.CompositionAlias);

            res.AlreadyCompleted = true;
            res.Completed = true;

            return Task.FromResult<ActionResult<CompleteCrowdfunderMigrationRes>>(Ok(res));
        }

        var notReadyCampaigns = EvaluateEnabledCampaigns().Where(x => !x.Ready).ToList();

        if (notReadyCampaigns.HasAny()) {
            _logger.LogWarning("Refusing to complete the crowdfunder migration as {Count} campaigns are not ready",
                               notReadyCampaigns.Count);

            res.NotReadyCampaigns = notReadyCampaigns;

            return Task.FromResult<ActionResult<CompleteCrowdfunderMigrationRes>>(Ok(res));
        }

        // Enumerated dynamically via the composition axis rather than from a hardcoded list of campaign types,
        // as a site can compose the legacy type onto types the others do not and a hardcoded list would silently
        // leave that site's data behind.
        var composedContentTypes = _contentTypeService.GetComposedOf(legacyContentType.Id).ToList();
        var compositionRemovedFrom = new List<string>();

        foreach (var contentType in composedContentTypes) {
            // Must be RemoveContentType rather than assigning ContentTypeComposition: only RemoveContentType
            // populates RemovedContentTypes, which is what makes the repository purge the now-orphaned
            // umbracoPropertyData rows when the type is saved.
            contentType.RemoveContentType(PlatformsConstants.CrowdfundingCampaign.CompositionAlias);

            _contentTypeService.Save(contentType);

            compositionRemovedFrom.Add(contentType.Alias);

            _logger.LogInformation("Removed the {Composition} composition from {ContentType}",
                                   PlatformsConstants.CrowdfundingCampaign.CompositionAlias,
                                   contentType.Alias);
        }

        // TODO Once every site has run this, rename the crowdfunder type onto the alias freed here. That has to
        // happen in a package release rather than from an endpoint, because the stored alias and the compiled
        // constant have to change together or the content model stops binding.
        _contentTypeService.Delete(legacyContentType);

        _logger.LogInformation("Deleted the {Alias} content type",
                               PlatformsConstants.CrowdfundingCampaign.CompositionAlias);

        res.Completed = true;
        res.CompositionRemovedFrom = compositionRemovedFrom;
        res.LegacyCompositionDeleted = true;

        return Task.FromResult<ActionResult<CompleteCrowdfunderMigrationRes>>(Ok(res));
    }

    private IReadOnlyList<CrowdfunderMigrationCampaignRes> EvaluateEnabledCampaigns() {
        var results = new List<CrowdfunderMigrationCampaignRes>();
        var sourceAliases = CrowdfunderContentSources.All;

        var crowdfunders = _contentService.GetCrowdfundersByCampaign(_contentTypeService);

        foreach (var campaign in _contentService.GetCrowdfundingEnabledCampaigns(_contentTypeService)) {
            var res = new CrowdfunderMigrationCampaignRes();
            res.CampaignId = campaign.Key;
            res.CampaignName = campaign.Name;

            var crowdfunder = crowdfunders.GetValueOrDefault(campaign.Key);

            res.HasCrowdfunder = crowdfunder != null;
            res.TemplateSourceAlias = campaign.FirstAliasWithValue(sourceAliases);

            res.UsedFallbackSource = res.TemplateSourceAlias.HasValue() &&
                                     res.TemplateSourceAlias !=
                                     PlatformsConstants.CrowdfundingCampaign.Properties.Content;

            if (crowdfunder != null) {
                var pageTemplate = PlatformsConstants.Crowdfunders.Crowdfunder.Properties.PageTemplate;

                res.PageTemplatePopulated = crowdfunder.FirstAliasWithValue([pageTemplate]).HasValue();
            }

            // A template is only demanded where there was a source to copy from.
            res.Ready = res.HasCrowdfunder &&
                        (res.TemplateSourceAlias == null || res.PageTemplatePopulated);

            results.Add(res);
        }

        return results;
    }

    private IReadOnlyList<CrowdfunderWithoutEnabledCampaignRes> GetCrowdfundersWithoutEnabledCampaign() {
        var results = new List<CrowdfunderWithoutEnabledCampaignRes>();
        var alias = AliasHelper<CrowdfundingCampaignContent>.PropertyAlias(x => x.CrowdfundingEnabled);

        foreach (var crowdfunder in _contentLocator.All<CrowdfunderContent>()) {
            var campaign = crowdfunder.Campaign;

            if (campaign == null || (campaign.HasProperty(alias) && campaign.Value<bool>(alias))) {
                continue;
            }

            var res = new CrowdfunderWithoutEnabledCampaignRes();
            res.CampaignId = campaign.Key;
            res.CampaignName = campaign.Name;
            res.CrowdfunderId = crowdfunder.Key;
            res.CrowdfunderName = crowdfunder.Content().Name;

            results.Add(res);
        }

        return results;
    }
}