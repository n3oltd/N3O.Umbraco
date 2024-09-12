using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crm.Engage;

public class EngageComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped(typeof(ClientFactory<>));
        builder.Services.AddSingleton<CloudUrlAccessor>();
        builder.Services.AddScoped<IUserDirectoryIdAccessor, UserDirectoryIdAccessor>();
        
        builder.Services.AddScoped<IAccountManager, EngageAccountManager>();
    }
}