using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.EditorJs;

public class CodeBlockDataConverter : BlockDataConverter<CodeBlockData, None> {
    public CodeBlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                  IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, publishedUrlProvider) { }
    
    protected override string TypeId => "code";
}

public class CodeBlockData {
    public string Code { get; set; }
}