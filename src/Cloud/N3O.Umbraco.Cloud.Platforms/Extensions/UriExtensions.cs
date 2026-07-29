using Flurl;
using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class UriExtensions {
    public static string RebaseOnSiteRoot(this Uri url, IUrlBuilder urlBuilder) {
        if (url == null) {
            return null;
        }

        var rootUrl = urlBuilder.Root();
        var rebasedUrl = new Url(url.AbsolutePath);

        rebasedUrl.Scheme = rootUrl.Scheme;
        rebasedUrl.Host = rootUrl.Host;
        rebasedUrl.Port = rootUrl.Port;

        return rebasedUrl;
    }
}
