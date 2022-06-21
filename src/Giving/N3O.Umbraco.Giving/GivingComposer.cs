using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Giving;

public class GivingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(GivingConstants.ApiName);
        
        builder.Services.AddSingleton<IFundStructureAccessor, FundStructureAccessor>();
        builder.Services.AddSingleton<IPricedAmountValidator, PricedAmountValidator>();
        builder.Services.AddSingleton<IPriceCalculator, PriceCalculator>();
    }
}
