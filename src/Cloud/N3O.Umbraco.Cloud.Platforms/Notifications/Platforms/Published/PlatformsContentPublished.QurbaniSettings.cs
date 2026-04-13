using Microsoft.Extensions.Logging;
using N3O.Umbraco.Blocks;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class QurbaniSettingsPublished : CloudContentPublished {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;
    private readonly IBlocksRenderer _blocksRenderer;

    public QurbaniSettingsPublished(ISubscriptionAccessor subscriptionAccessor,
                                    ICloudUrl cloudUrl,
                                    IBackgroundJob backgroundJob,
                                    Lazy<IContentLocator> contentLocator,
                                    IUmbracoMapper mapper,
                                    ILogger<CloudContentPublished> logger,
                                    IBlocksRenderer blocksRenderer)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentLocator = contentLocator;
        _mapper = mapper;
        _blocksRenderer = blocksRenderer;
    }

    protected override async Task<object> GetBodyAsync(IContent content) {
        var seasonContent = _contentLocator.Value.Single<QurbaniSeasonContent>();

        var settingsReq = _mapper.Map<QurbaniSeasonContent, QurbaniSeasonReq>(seasonContent);

        return settingsReq;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsQurbaniSeasonSettings() ||
               content.IsQurbaniSeasonOfferSettings() ||
               content.IsQurbaniSeasonLocationSettings() ||
               content.IsQurbaniSeasonGroupSettings();
    }

    protected override string HookId => PlatformsConstants.WebhookIds.QurbaniSettings;
}
