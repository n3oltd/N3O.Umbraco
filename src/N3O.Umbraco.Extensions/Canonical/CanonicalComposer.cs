using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Canonical;

public class CanonicalComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<ICanonicalUrlProvider>(),
                    t => builder.Services.AddTransient(typeof(ICanonicalUrlProvider), t));
    }
}
