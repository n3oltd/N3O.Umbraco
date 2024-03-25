using N3O.Umbraco.Content;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.ImageProcessing.Content;

public class ImagePresetContent : UmbracoContent<ImagePresetContent> {
    public IEnumerable<IPublishedElement> Operations => GetNestedAs(x => x.Operations);
}