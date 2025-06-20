using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CampaignSaving : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CampaignSaving(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        if (!_webHostEnvironment.IsProduction()) {
            foreach (var content in notification.SavedEntities) {
                if (content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias)) {
                    content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, CrowdfunderStatuses.Active.Name);
                }
            }
        }
    }
}