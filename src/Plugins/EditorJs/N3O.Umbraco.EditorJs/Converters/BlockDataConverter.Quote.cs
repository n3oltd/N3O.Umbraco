using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.PublishedCache;

namespace N3O.Umbraco.EditorJs;

public class QuoteBlockDataConverter : BlockDataConverter<QuoteBlockData> {
    public QuoteBlockDataConverter(IPublishedContentCache contentCache, IPublishedMediaCache mediaCache,
                                   IPublishedUrlProvider publishedUrlProvider)
        : base(contentCache, mediaCache, publishedUrlProvider) { }
    
    protected override string TypeId => "quote";

    protected override void Process(QuoteBlockData data) {
        data.Text = ConvertUmbracoLinks(data.Text);
        data.Caption = ConvertUmbracoLinks(data.Caption);
    }
}

public class QuoteBlockData {
    public string Text { get; set; }
    public string Caption { get; set; }
    public string Alignment { get; set; }
}