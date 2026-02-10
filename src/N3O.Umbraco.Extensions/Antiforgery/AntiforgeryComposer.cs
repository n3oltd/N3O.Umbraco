using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.BackOffice.Security;
using WebConstants = Umbraco.Cms.Core.Constants;

namespace N3O.Umbraco.Antiforgery;

public class AntiforgeryComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.Configure<AntiforgeryOptions>(options => {
            options.HeaderName = WebConstants.Web.AngularHeadername;
            options.Cookie.Name = WebConstants.Web.CsrfValidationCookieName;
        });

        builder.Services.AddSingleton<IBackOfficeAntiforgery, OurBackofficeAntiforgery>();
    }
}
