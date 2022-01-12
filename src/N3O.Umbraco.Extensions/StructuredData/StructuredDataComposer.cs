using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.StructuredData {
    public class StructuredDataComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            RegisterAll(t => t.ImplementsInterface<IStructuredDataProvider>(),
                        t => builder.Services.AddTransient(typeof(IStructuredDataProvider), t));
        }
    }
}
