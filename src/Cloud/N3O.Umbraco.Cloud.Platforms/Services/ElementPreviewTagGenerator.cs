using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class ElementPreviewTagGenerator : PreviewTagGenerator {
    protected ElementPreviewTagGenerator(ICdnClient cdnClient, IJsonProvider jsonProvider)
        : base(cdnClient, jsonProvider) { }
    
    protected abstract ElementType ElementType { get; }

    protected override string ContentTypeAlias => ElementType.ContentTypeAlias;
}