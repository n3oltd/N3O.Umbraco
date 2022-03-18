using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments.Bambora {
    public class BamboraComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddOpenApiDocument(BamboraConstants.ApiName);
        }
    }
}