using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Content;

public class ContentLocator : Locator, IContentLocator {
    private readonly IDocumentNavigationQueryService _navigationQueryService;

    public ContentLocator(IUmbracoContextAccessor umbracoContextAccessor,
                          IDocumentNavigationQueryService navigationQueryService) : base(umbracoContextAccessor) {
        _navigationQueryService = navigationQueryService;
    }

    protected override IPublishedCache GetCache(IUmbracoContextAccessor umbracoContextAccessor) {
        return umbracoContextAccessor.GetContentCache();
    }

    protected override IEnumerable<Guid> GetRootKeys() {
        _navigationQueryService.TryGetRootKeys(out var rootKeys);
        return rootKeys ?? Enumerable.Empty<Guid>();
    }
}
