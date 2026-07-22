using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using MediaConstants = Umbraco.Cms.Core.Constants.Conventions.Media;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class MediaWithCropsExtensions {
    public static ImageSimpleContentReq ToImageSimpleContentReq(this MediaWithCrops media, IMediaUrl mediaUrl) {
        if (media == null) {
            return null;
        }
        
        var req = new ImageSimpleContentReq();
        req.SourceFile = mediaUrl.GetMediaUrl(media, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x)).ToString();
        
        req.Main = new ImageSimpleProcessingReq();
        req.Main.Crop = new ImageCropReq();
        req.Main.Crop.X = 0;
        req.Main.Crop.Y = 0;
        req.Main.Crop.Width = (int) media.Properties.Single(x => x.Alias == MediaConstants.Width).GetValue();
        req.Main.Crop.Height = (int) media.Properties.Single(x => x.Alias == MediaConstants.Height).GetValue();

        return req;
    }

    public static SvgContentReq ToSvgContentReq(this MediaWithCrops media, IMediaUrl mediaUrl) {
        if (media == null) {
            return null;
        }
        
        var req = new SvgContentReq();
        req.SourceFile = mediaUrl.GetMediaUrl(media, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x)).ToString();

        return req;
    }
}