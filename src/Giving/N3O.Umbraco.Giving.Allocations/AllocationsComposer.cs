using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Giving.Allocations;

public class AllocationsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IFundStructureAccessor, FundStructureAccessor>();
        builder.Services.AddSingleton<IPricedAmountValidator, PricedAmountValidator>();
        builder.Services.AddSingleton<IPriceCalculator, PriceCalculator>();
    }
}
