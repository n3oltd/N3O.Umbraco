using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.EditorJs;

public class HeaderBlockDataConverter : BlockDataConverter<HeaderBlockData> {
    public HeaderBlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                    IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, publishedUrlProvider) { }
    
    protected override string TypeId => "header";
}

public class HeaderBlockData {
    public string Text { get; set; }
    public int Level { get; set; }
}