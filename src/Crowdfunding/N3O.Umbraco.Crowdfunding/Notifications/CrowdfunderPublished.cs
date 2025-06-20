using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CrowdfunderPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IBackgroundJob _backgroundJob;

    public CrowdfunderPublished(IBackgroundJob backgroundJob) {
        _backgroundJob = backgroundJob;
    }

    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            var isCampaign = content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias);
            var isFundraiser = content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias);

            if (isCampaign || isFundraiser) {
                var crowdfunderType = isCampaign ? CrowdfunderTypes.Campaign : CrowdfunderTypes.Fundraiser;

                _backgroundJob.EnqueueCrowdfunderUpdated(content.Key, crowdfunderType);
            }
        }

        return Task.CompletedTask;
    }
}