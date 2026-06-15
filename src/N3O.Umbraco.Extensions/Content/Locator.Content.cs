using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Content;

public class ContentLocator : Locator, IContentLocator {
    private readonly IDocumentNavigationQueryService _navigationQueryService;
    private readonly IPublishedContentCache _contentCache;

    public ContentLocator(IDocumentNavigationQueryService navigationQueryService,
                          IPublishedContentCache contentCache) {
        _navigationQueryService = navigationQueryService;
        _contentCache = contentCache;
    }

    protected override IPublishedCache PublishedCache => _contentCache;

    protected override IEnumerable<IPublishedContent> GetRootContents() {
        return _navigationQueryService.GetPublishedRootContents(_contentCache);
    }
}
