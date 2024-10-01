using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crm.Engage;

public class EngageComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped(typeof(ClientFactory<>));
        builder.Services.AddSingleton<CloudUrlAccessor>();
        builder.Services.AddScoped<IAccountManager, EngageAccountManager>();
        builder.Services.AddScoped<ICrowdfunderManager, EngageCrowdfunderManager>();
        builder.Services.AddSingleton<ISubscriptionAccessor, SubscriptionAccessor>();
    }
}