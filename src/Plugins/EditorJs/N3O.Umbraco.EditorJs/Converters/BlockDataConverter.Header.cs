using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.PublishedCache;

namespace N3O.Umbraco.EditorJs;

public class HeaderBlockDataConverter : BlockDataConverter<HeaderBlockData> {
    public HeaderBlockDataConverter(IPublishedContentCache contentCache, IPublishedMediaCache mediaCache,
                                    IPublishedUrlProvider publishedUrlProvider)
        : base(contentCache, mediaCache, publishedUrlProvider) { }
    
    protected override string TypeId => "header";

    protected override void Process(HeaderBlockData data) {
        data.Text = ConvertUmbracoLinks(data.Text);
    }
}

public class HeaderBlockData {
    public string Text { get; set; }
    public int Level { get; set; }
}