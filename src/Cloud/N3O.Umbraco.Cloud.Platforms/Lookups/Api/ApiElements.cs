using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

[Order(int.MaxValue)]
public class ApiElements : ApiLookupsCollection<Element> {
    private readonly ICdnClient _cdnClient;

    public ApiElements(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<Element>> FetchAsync(CancellationToken cancellationToken) {
        var publishedElements = await _cdnClient.DownloadSubscriptionContentAsync<PublishedElements>(SubscriptionFiles.Elements,
                                                                                                     JsonSerializers.JsonProvider,
                                                                                                     cancellationToken);

        var elements = new List<Element>();

        foreach (var publishedElement in publishedElements.Elements) {
            var element = new Element(publishedElement.Id,
                                      publishedElement.Name,
                                      null,
                                      publishedElement.ElementKind.GetValueOrThrow());
            
            elements.Add(element);
        }

        return elements;
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(12);
}