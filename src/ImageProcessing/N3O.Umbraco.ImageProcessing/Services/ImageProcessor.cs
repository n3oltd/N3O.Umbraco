using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.ImageProcessing.Content;
using N3O.Umbraco.ImageProcessing.Models;
using N3O.Umbraco.ImageProcessing.Operations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.ImageProcessing;

public class ImageProcessor : IImageProcessor {
    private readonly IEnumerable<IImageOperation> _allOperations;
    private readonly IContentLocator _contentLocator;
    private readonly MediaFileManager _mediaFileManager;
    private readonly Image _image;

    public ImageProcessor(IEnumerable<IImageOperation> allOperations,
                          IContentLocator contentLocator,
                          MediaFileManager mediaFileManager,
                          Image image) {
        _allOperations = allOperations;
        _contentLocator = contentLocator;
        _mediaFileManager = mediaFileManager;
        _image = image;
    }
    
    public IImageProcessor ApplyOperation(IPublishedElement options, ImagePresetContent preset) {
        var operation = _allOperations.Single(x => x.IsOperation(options));
        
        _image.Mutate(c => {
            operation.Apply(options, c, preset);
        });

        return this;
    }
    
    public IImageProcessor ApplyPreset(string presetName) {
        var preset = _contentLocator.Single<ImagePresetContent>(x => x.Content().Name.EqualsInvariant(presetName));

        foreach (var operation in preset.Operations) {
            ApplyOperation(operation, preset);
        }
        
        return this;
    }

    public IImageProcessor Combine(params ImageLayer[] layers) {
        return Combine(layers.AsEnumerable());
    }
    
    public IImageProcessor Combine(IEnumerable<ImageLayer> layers) {

        foreach (var layer in layers) {
            var image = (layer.Image ?? LoadMediaImage(layer.SrcPath)).Clone(o => o.Resize(layer.Size)); 
            
            _image.Mutate(operation => {
                operation.DrawImage(image, layer.Point, 1f);
            });
        }

        return this;
    }

    public IImageProcessor Mutate(Action<IImageProcessingContext> action) {
        _image.Mutate(action);

        return this;
    }

    private Image LoadMediaImage(string srcPath) {
        var stream = _mediaFileManager.FileSystem.OpenFile(srcPath);

        var image = Image.Load(stream);

        return image;
    }
}