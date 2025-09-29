using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.BackOffice.Security;

namespace N3O.Umbraco.Antiforgery;

public class AntiforgeryComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IBackOfficeAntiforgery, OurBackofficeAntiforgery>();
    }
}
