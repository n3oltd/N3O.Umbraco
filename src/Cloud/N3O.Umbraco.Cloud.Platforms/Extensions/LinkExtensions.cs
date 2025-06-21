using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class LinkExtensions {
    public static Uri GetPublishedUri(this Link link) {
        if (link == null) {
            return null;
        }

        var url = link.Content?.AbsoluteUrl() ?? link?.Url;

        if (url.HasValue()) {
            return new Uri(url);
        } else {
            return null;
        }
    }
}