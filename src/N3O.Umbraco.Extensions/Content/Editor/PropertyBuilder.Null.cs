using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class NullPropertyBuilder : PropertyBuilder {
    public NullPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
}
