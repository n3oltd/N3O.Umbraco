using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Redirects;

public class RedirectManagement : IRedirectManagement {
    private readonly IContentCache _contentCache;

    public RedirectManagement(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public Redirect FindRedirect(string requestPath) {
        if (!requestPath.HasValue()) {
            return null;
        }

        requestPath = Normalise(requestPath);

        var searchPaths = new[] {
            requestPath,
            requestPath + "/",
            "/" + requestPath,
            "/" + requestPath + "/"
        };

        var redirects = _contentCache.All<RedirectContent>();
        var redirect = redirects.FirstOrDefault(x => searchPaths.Contains(x.Content().Name, true));

        return redirect.IfNotNull(x => new Redirect(x.Temporary, x.GetLinkUrl()));
    }

    private string Normalise(string requestPath) {
        requestPath = requestPath.ToLowerInvariant();

        if (requestPath.StartsWith("/")) {
            requestPath = requestPath.Substring(1);
        }

        if (requestPath.EndsWith("/")) {
            requestPath = requestPath.Substring(0, requestPath.Length - 1);
        }

        return requestPath;
    }
}
