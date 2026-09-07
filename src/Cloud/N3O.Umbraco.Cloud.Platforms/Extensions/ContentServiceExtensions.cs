using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class ContentServiceExtensions {
    private const int PageSize = 100;

    public static IEnumerable<IContent> GetCrowdfundingCampaigns(this IContentService contentService,
                                                                  IContentTypeService contentTypeService) {
        var contentType = contentTypeService.Get(PlatformsConstants.CrowdfundingCampaigns.CrowdfundingCampaign.Alias);

        if (contentType == null) {
            yield break;
        }

        for (var pageIndex = 0; true; pageIndex++) {
            var page = contentService.GetPagedOfType(contentType.Id, pageIndex, PageSize, out var totalRecords, null);

            foreach (var content in page) {
                yield return content;
            }

            if ((pageIndex + 1) * (long) PageSize >= totalRecords) {
                yield break;
            }
        }
    }
}
