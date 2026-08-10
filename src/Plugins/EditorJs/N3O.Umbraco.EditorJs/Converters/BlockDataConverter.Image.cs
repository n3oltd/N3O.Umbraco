using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.PublishedCache;

namespace N3O.Umbraco.EditorJs;

public class ImageBlockDataConverter : BlockDataConverter<ImageBlockData> {
    protected override string TypeId => "image";
    
    public ImageBlockDataConverter(IPublishedContentCache contentCache, IPublishedMediaCache mediaCache,
                                   IPublishedUrlProvider publishedUrlProvider)
        : base(contentCache, mediaCache, publishedUrlProvider) { }
}

public class ImageBlockData {
    public string Url { get; set; }
    public string Alt { get; set; }
    public Udi Udi { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
}