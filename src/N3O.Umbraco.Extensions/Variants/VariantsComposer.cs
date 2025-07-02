using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Variants;

public class VariantsComposer : IComposer {
    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped<IVariations, Variations>();
    }
}