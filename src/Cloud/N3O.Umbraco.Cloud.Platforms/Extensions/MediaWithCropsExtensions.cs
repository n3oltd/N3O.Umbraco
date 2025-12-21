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
    public static ImageSimpleContentReq ToImageSimpleContentReq(this MediaWithCrops media,
                                                                IMediaUrl mediaUrl) {
        var req = new ImageSimpleContentReq();
        req.SourceFile = mediaUrl.GetMediaUrl(media, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x)).ToString();
        
        req.Main = new ImageSimpleProcessingReq();
        req.Main.Crop = new ImageCropReq();
        
        req.Main.Crop.BottomLeft = new PointReq();
        req.Main.Crop.BottomLeft.X = 0;
        req.Main.Crop.BottomLeft.Y = ((int) media.Properties.Single(x => x.Alias == MediaConstants.Height).GetValue()) - 1;
        
        req.Main.Crop.TopRight = new PointReq();
        req.Main.Crop.TopRight.X = ((int) media.Properties.Single(x => x.Alias == MediaConstants.Width).GetValue()) - 1;
        req.Main.Crop.TopRight.Y = 0;

        return req;
    }
}