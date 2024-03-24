using N3O.Umbraco.ImageProcessing.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.ImageProcessing;

public class ImageProcessor : IImageProcessor {
    private readonly Image<Rgba32> _image;

    public ImageProcessor(Image<Rgba32> image) {
        _image = image;
    }

    public ImageProcessor Combine(params ImageLayer[] layers) {
        return Combine(layers.AsEnumerable());
    }
    
    public ImageProcessor Combine(IEnumerable<ImageLayer> layers) {
        foreach (var layer in layers) {
            var options = new GraphicsOptions();
            options.BlendPercentage = 1f;
            options.ColorBlendingMode = PixelColorBlendingMode.Normal;
            
            _image.Mutate(operation => {
                operation.DrawImage(layer.Image, layer.Rectangle, options);
            });
        }

        return this;
    }
}