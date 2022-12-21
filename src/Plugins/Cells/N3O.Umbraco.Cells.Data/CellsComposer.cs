using N3O.Umbraco.Composing;
using N3O.Umbraco.Data.Converters;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Cells.Data;

public class CellsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        UploadDataTypes.Register(CellsConstants.PropertyEditorAlias);
    }
}
