using Flurl;
using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.Extensions;

public static class UrlBuilderExtensions {
    public static Url SpecialPage(this UrlBuilder urlBuilder,
                                  IContentCache contentCache,
                                  SpecialContent specialContent) {
        var url = urlBuilder.Root();
        var path = SpecialContentPathParser.GetPath(contentCache, specialContent);

        if (!path.HasValue()) {
            throw new Exception($"Could not resolve path for {specialContent.Id.Quote()}");
        }
        
        url.AppendPathSegment(path);

        return url;
    }
}