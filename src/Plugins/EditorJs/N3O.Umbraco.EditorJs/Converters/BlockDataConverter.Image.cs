using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.EditorJs;

public class ImageBlockDataConverter : BlockDataConverter<ImageBlockData, None> {
    protected override string TypeId => "image";
    
    public ImageBlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                   IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, publishedUrlProvider) { }
}

public class ImageBlockData {
    public string Url { get; set; }
    public string Alt { get; set; }
    public Udi Udi { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
}