using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Authentication;

public class AuthenticationComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(AuthenticationConstants.ApiName);
    }
}