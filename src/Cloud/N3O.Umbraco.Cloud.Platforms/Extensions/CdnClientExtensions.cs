using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class CdnClientExtensions {
    public static async Task<PlatformsPage> DownloadPlatformsPageAsync(this ICdnClient cdnClient,
                                                                       IJsonProvider jsonProvider,
                                                                       PublishedFileKind kind,
                                                                       string path,
                                                                       CancellationToken cancellationToken = default) {
        var pagePath = $"{kind.Id}/{path.Trim('/')}/index.json";
        
        var publishedContentResult = await cdnClient.DownloadPublishedContentAsync(pagePath, cancellationToken);

        if (publishedContentResult.NotFound) {
            return null;
        } else {
            var publishedPlatformsPage = jsonProvider.DeserializeDynamicTo<PublishedPlatformsPage>(publishedContentResult.Content);

            var mergeModels = await publishedPlatformsPage.OrEmpty(x => x.MergeModels)
                                                          .SelectListAsync(x => FetchMergeModelAsync(cdnClient, x));
            
            return new PlatformsPage(publishedContentResult.Id.GetValueOrThrow(),
                                     publishedContentResult.Kind,
                                     publishedContentResult.Path,
                                     publishedPlatformsPage.MetaTags,
                                     mergeModels);
        }
    }
    
    private static Task<PublishedContentResult> FetchMergeModelAsync(ICdnClient cdnClient,
                                                                     PublishedFileInfo publishedModel) {
        return cdnClient.DownloadPublishedContentAsync(publishedModel.Path);
    }
}