using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Cloud;

[ComposeAfter(typeof(LocalizationComposer))]
public class CloudComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddOpenApiDocument(CloudConstants.BackOfficeApiName);
        
        builder.Services.AddScoped(typeof(ClientFactory<>));
        builder.Services.AddSingleton<ICdnClient, CdnClient>();
        builder.Services.AddSingleton<ICloudUrl, CloudUrl>();
        builder.Services.AddSingleton<ILocalizationSettingsAccessor, PublishedLocalizationSettingsAccessor>();
        builder.Services.AddSingleton<INisab, Nisab>();
        builder.Services.AddSingleton<IOrganizationInfoAccessor, OrganizationInfoAccessor>();
        builder.Services.AddSingleton<ISubscriptionAccessor, SubscriptionAccessor>();
    }
}