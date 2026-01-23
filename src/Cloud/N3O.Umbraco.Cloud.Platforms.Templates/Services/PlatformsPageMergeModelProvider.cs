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

        if (getPageResult.HasValue(x => x.Page)) {
            mergeModels[getPageResult.Page.Kind.Id] = getPageResult.Page.Content;
        }

        foreach (var x in getPageResult.OrEmpty(x => x.Page?.AdditionalModels)) {
            mergeModels[x.Kind.Id] = x.Content;
        }
    }
}