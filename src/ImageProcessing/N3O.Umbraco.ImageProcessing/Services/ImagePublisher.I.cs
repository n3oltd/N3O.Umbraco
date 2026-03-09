using N3O.Umbraco.ImageProcessing.Models;
using N3O.Umbraco.Plugins.Lookups;
using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.ImageProcessing;

public interface IImagePublisher {
    PublishedImage Publish(Action<CacheKeyBuilder> cacheKeyBuilderAction,
                           Func<IImageBuilder, IFluentImageBuilder> imageBuilderAction,
                           ImageFormat format,
                           bool forcePublish = false);
    
    PublishedImage Publish(string cacheKey,
                           Func<IImageBuilder, IFluentImageBuilder> imageBuilderAction,
                           ImageFormat format,
                           bool forcePublish = false);
}