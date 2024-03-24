using N3O.Umbraco.ImageProcessing.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.ImageProcessing;

public interface IImageProcessor {
    ImageProcessor Combine(params ImageLayer[] layers);
    ImageProcessor Combine(IEnumerable<ImageLayer> layers);
}