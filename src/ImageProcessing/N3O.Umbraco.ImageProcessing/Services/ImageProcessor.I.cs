using N3O.Umbraco.ImageProcessing.Content;
using N3O.Umbraco.ImageProcessing.Models;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.ImageProcessing;

public interface IImageProcessor {
    IImageProcessor ApplyOperation(IPublishedElement options, ImagePresetContent preset);
    IImageProcessor ApplyPreset(string presetName);
    IImageProcessor Combine(params ImageLayer[] layers);
    IImageProcessor Combine(IEnumerable<ImageLayer> layers);
    IImageProcessor Mutate(Action<IImageProcessingContext> action);
}