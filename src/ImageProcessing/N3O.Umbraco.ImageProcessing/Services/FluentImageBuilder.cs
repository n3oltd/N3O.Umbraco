using N3O.Umbraco.Extensions;
using N3O.Umbraco.ImageProcessing.Operations;
using N3O.Umbraco.Plugins.Extensions;
using N3O.Umbraco.Utilities;
using NodaTime;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
                              int width,
                              int height) {//TODO call other ctor from this one
        _clock = clock;
        _urlBuilder = urlBuilder;
        _mediaFileManager = mediaFileManager;
        _image = new Image<Rgba32>(width, height);

        Processor = new ImageProcessor(allOperations, _image);
    }

    public FluentImageBuilder(IEnumerable<IImageOperation> allOperations,
                              IClock clock,
                              IUrlBuilder urlBuilder,
                              MediaFileManager mediaFileManager,
                              Image image) {
        _clock = clock;
        _urlBuilder = urlBuilder;
        _mediaFileManager = mediaFileManager;
        _image = image;

        Processor = new ImageProcessor(allOperations, _image);
    }

    public async Task<Image> LoadImageAsync(string srcPath) {
        var stream = _mediaFileManager.FileSystem.OpenFile(srcPath);

        var image = await Image.LoadAsync(stream);

        return image;
    }

    public async Task<string> PublishToUrlAsync(Func<Image, Stream, Task> saveAsync, string filename) {
        using (var stream = new MemoryStream()) { 
            await saveAsync(_image, stream);
            
            stream.Rewind();
            
            var url = await PublishToUrlAsync(stream, filename);

            return url;
        }
    }

    public async Task<T> SaveAsync<T>(Func<Image, Task<T>> saveAsync) {
        var res = await saveAsync(_image);

        return res;
    }
    
    private async Task<string> PublishToUrlAsync(Stream stream, string filename) {
        var instant = _clock.GetCurrentInstant();
        var mediaPath = filename.GetMediaUrlPath(instant);

        _mediaFileManager.FileSystem.AddFile(mediaPath, stream);

        var url = _urlBuilder.Root().AppendPathSegments(mediaPath);

        return url;
    }

    public IImageProcessor Processor { get; }
}