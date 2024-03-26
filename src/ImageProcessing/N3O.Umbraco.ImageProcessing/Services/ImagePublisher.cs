using Flurl;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Plugins.Lookups;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Concurrent;
using System.IO;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing;

public class ImagePublisher : IImagePublisher {
    private static readonly ConcurrentDictionary<string, Url> CachedUrls = new();
    
    private readonly MediaFileManager _mediaFileManager;
    private readonly IImageBuilder _imageBuilder;
    private readonly IUrlBuilder _urlBuilder;

    public ImagePublisher(MediaFileManager mediaFileManager, IImageBuilder imageBuilder, IUrlBuilder urlBuilder) {
        _mediaFileManager = mediaFileManager;
        _imageBuilder = imageBuilder;
        _urlBuilder = urlBuilder;
    }

    public Url Publish(Action<CacheKeyBuilder> cacheKeyBuilderAction,
                       Func<IImageBuilder, IFluentImageBuilder> imageBuilderAction,
                       ImageFormat format) {
        var cacheKeyBuilder = CacheKeyBuilder.Create();

        cacheKeyBuilderAction(cacheKeyBuilder);
        
        var cacheKey = cacheKeyBuilder.Build();

        return Publish(cacheKey, imageBuilderAction, format);
    }
    
    public Url Publish(string cacheKey,
                       Func<IImageBuilder, IFluentImageBuilder> imageBuilderAction,
                       ImageFormat format) {
        return CachedUrls.GetOrAddAtomic(cacheKey, () => {
            var url = TryFind(cacheKey, format) ?? SaveAndPublish(cacheKey, imageBuilderAction, format);

            return url;
        });
    }
    
    private Url TryFind(string cacheKey, ImageFormat format) {
        var mediaPath = GetMediaPath(cacheKey, format);

        if (_mediaFileManager.FileSystem.FileExists(mediaPath)) {
            return GetUrl(mediaPath);
        } else {
            return null;
        }
    }
    
    private Url SaveAndPublish(string cacheKey,
                               Func<IImageBuilder, IFluentImageBuilder> imageBuilderAction,
                               ImageFormat format) {
        var imageBuilder = imageBuilderAction(_imageBuilder);

        using (var stream = new MemoryStream()) {
            imageBuilder.Do(x => x.Save(stream, format.Encoder));
            
            stream.Rewind();

            return Publish(stream, cacheKey, format);
        };
    }
    
    private Url Publish(Stream stream, string cacheKey, ImageFormat format) {
        var mediaPath = GetMediaPath(cacheKey, format);

        _mediaFileManager.FileSystem.AddFile(mediaPath, stream);

        return GetUrl(mediaPath);
    }

    private string GetMediaPath(string cacheKey, ImageFormat format) {
        return $"/media/cdn/{cacheKey}{format.Extension}";
    }
    
    private Url GetUrl(string mediaPath) {
        var url = _urlBuilder.Root().AppendPathSegments(mediaPath);

        return url;
    }
}