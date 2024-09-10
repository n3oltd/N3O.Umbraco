using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.ImageProcessing.Content;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.ImageProcessing.Operations;

public abstract class ImageOperation<TElement> : IImageOperation where TElement : UmbracoElement<TElement> {
    private readonly MediaFileManager _mediaFileManager;

    protected ImageOperation(MediaFileManager mediaFileManager) {
        _mediaFileManager = mediaFileManager;
    }
    
    public void Apply(IPublishedElement options, IImageProcessingContext image, ImagePresetContent preset) {
        Apply(options.As<TElement>(preset.Content()), image);
    }

    public bool IsOperation(IPublishedElement options) {
        return options.ContentType.Alias.EqualsInvariant(AliasHelper<TElement>.ContentTypeAlias());
    }

    protected Image LoadMediaImage(string srcPath) {
        var stream = _mediaFileManager.FileSystem.OpenFile(srcPath);

        return Image.Load(stream);
    }
    
    protected abstract void Apply(TElement options, IImageProcessingContext image);
}