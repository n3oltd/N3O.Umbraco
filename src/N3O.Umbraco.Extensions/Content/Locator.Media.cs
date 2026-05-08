using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Media;

public class MediaLocator : Locator, IMediaLocator {
    private readonly IMediaNavigationQueryService _navigationQueryService;

    public MediaLocator(IUmbracoContextAccessor umbracoContextAccessor,
                        IMediaNavigationQueryService navigationQueryService) : base(umbracoContextAccessor) {
        _navigationQueryService = navigationQueryService;
    }

    protected override IPublishedCache GetCache(IUmbracoContextAccessor umbracoContextAccessor) {
        return umbracoContextAccessor.GetMediaCache();
    }

    protected override IEnumerable<Guid> GetRootKeys() {
        _navigationQueryService.TryGetRootKeys(out var rootKeys);
        return rootKeys ?? Enumerable.Empty<Guid>();
    }
}
