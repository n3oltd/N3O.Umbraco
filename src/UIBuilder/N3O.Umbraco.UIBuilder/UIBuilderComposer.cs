using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Konstrukt.Extensions;

namespace N3O.Umbraco.UIBuilder;

public class UIBuilderComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.AddKonstrukt();
    }
}
