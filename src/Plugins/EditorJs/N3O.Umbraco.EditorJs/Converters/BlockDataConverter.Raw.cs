using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.EditorJs;

public class RawHtmlBlockDataConverter : BlockDataConverter<RawHtmlBlockData, None> {
    public RawHtmlBlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                     IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, publishedUrlProvider) { }

    protected override string TypeId => "raw";
}

public class RawHtmlBlockData {
    public string Html { get; set; }
}