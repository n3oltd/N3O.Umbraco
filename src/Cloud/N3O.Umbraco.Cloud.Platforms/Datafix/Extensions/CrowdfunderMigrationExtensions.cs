using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

// TODO Delete along with the rest of the Datafix folder once every site has completed the migration.
public static class CrowdfunderMigrationExtensions {
    private const int PageSize = 100;

    public static string FirstAliasWithValue(this IContent content, IEnumerable<string> candidateAliases) {
        foreach (var alias in candidateAliases.OrEmpty()) {
            if (HasContent(content.GetValue(alias))) {
                return alias;
            }
        }

        return null;
    }

    public static Dictionary<Guid, IContent> GetCrowdfundersByCampaign(this IContentService contentService,
                                                                      IContentTypeService contentTypeService) {
        var crowdfunders = new Dictionary<Guid, IContent>();
        var contentType = contentTypeService.Get(PlatformsConstants.Crowdfunders.Crowdfunder.Alias);

        if (contentType == null) {
            return crowdfunders;
        }

        foreach (var crowdfunder in GetAllOfType(contentService, contentType.Id)) {
            var campaignKey = crowdfunder.GetCrowdfunderCampaignKey();

            if (campaignKey.HasValue()) {
                crowdfunders[campaignKey.GetValueOrThrow()] = crowdfunder;
            }
        }

        return crowdfunders;
    }

    public static IReadOnlyList<IContent> GetCrowdfundingEnabledCampaigns(this IContentService contentService,
                                                                         IContentTypeService contentTypeService) {
        var compositionAlias = PlatformsConstants.CrowdfundingCampaign.CompositionAlias;
        var legacyComposition = contentTypeService.Get(compositionAlias);

        if (legacyComposition == null) {
            return [];
        }

        var alias = AliasHelper<CrowdfundingCampaignContent>.PropertyAlias(x => x.CrowdfundingEnabled);
        var campaigns = new List<IContent>();

        foreach (var contentType in contentTypeService.GetComposedOf(legacyComposition.Id)) {
            campaigns.AddRange(GetAllOfType(contentService, contentType.Id)
                                   .Where(x => x.GetValue<bool>(alias)));
        }

        return campaigns;
    }

    private static IEnumerable<IContent> GetAllOfType(IContentService contentService, int contentTypeId) {
        var pageIndex = 0;
        long total;

        do {
            var page = contentService.GetPagedOfType(contentTypeId, pageIndex++, PageSize, out total, null);

            foreach (var content in page) {
                yield return content;
            }
        } while (pageIndex * PageSize < total);
    }

    private static bool HasContent(object value) {
        var json = value as string;

        if (json == null) {
            return value != null;
        }

        if (!json.HasValue()) {
            return false;
        }

        var trimmed = json.TrimStart();

        if (!trimmed.StartsWith('{')) {
            return true;
        }

        JObject envelope;

        try {
            envelope = JObject.Parse(trimmed);
        } catch (JsonException) {
            return true;
        }

        var blocks = envelope.Property("blocks") ?? envelope.Property("contentData");

        if (blocks == null) {
            return true;
        }

        return blocks.Value.HasValues || envelope.Property("header")?.Value.HasValues == true;
    }
}
