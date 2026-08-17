using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.PublishedCache;

namespace N3O.Umbraco.EditorJs;

public class RawHtmlBlockDataConverter : BlockDataConverter<RawHtmlBlockData> {
    public RawHtmlBlockDataConverter(IPublishedContentCache contentCache, IPublishedMediaCache mediaCache,
                                     IPublishedUrlProvider publishedUrlProvider)
        : base(contentCache, mediaCache, publishedUrlProvider) { }

    protected override string TypeId => "raw";
}

public class RawHtmlBlockData {
    public string Html { get; set; }
}