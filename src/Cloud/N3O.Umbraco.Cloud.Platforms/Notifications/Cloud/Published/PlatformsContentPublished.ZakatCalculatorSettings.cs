using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ZakatCalculatorSettingsPublished : CloudContentPublished {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public ZakatCalculatorSettingsPublished(ISubscriptionAccessor subscriptionAccessor,
                                            ICloudUrl cloudUrl,
                                            IBackgroundJob backgroundJob,
                                            Lazy<IContentLocator> contentLocator,
                                            IUmbracoMapper mapper,
                                            ILogger<CloudContentPublished> logger)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentLocator = contentLocator;
        _mapper = mapper;
    }

    protected override object GetBody(IContent content) {
        var settingsContent = _contentLocator.Value.ById<ZakatCalculatorSettingsContent>(content.Key);

        var settingsReq = new ZakatPlatformsSettingsReq();
        settingsReq.Calculator = _mapper.Map<ZakatCalculatorSettingsContent, ZakatCalculatorSettingsReq>(settingsContent);
        
        return settingsReq;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsZakatCalculatorSettings();
    }

    protected override string HookId => PlatformsConstants.WebhookIds.ZakatSettings;
}