using N3O.Umbraco.Content;
using N3O.Umbraco.Pages;
using N3O.Umbraco.Redirects;
using System.Net;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace N3O.Umbraco.ContentFinders {
    public class Site404ContentFinder : IContentLastChanceFinder {
        private readonly IContentCache _contentCache;
        private readonly IRedirectManagement _redirectManagement;

        public Site404ContentFinder(IContentCache contentCache, IRedirectManagement redirectManagement) {
            _contentCache = contentCache;
            _redirectManagement = redirectManagement;
        }

        public bool TryFindContent(IPublishedRequestBuilder request) {
            if (TryFindRedirectContent(request)) {
                return false;
            }

            if (request != null && request.ResponseStatusCode == 404) {
                var notFound = (int) HttpStatusCode.NotFound;
                request.SetResponseStatus(notFound);
                request.SetPublishedContent(_contentCache.Single<UrlNotFoundPageContent>()?.Content);

                return true;
            }

            return false;
        }

        private bool TryFindRedirectContent(IPublishedRequestBuilder request) {
            var requestedPath = request.Uri.GetAbsolutePathDecoded().ToLowerInvariant();

            var redirect = _redirectManagement.FindRedirect(requestedPath);

            if (redirect != null) {
                var redirectUrl = redirect.Url;

                _redirectManagement.LogHit(redirect.Id);

                var httpCode = redirect.Temporary ? HttpStatusCode.TemporaryRedirect : HttpStatusCode.Redirect;
                request.SetRedirect(redirectUrl, (int) httpCode);

                return true;
            }

            return false;
        }
    }
}
