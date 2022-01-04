using N3O.Umbraco.Composing;
using N3O.Umbraco.TextResourceEditor.DataTypes;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.TextResourceEditor {
    public class TextResourceEditorComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.PropertyValueConverters().Append<TemplateTextEditorValueConverter>();
        }
    }
}