using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Giving;

public class GivingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(GivingConstants.ApiName);
        
        RegisterAll(t => t.ImplementsInterface<IAllocationExtensionRequestBinder>(),
                    t => builder.Services.AddTransient(typeof(IAllocationExtensionRequestBinder), t));
        
        RegisterAll(t => t.ImplementsInterface<IAllocationExtensionRequestValidator>(),
                    t => builder.Services.AddTransient(typeof(IAllocationExtensionRequestValidator), t));
    }
}
