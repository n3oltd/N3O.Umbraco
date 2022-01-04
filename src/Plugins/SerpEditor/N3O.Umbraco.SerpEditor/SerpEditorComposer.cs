using N3O.Umbraco.Composing;
using N3O.Umbraco.SerpEditor.DataTypes;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.SerpEditor {
    public class SerpEditorComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.PropertyValueConverters().Append<SerpEditorValueConverter>();
        }
    }
}