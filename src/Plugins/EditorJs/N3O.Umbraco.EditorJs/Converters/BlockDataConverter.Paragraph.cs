using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.EditorJs;

public class ParagraphBlockDataConverter : BlockDataConverter<ParagraphBlockData> {
    public ParagraphBlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                       IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, publishedUrlProvider) { }

    protected override string TypeId => "paragraph";
    
    protected override void Process(ParagraphBlockData data) {
        data.Text = ConvertUmbracoLinks(data.Text);
        data.Text = DecodePlatformsElements(data.Text);
    }
}

public class ParagraphBlockData {
    public string Text { get; set; }
}