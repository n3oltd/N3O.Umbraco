using N3O.Umbraco.Content;
using N3O.Umbraco.ImageProcessing.Operations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing;

public class ImageBuilder : IImageBuilder {
    private readonly IEnumerable<IImageOperation> _allOperations;
    private readonly MediaFileManager _mediaFileManager;
    private readonly IContentLocator _contentLocator;

    public ImageBuilder(IEnumerable<IImageOperation> allOperations,
                        MediaFileManager mediaFileManager,
                        IContentLocator contentLocator) {
        _allOperations = allOperations;
        _mediaFileManager = mediaFileManager;
        _contentLocator = contentLocator;
    }
    
    public IFluentImageBuilder Create(int width, int height, Color backgroundColor) {
        return new FluentImageBuilder(_allOperations,
                                      _mediaFileManager,
                                      _contentLocator,
                                      new Image<Rgba32>(width, height, backgroundColor));
    }

    public IFluentImageBuilder Create(Size size, Color backgroundColor) {
        return Create(size.Width, size.Height, backgroundColor);
    }

    public IFluentImageBuilder Create(string srcPath) {
        var stream = _mediaFileManager.FileSystem.OpenFile(srcPath);

        var image = Image.Load(stream);

        return new FluentImageBuilder(_allOperations, _mediaFileManager, _contentLocator, image);
    }
}