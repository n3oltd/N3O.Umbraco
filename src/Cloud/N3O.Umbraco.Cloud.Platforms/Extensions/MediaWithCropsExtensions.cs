using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class MediaWithCropsExtensions {
    public static ImageSimpleContentReq ToImageSimpleContentReq(this MediaWithCrops media,
                                                                IMediaUrl mediaUrl) {
        var req = new ImageSimpleContentReq();
        req.SourceFile = mediaUrl.GetMediaUrl(media, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x)).ToString();
        
        req.Main = new ImageSimpleProcessingReq();
        req.Main.Crop = new ImageCropReq();
        
        req.Main.Crop.BottomLeft = new PointReq();
        req.Main.Crop.BottomLeft.X = (int?) media.LocalCrops.GetCrop(null)?.Coordinates?.X1;
        req.Main.Crop.BottomLeft.X = (int?) media.LocalCrops.GetCrop(null)?.Coordinates?.X2;
        
        req.Main.Crop.TopRight = new PointReq();
        req.Main.Crop.TopRight.X = (int?) media.LocalCrops.GetCrop(null)?.Coordinates?.Y1;
        req.Main.Crop.TopRight.X = (int?) media.LocalCrops.GetCrop(null)?.Coordinates?.Y2;

        return req;
    }
}