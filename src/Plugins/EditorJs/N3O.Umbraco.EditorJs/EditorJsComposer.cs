using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.EditorJs.DataTypes;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.EditorJs;

public class EditorJsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.PropertyValueConverters().Append<EditorJsValueConverter>();
        
        RegisterAll(t => t.ImplementsInterface<IBlockDataConverter>(),
                    t => builder.Services.AddTransient(typeof(IBlockDataConverter), t));
    }
}
