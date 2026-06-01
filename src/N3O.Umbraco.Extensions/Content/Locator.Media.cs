using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Media;

public class MediaLocator : Locator, IMediaLocator {
    private readonly IMediaNavigationQueryService _navigationQueryService;
    private readonly IPublishedMediaCache _mediaCache;

    public MediaLocator(IUmbracoContextAccessor umbracoContextAccessor,
                        IMediaNavigationQueryService navigationQueryService,
                        IPublishedMediaCache mediaCache) : base(umbracoContextAccessor) {
        _navigationQueryService = navigationQueryService;
        _mediaCache = mediaCache;
    }

    protected override IPublishedContent GetById(int id) => _mediaCache.GetById(id);
    protected override IPublishedContent GetById(Guid id) => _mediaCache.GetById(id);

    protected override IEnumerable<Guid> GetRootKeys() {
        _navigationQueryService.TryGetRootKeys(out var rootKeys);
        return rootKeys ?? Enumerable.Empty<Guid>();
    }
}
