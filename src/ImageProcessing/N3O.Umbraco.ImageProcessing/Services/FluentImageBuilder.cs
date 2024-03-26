using N3O.Umbraco.Content;
using N3O.Umbraco.ImageProcessing.Operations;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing;

public class FluentImageBuilder : IFluentImageBuilder {
    private readonly MediaFileManager _mediaFileManager;
    private readonly Image _image;

    public FluentImageBuilder(IEnumerable<IImageOperation> allOperations,
                              MediaFileManager mediaFileManager,
                              IContentLocator contentLocator,
                              Image image) {
        _mediaFileManager = mediaFileManager;
        _image = image;

        Processor = new ImageProcessor(allOperations, contentLocator, mediaFileManager, _image);
    }
    
    public T Do<T>(Func<Image, T> action) {
        var res = action(_image);

        return res;
    }

    public void Do(Action<Image> action) {
        action(_image);
    }

    public Image LoadMediaImage(string srcPath) {
        var stream = _mediaFileManager.FileSystem.OpenFile(srcPath);

        var image = Image.Load(stream);

        return image;
    }

    public IImageProcessor Processor { get; }
}