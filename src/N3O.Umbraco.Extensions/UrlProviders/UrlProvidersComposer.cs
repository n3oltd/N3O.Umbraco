using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.UrlProviders {
    public class UrlProvidersComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            RegisterAll(t => t.ImplementsInterface<IUrlProvider>(),
                        (t, index) => builder.UrlProviders().Insert(index, t));
        }
    }
}
