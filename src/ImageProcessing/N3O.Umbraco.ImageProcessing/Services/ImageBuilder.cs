using N3O.Umbraco.Utilities;
using NodaTime;
using SixLabors.ImageSharp;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing;

public class ImageBuilder : IImageBuilder {
    private readonly IClock _clock;
    private readonly IUrlBuilder _urlBuilder;
    private readonly MediaFileManager _mediaFileManager;

    public ImageBuilder(IClock clock, IUrlBuilder urlBuilder, MediaFileManager mediaFileManager) {
        _clock = clock;
        _urlBuilder = urlBuilder;
        _mediaFileManager = mediaFileManager;
    }
    
    public IFluentImageBuilder Create(int width, int height) {
        return new FluentImageBuilder(_clock, _urlBuilder, _mediaFileManager, width, height);
    }

    public IFluentImageBuilder Create(Size size) {
        return Create(size.Width, size.Height);
    }
}