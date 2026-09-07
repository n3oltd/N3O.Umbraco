using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class ContentServiceExtensions {
    private const int PageSize = 100;

    // Read through the content service rather than the published cache, so an unpublished crowdfunding
    // campaign is included and one holding a legacy picker value binds like any other.
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
