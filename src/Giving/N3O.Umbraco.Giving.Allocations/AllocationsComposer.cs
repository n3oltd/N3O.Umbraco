using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Giving.Allocations;

public class AllocationsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IFundStructureAccessor, FundStructureAccessor>();
        builder.Services.AddSingleton<IPricedAmountValidator, PricedAmountValidator>();
        builder.Services.AddSingleton<IPriceCalculator, PriceCalculator>();
        
        RegisterAll(t => t.ImplementsInterface<IAllocationExtensionRequestBinder>(),
                    t => builder.Services.AddTransient(typeof(IAllocationExtensionRequestBinder), t));
        
        RegisterAll(t => t.ImplementsInterface<IAllocationExtensionRequestValidator>(),
                    t => builder.Services.AddTransient(typeof(IAllocationExtensionRequestValidator), t));
    }
}
