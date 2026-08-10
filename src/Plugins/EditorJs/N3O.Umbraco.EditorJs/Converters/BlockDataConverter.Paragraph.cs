using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.PublishedCache;

namespace N3O.Umbraco.EditorJs;

public class ParagraphBlockDataConverter : BlockDataConverter<ParagraphBlockData> {
    public ParagraphBlockDataConverter(IPublishedContentCache contentCache, IPublishedMediaCache mediaCache,
                                       IPublishedUrlProvider publishedUrlProvider)
        : base(contentCache, mediaCache, publishedUrlProvider) { }

    protected override string TypeId => "paragraph";
    
    protected override void Process(ParagraphBlockData data) {
        data.Text = ConvertUmbracoLinks(data.Text);
        data.Text = DecodePlatformsElements(data.Text);
    }
}

public class ParagraphBlockData {
    public string Text { get; set; }
}