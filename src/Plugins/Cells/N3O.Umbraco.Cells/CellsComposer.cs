using N3O.Umbraco.Cells.DataTypes;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Cells;

public class CellsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.PropertyValueConverters().Append<CellsValueConverter>();
    }
}
