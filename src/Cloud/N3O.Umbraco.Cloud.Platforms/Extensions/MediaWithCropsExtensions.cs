using Flurl;
using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class MediaWithCropsExtensions {
    public static Uri GetPublishedUri(this MediaWithCrops media,
                                      IContentLocator contentLocator,
                                      IWebHostEnvironment webHostEnvironment) {
        if (media == null) {
            return null;
        }

        var cropUrlStr = media.GetCropUrl(urlMode: UrlMode.Absolute);
        
        if (cropUrlStr.HasValue()) {
            var cropUrl = new Url(cropUrlStr);
            var baseUrl = contentLocator.Single<UrlSettingsContent>().BaseUrl(webHostEnvironment);

            var url = new Url(baseUrl);
            url.Path = cropUrl.Path;
            url.Query = cropUrl.Query;
            
            return new Uri(url);
        } else {
            return null;
        }
    }
}