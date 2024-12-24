using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cells.Content;

public class CellsPropertyBuilder : PropertyBuilder {
    public CellsPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }

    public void Set(object[][] value) {
        Value = value;
    }
}
