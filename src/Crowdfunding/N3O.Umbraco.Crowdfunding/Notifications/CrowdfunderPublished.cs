using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CrowdfunderPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        // TODO we will probably need to change this to publishing or saving, but in any case we
        // will check if the campaign has been renamed, and if it has we will send a background job
        // command to execute a SQL UPDATE CampaignName WHERE CampaignId = '' query on the contributions
        // tables. We can actually get away with executing this updatein all cases as the volume will be low,
        // we don't really care if it ends up being a no op.
        // We will also need to do this for fundraisers and teams.

        return Task.CompletedTask;
    }
}