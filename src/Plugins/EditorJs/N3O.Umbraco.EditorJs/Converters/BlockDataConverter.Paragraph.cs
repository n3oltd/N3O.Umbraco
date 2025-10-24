using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.EditorJs;

public class ParagraphBlockDataConverter : BlockDataConverter<ParagraphBlockData, ParagraphTunesData> {
    public ParagraphBlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                       IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, publishedUrlProvider) { }

    protected override string TypeId => "paragraph";
}

public class ParagraphBlockData {
    public string Text { get; set; }
}

public class ParagraphTunesData {
    public ParagraphAlignmentTune AlignmentTune { get; set; }
}

public class ParagraphAlignmentTune {
    public string Alignment { get; set; }
}