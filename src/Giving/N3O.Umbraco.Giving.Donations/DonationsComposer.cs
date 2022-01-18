using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Giving.Donations {
    public class DonationsComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddOpenApiDocument(DonationConstants.ApiName);
        }
    }
}
