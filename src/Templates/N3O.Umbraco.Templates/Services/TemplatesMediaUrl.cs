using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using N3O.Umbraco.Utilities;
using System;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Templates;

public class TemplatesMediaUrl : MediaUrl {
    public TemplatesMediaUrl(MediaFileManager mediaFileManager, IUrlBuilder urlBuilder) 
        : base(mediaFileManager, urlBuilder) {
    }

    public override string GetCropUrl(MediaWithCrops mediaWithCrops,
                                      string cropAlias,
                                      string furtherOptions = null,
                                      UrlMode urlMode = UrlMode.Default) {
        return GetUrl(mediaWithCrops, () => base.GetCropUrl(mediaWithCrops, cropAlias, furtherOptions, urlMode));
    }

    public override string GetCropUrl(MediaWithCrops mediaWithCrops,
                                      int width,
                                      string furtherOptions = null,
                                      UrlMode urlMode = UrlMode.Default) {
        return GetUrl(mediaWithCrops, () => base.GetCropUrl(mediaWithCrops, width, furtherOptions, urlMode));
    }

    public override string GetCropUrl(MediaWithCrops mediaWithCrops,
                                      int width,
                                      int height,
                                      string furtherOptions = null,
                                      UrlMode urlMode = UrlMode.Default) {
        return GetUrl(mediaWithCrops, () => base.GetCropUrl(mediaWithCrops, width, height, furtherOptions, urlMode));
    }

    public override string GetMediaUrl(MediaWithCrops mediaWithCrops,
                                       string furtherOptions = null,
                                       UrlMode urlMode = UrlMode.Default) {
        return GetUrl(mediaWithCrops, () => base.GetMediaUrl(mediaWithCrops, furtherOptions, urlMode));
    }

    private string GetUrl(MediaWithCrops mediaWithCrops, Func<string> fallbackAction) {
        if (mediaWithCrops.Content.Value(TemplateConstants.Media.MergeField).HasValue()) {
            return mediaWithCrops.Content.Value<string>(TemplateConstants.Media.MergeField);
        } else {
            return fallbackAction();
        }
    }
}