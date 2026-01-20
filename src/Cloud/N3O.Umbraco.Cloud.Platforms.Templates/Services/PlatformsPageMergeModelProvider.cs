using N3O.Umbraco.Extensions;
using N3O.Umbraco.Templates;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

public class PlatformsPageMergeModelProvider : MergeModelsProvider {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;

    public PlatformsPageMergeModelProvider(IPlatformsPageAccessor platformsPageAccessor) {
        _platformsPageAccessor = platformsPageAccessor;
    }

    protected override async Task PopulateModelsAsync(IPublishedContent content,
                                                      Dictionary<string, object> mergeModels,
                                                      CancellationToken cancellationToken = default) {
        var getPageResult = await _platformsPageAccessor.GetAsync();

        foreach (var x in getPageResult.OrEmpty(x => x.Page?.MergeModels)) {
            mergeModels[x.Kind.Id] = x.Content;
        }
    }
}