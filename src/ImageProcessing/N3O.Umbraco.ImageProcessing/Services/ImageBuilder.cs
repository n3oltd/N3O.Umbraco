using N3O.Umbraco.ImageProcessing.Operations;
using N3O.Umbraco.Utilities;
using NodaTime;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing;

public class ImageBuilder : IImageBuilder {
    private readonly IEnumerable<IImageOperation> _allOperations;
    private readonly IClock _clock;
    private readonly IUrlBuilder _urlBuilder;
    private readonly MediaFileManager _mediaFileManager;

    public ImageBuilder(IEnumerable<IImageOperation> allOperations, IClock clock, IUrlBuilder urlBuilder, MediaFileManager mediaFileManager) {
        _allOperations = allOperations;
        _clock = clock;
        _urlBuilder = urlBuilder;
        _mediaFileManager = mediaFileManager;
    }
    
    public IFluentImageBuilder Create(int width, int height) {
        return new FluentImageBuilder(_allOperations, _clock, _urlBuilder, _mediaFileManager, width, height);
    }

    public IFluentImageBuilder Create(Size size) {
        return Create(size.Width, size.Height);
    }

    public async Task<IFluentImageBuilder> CreateAsync(string srcPath) {
        var stream = _mediaFileManager.FileSystem.OpenFile(srcPath);

        var image = await Image.LoadAsync(stream);

        return new FluentImageBuilder(_allOperations, _clock, _urlBuilder, _mediaFileManager, image);
    }
}