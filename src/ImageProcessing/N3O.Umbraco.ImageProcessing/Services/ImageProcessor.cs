using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.ImageProcessing.Content;
using N3O.Umbraco.ImageProcessing.Models;
using N3O.Umbraco.ImageProcessing.Operations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.ImageProcessing;

public class ImageProcessor : IImageProcessor {
    private readonly IEnumerable<IImageOperation> _allOperations;
    private readonly IContentCache _contentCache;
    private readonly Image _image;

    public ImageProcessor(IEnumerable<IImageOperation> allOperations, IContentCache contentCache, Image image) {
        _allOperations = allOperations;
        _contentCache = contentCache;
        _image = image;
    }
    
    public IImageProcessor ApplyOperation(IPublishedElement options) {
        var operation = _allOperations.Single(x => x.IsOperation(options));
        
        _image.Mutate(c => {
            operation.Apply(options, c);
        });

        return this;
    }
    
    public IImageProcessor ApplyPreset(string presetName) {
        var preset = _contentCache.Single<ImagePresetContent>(x => x.Content().Name.EqualsInvariant(presetName));

        foreach (var operation in preset.Operations) {
            ApplyOperation(operation);
        }
        
        return this;
    }

    public IImageProcessor Combine(params ImageLayer[] layers) {
        return Combine(layers.AsEnumerable());
    }
    
    public IImageProcessor Combine(IEnumerable<ImageLayer> layers) {
        _image.Mutate(operation => {
            foreach (var layer in layers) {
                var options = new GraphicsOptions();
                options.BlendPercentage = 1f;
                options.ColorBlendingMode = PixelColorBlendingMode.Normal;
                
                operation.DrawImage(layer.Image, layer.Rectangle, PixelColorBlendingMode.Overlay,  PixelAlphaCompositionMode.DestOver, 1f);
            }
        });
        
        return this;
    }
}