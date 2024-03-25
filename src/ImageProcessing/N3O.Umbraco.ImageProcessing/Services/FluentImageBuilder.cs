using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.ImageProcessing.Operations;
using N3O.Umbraco.Plugins.Extensions;
using N3O.Umbraco.Utilities;
using NodaTime;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing;

public class FluentImageBuilder : IFluentImageBuilder {
    private readonly IClock _clock;
    private readonly IUrlBuilder _urlBuilder;
    private readonly MediaFileManager _mediaFileManager;
    private readonly Image _image;

    public FluentImageBuilder(IEnumerable<IImageOperation> allOperations,
                              IClock clock,
                              IUrlBuilder urlBuilder,
                              MediaFileManager mediaFileManager,
                              IContentCache contentCache,
                              Image image) {
        _clock = clock;
        _urlBuilder = urlBuilder;
        _mediaFileManager = mediaFileManager;
        _image = image;

        Processor = new ImageProcessor(allOperations, contentCache, _image);
    }

    public Image LoadMediaImage(string srcPath) {
        var stream = _mediaFileManager.FileSystem.OpenFile(srcPath);

        var image = Image.Load(stream);

        return image;
    }

    public string PublishToUrl(Action<Image, Stream> save, string filename) {
        using (var stream = new MemoryStream()) { 
            save(_image, stream);
            
            stream.Rewind();
            
            var url = PublishToUrl(stream, filename);

            return url;
        }
    }

    public T Save<T>(Func<Image, T> save) {
        var res = save(_image);

        return res;
    }
    
    private string PublishToUrl(Stream stream, string filename) {
        var instant = _clock.GetCurrentInstant();
        var mediaPath = filename.GetMediaUrlPath(instant);

        _mediaFileManager.FileSystem.AddFile(mediaPath, stream);

        var url = _urlBuilder.Root().AppendPathSegments(mediaPath);

        return url;
    }

    public IImageProcessor Processor { get; }
}