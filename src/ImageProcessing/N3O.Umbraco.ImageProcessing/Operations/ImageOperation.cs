using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using SixLabors.ImageSharp.Processing;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.ImageProcessing.Operations;

public abstract class ImageOperation<TElement> : IImageOperation where TElement : UmbracoElement<TElement> {
    public void Apply(IPublishedElement options, IImageProcessingContext image) {
        Apply(options.As<TElement>(), image);
    }

    public bool IsOperation(IPublishedElement options) {
        return options.ContentType.Alias.EqualsInvariant(AliasHelper<TElement>.ContentTypeAlias());
    }

    protected abstract void Apply(TElement options, IImageProcessingContext image);
}