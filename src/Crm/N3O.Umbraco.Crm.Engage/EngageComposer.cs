using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crm.Engage;

public class EngageComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(EngageConstants.ApiName);
        
        builder.Services.AddScoped(typeof(ClientFactory<>));
        builder.Services.AddSingleton<CloudUrlAccessor>();
        builder.Services.AddScoped<IUserDirectoryIdAccessor, UserDirectoryIdAccessor>();
    }
}