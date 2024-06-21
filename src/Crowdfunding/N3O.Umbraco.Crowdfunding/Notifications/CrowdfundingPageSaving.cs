using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CrowdfundingPageSaving : INotificationAsyncHandler<ContentSavingNotification> {
    // TODO This should be settings the name of the content node in Umbraco to: "{PageName} ({slug}) so the user
    // can search for either of these.
    public async Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        return;
    }
}