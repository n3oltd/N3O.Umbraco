using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Uploader.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Threading.Tasks;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.ImageProcessing.Operations;

public abstract class ImageOperation<TElement> : IImageOperation where TElement : UmbracoElement<TElement> {
    private readonly MediaFileManager _mediaFileManager;

    protected ImageOperation(MediaFileManager mediaFileManager) {
        _mediaFileManager = mediaFileManager;
    }
    
    public async Task ApplyAsync(IPublishedElement options, IImageProcessingContext image) {
        await ApplyAsync(options.As<TElement>(), image);
    }

    public bool IsOperation(IPublishedElement options) {
        return options.ContentType.Alias.EqualsInvariant(AliasHelper<TElement>.ContentTypeAlias());
    }

    protected Task<Image> LoadImageAsync(FileUpload upload) {
        var stream = _mediaFileManager.FileSystem.OpenFile(upload.Src);

        return Image.LoadAsync(stream);
    }
    
    protected abstract Task ApplyAsync(TElement options, IImageProcessingContext image);
}