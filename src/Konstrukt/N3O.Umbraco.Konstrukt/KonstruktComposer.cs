using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Konstrukt.Extensions;

namespace N3O.Umbraco.Konstrukt;

public class KonstruktComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.AddKonstrukt();
    }
}
