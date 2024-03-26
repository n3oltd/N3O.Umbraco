using Flurl;
using N3O.Umbraco.Plugins.Lookups;
using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.ImageProcessing;

public interface IImagePublisher {
    Url Publish(Action<CacheKeyBuilder> cacheKeyBuilderAction,
                Func<IImageBuilder, IFluentImageBuilder> imageBuilderAction,
                ImageFormat format);
    
    Url Publish(string cacheKey, Func<IImageBuilder, IFluentImageBuilder> imageBuilderAction, ImageFormat format);
}