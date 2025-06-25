using Flurl;
using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using MediaConventions = Umbraco.Cms.Core.Constants.Conventions.Media;

namespace N3O.Umbraco.Media;

public class MediaUrl : IMediaUrl {
    private readonly MediaFileManager _mediaFileManager;
    private readonly IUrlBuilder _urlBuilder;

    public MediaUrl(MediaFileManager mediaFileManager, IUrlBuilder urlBuilder) {
        _mediaFileManager = mediaFileManager;
        _urlBuilder = urlBuilder;
    }

    public string GetCropUrl(MediaWithCrops mediaWithCrops,
                             string cropAlias,
                             string furtherOptions = null,
                             UrlMode urlMode = UrlMode.Default) {
        return GetUrl(() => mediaWithCrops?.GetCropUrl(cropAlias: cropAlias,
                                                       furtherOptions: furtherOptions,
                                                       urlMode: urlMode));
    }

    public string GetCropUrl(MediaWithCrops mediaWithCrops,
                             int width,
                             string furtherOptions = null,
                             UrlMode urlMode = UrlMode.Default) {
        return GetUrl(() => mediaWithCrops?.GetCropUrl(width: width,
                                                       preferFocalPoint: true,
                                                       imageCropMode: ImageCropMode.Crop,
                                                       furtherOptions: furtherOptions,
                                                       urlMode: urlMode));
    }

    public string GetCropUrl(MediaWithCrops mediaWithCrops,
                             int width,
                             int height,
                             string furtherOptions = null,
                             UrlMode urlMode = UrlMode.Default) {
        return GetUrl(() => mediaWithCrops?.GetCropUrl(width: width,
                                                       height: height,
                                                       preferFocalPoint: true,
                                                       imageCropMode: ImageCropMode.Crop,
                                                       furtherOptions: furtherOptions,
                                                       urlMode: urlMode));
    }

    public string GetMediaUrl(MediaWithCrops mediaWithCrops,
                              string furtherOptions = null,
                              UrlMode urlMode = UrlMode.Default) {
        return GetUrl(() => mediaWithCrops?.GetCropUrl(furtherOptions: furtherOptions, urlMode: urlMode));
    }

    public HtmlString InlineSvg(MediaWithCrops mediaWithCrops) {
        var srcPath = mediaWithCrops?.Content.HasProperty(MediaConventions.File) == true
                          ? mediaWithCrops.Content.Value(MediaConventions.File) as string
                          : null;

        if (srcPath.HasValue()) {
            return new HtmlString(_mediaFileManager.InlineSvg(srcPath));
        } else {
            return null;
        }
    }

    private Url GetUrl(Func<string> getSrc) {
        var src = getSrc();
        
        if (src.HasValue()) {
            var srcUrl = new Url(src);

            if (srcUrl.Host.HasValue()) {
                var url = _urlBuilder.Root();

                url.AppendPathSegment(srcUrl.Path);
                url.Query = srcUrl.Query;

                return url;
            }
        }
        
        return null;
    }
}