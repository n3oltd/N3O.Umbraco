using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing;

public class ImageBuilder : IImageBuilder {
    private readonly MediaFileManager _mediaFileManager;

    public ImageBuilder(MediaFileManager mediaFileManager) {
        _mediaFileManager = mediaFileManager;
    }
    
    public IFluentImageBuilder Create(int width, int height, Color backgroundColor) {
        return new FluentImageBuilder(_mediaFileManager, new Image<Rgba32>(width, height, backgroundColor));
    }

    public IFluentImageBuilder Create(Size size, Color backgroundColor) {
        return Create(size.Width, size.Height, backgroundColor);
    }

    public IFluentImageBuilder Create(string srcPath) {
        var stream = _mediaFileManager.FileSystem.OpenFile(srcPath);

        var image = Image.Load(stream);

        return new FluentImageBuilder(_mediaFileManager, image);
    }
}