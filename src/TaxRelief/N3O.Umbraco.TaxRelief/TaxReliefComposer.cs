using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.TaxRelief;

public class TaxReliefComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<ITaxReliefSchemeAccessor, TaxReliefSchemeAccessor>();
    }
}
