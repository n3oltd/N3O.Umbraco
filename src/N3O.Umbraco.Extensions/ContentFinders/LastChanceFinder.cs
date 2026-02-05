using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Net;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.ContentFinders;

public class LastChanceFinder : IContentLastChanceFinder {
    private readonly IContentCache _contentCache;

    public LastChanceFinder(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public Task<bool> TryFindContent(IPublishedRequestBuilder request) {
        /*if (request != null && request.ResponseStatusCode == 404) {
            var notFound = (int) HttpStatusCode.NotFound;
            request.SetResponseStatus(notFound);
            request.SetPublishedContent(_contentCache.Special(SpecialPages.NotFound));

            return Task.FromResult(true);
        }*/

        return Task.FromResult(false);
    }
}
