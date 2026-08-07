using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;

namespace N3O.Umbraco.Media;

public class MediaLocator : Locator, IMediaLocator {
    private readonly IMediaNavigationQueryService _navigationQueryService;
    private readonly IPublishedMediaCache _mediaCache;

    public MediaLocator(IMediaNavigationQueryService navigationQueryService,
                        IPublishedMediaCache mediaCache) {
        _navigationQueryService = navigationQueryService;
        _mediaCache = mediaCache;
    }
    
    protected override IPublishedCache PublishedCache => _mediaCache;

    protected override IEnumerable<IPublishedContent> GetRootContents() {
        return _navigationQueryService.GetPublishedRootContents(_mediaCache);
    }
}
