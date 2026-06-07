using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.PublishedCache;

namespace N3O.Umbraco.EditorJs;

public class CodeBlockDataConverter : BlockDataConverter<CodeBlockData> {
    public CodeBlockDataConverter(IPublishedContentCache contentCache, IPublishedMediaCache mediaCache,
                                  IPublishedUrlProvider publishedUrlProvider)
        : base(contentCache, mediaCache, publishedUrlProvider) { }
    
    protected override string TypeId => "code";
}

public class CodeBlockData {
    public string Code { get; set; }
}