using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.EditorJs;

public class EmbedBlockDataConverter : BlockDataConverter<EmbedBlockData> {
    public EmbedBlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                   IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, publishedUrlProvider) { }
    
    protected override string TypeId => "embed";
}

public class EmbedBlockData {
    public string Service { get; set; }
    public string Source { get; set; }
    public string Embed { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public string Caption { get; set; }
}