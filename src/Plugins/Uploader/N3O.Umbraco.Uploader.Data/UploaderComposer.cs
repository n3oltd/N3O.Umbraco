using N3O.Umbraco.Composing;
using N3O.Umbraco.Data.Converters;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Uploader.Data;

public class UploaderComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        UploadDataTypes.Register(UploaderConstants.PropertyEditorAlias);
    }
}
