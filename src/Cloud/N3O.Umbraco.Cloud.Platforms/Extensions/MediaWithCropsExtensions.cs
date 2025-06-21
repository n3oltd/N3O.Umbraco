using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class MediaWithCropsExtensions {
    public static Uri GetPublishedUri(this MediaWithCrops media) {
        if (media == null) {
            return null;
        }

        var url = media.GetCropUrl(urlMode: UrlMode.Absolute);

        if (url.HasValue()) {
            return new Uri(url);
        } else {
            return null;
        }
    }
}