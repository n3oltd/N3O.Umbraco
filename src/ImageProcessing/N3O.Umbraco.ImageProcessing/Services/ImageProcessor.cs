using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
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
    private readonly Image _image;
    private readonly IContentCache _contentCache;

    public ImageProcessor(IEnumerable<IImageOperation> allOperations, Image image, IContentCache contentCache) {
        _allOperations = allOperations;
        _image = image;
        _contentCache = contentCache;
    }
    
    public void ApplyOperation(string presetName) {
        var options = _contentCache.Single(ImageProcessingConstants.Preset.ContentTypeAlias, x => x.Name.EqualsInvariant(presetName));
        
        options.
    }

    public IImageProcessor ApplyOperation(IPublishedElement operationOptions) {
        var operation = _allOperations.Single(x => x.IsOperation(operationOptions));
        
        _image.Mutate(c => {
            operation.Apply(operationOptions, c);
        });

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