using N3O.Umbraco.Authentication.Auth0.Extensions;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace DemoSite;

public class DemoSiteComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.AddAuth0MemberExternalLogins();
    }
}