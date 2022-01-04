using N3O.Umbraco.Composing;
using N3O.Umbraco.Uploader.DataTypes;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Uploader {
    public class UploaderComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.PropertyValueConverters().Append<UploaderValueConverter>();
        }
    }
}