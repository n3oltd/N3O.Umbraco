using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[Order(int.MinValue)]
public class ContentElements : LookupsCollection<Element> {
    private readonly IContentCache _contentCache;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public ContentElements(IContentCache contentCache, IUmbracoContextAccessor umbracoContextAccessor) {
        _contentCache = contentCache;
        _umbracoContextAccessor = umbracoContextAccessor;

        _contentCache.Flushed += ContentCacheOnFlushed;
    }
    
    protected override Task<IReadOnlyList<Element>> LoadAllAsync(CancellationToken cancellationToken) {
        var all = GetFromCache();
        
        return Task.FromResult(all);
    }

    private IReadOnlyList<Element> GetFromCache() {
        List<ElementContent> content;
        
        if (_umbracoContextAccessor.TryGetUmbracoContext(out _)) {
            content = _contentCache.All<IPublishedContent>(x => x.IsComposedOf(AliasHelper<ElementContent>.ContentTypeAlias()))
                                   .OrderBy(x => x.Name)
                                   .As<ElementContent>()
                                   .ToList();
        } else {
            content = [];
        }
        
        var lookups = content.Select(ToElement).ToList();

        return lookups;
    }

    private Element ToElement(ElementContent elementContent) {
        ElementKind elementKind;
        
        if (elementContent.Type == ElementTypes.CreateCrowdfunderButton) {
            elementKind = ElementKind.CreateCrowdfunderButton;
        } else if (elementContent.Type == ElementTypes.DonationButton) {
            elementKind = ElementKind.DonationButtonCustom;
        } else if (elementContent.Type == ElementTypes.DonationForm) {
            elementKind = ElementKind.DonationFormCustom;
        } else if (elementContent.Type == ElementTypes.DonationPopup) {
            elementKind = ElementKind.DonationPopupCustom;
        } else {
            throw UnrecognisedValueException.For(elementContent.Type);
        }
        
        return new Element(LookupContent.GetId(elementContent.Content()),
                           LookupContent.GetName(elementContent.Content()),
                           elementContent.Content().Key,
                           elementKind,
                           elementContent.EmbedCode);
    }

    private void ContentCacheOnFlushed(object sender, EventArgs e) {
        var all = GetFromCache();
        
        Reload(all);
    }
}