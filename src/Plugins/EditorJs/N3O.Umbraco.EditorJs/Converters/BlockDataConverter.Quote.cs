using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.EditorJs;

public class QuoteBlockDataConverter : BlockDataConverter<QuoteBlockData> {
    public QuoteBlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                   IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, publishedUrlProvider) { }
    
    protected override string TypeId => "quote";
}

public class QuoteBlockData {
    public string Text { get; set; }
    public string Caption { get; set; }
    public string Alignment { get; set; }
}