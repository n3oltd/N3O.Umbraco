using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Cloud;

public class CloudComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped(typeof(ClientFactory<>));
        builder.Services.AddSingleton<ICdnClient, CdnClient>();
        builder.Services.AddSingleton<ICloudUrl, CloudUrl>();
        builder.Services.AddSingleton<IOrganizationInfoAccessor, OrganizationInfoAccessor>();
        builder.Services.AddSingleton<INisab, Nisab>();
        builder.Services.AddSingleton<ISubscriptionAccessor, SubscriptionAccessor>();
    }
}