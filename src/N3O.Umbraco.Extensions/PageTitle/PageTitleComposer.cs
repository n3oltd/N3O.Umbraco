using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.PageTitle;

public class PageTitleComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<IPageTitleProvider>(),
                    t => builder.Services.AddTransient(typeof(IPageTitleProvider), t));
    }
}
