using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(PlatformsConstants.BackOfficeApiName);

        builder.Services.AddScoped<IPlatformsPageAccessor, PlatformsPageAccessor>();
        
        RegisterAll(t => t.ImplementsInterface<IPreviewTagGenerator>(),
                    t => builder.Services.AddTransient(typeof(IPreviewTagGenerator), t));
    }
}