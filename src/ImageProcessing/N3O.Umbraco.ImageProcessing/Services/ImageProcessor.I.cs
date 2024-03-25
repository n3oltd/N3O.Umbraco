using N3O.Umbraco.ImageProcessing.Models;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.ImageProcessing;

public interface IImageProcessor {
    IImageProcessor ApplyOperation(IPublishedElement options);
    IImageProcessor ApplyPreset(string presetName);
    IImageProcessor Combine(params ImageLayer[] layers);
    IImageProcessor Combine(IEnumerable<ImageLayer> layers);
}