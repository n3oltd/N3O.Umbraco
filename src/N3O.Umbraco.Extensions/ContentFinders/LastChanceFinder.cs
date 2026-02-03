using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Redirects;
using System.Net;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace N3O.Umbraco.ContentFinders;

public class LastChanceFinder : IContentLastChanceFinder {
    private readonly IContentCache _contentCache;
    private readonly IRedirectManagement _redirectManagement;

    public LastChanceFinder(IContentCache contentCache, IRedirectManagement redirectManagement) {
        _contentCache = contentCache;
        _redirectManagement = redirectManagement;
    }

    public Task<bool> TryFindContent(IPublishedRequestBuilder request) {
        if (TryFindRedirectContent(request)) {
            return Task.FromResult(false);
        }

        if (request != null && request.ResponseStatusCode == 404) {
            var notFound = (int) HttpStatusCode.NotFound;
            request.SetResponseStatus(notFound);
            request.SetPublishedContent(_contentCache.Special(SpecialPages.NotFound));

            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    private bool TryFindRedirectContent(IPublishedRequestBuilder request) {
        var requestedPath = request.Uri.GetAbsolutePathDecoded().ToLowerInvariant();

        var redirect = _redirectManagement.FindRedirect(requestedPath);

        if (redirect != null) {
            _redirectManagement.LogHit(redirect.Id);

            Redirect(request, redirect.Temporary, redirect.Url);

            return true;
        }

        return false;
    }

    private void Redirect(IPublishedRequestBuilder request, bool temporary, string urlOrPath) {
        var httpCode = temporary ? HttpStatusCode.TemporaryRedirect : HttpStatusCode.Redirect;
        
        request.SetRedirect(urlOrPath, (int) httpCode);
    }
}
