using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class MediaWithCropsDto {
    public Guid Key { get; set; }
    public Guid MediaKey { get; set; }
    public IEnumerable<ImageCropperValue.ImageCropperCrop> Crops { get; set; }
    public ImageCropperValue.ImageCropperFocalPoint FocalPoint { get; set; }
    
    public MediaWithCrops ToMediaWithCrops(IPublishedValueFallback publishedValueFallback, IMediaLocator mediaLocator) {
        var media = mediaLocator.ById(MediaKey);

        var imageCropperValue = new ImageCropperValue();

        imageCropperValue.FocalPoint = FocalPoint.IfNotNull(x => new ImageCropperValue.ImageCropperFocalPoint {
            Left = x.Left, Top = x.Top
        });
        
        imageCropperValue.Crops = Crops?.Select(c => new ImageCropperValue.ImageCropperCrop {
                Alias = c.Alias,
                Width = c.Width,
                Height = c.Height,
                Coordinates = c.Coordinates.IfNotNull(x => new ImageCropperValue.ImageCropperCropCoordinates {
                    X1 = x.X1, Y1 = x.Y1, X2 = x.X2, Y2 = x.Y2
                })
            }).ToList();
        
        return new MediaWithCrops(media, publishedValueFallback, imageCropperValue);
    }
}